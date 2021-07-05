using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B50 RID: 6992
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06009A1D RID: 39453 RVA: 0x000669F0 File Offset: 0x00064BF0
		// (set) Token: 0x06009A1E RID: 39454 RVA: 0x002D52BC File Offset: 0x002D34BC
		private ResearchTabDef CurTab
		{
			get
			{
				return this.curTabInt;
			}
			set
			{
				if (value == this.curTabInt)
				{
					return;
				}
				this.curTabInt = value;
				Vector2 vector = this.ViewSize(this.CurTab);
				this.rightViewWidth = vector.x;
				this.rightViewHeight = vector.y;
				this.rightScrollPosition = Vector2.zero;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06009A1F RID: 39455 RVA: 0x002D530C File Offset: 0x002D350C
		private bool ColonistsHaveResearchBench
		{
			get
			{
				bool result = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].listerBuildings.ColonistsHaveResearchBench())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06009A20 RID: 39456 RVA: 0x002D534C File Offset: 0x002D354C
		public List<ResearchProjectDef> VisibleResearchProjects
		{
			get
			{
				if (this.cachedVisibleResearchProjects == null)
				{
					this.cachedVisibleResearchProjects = new List<ResearchProjectDef>(from d in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where Find.Storyteller.difficultyValues.AllowedBy(d.hideWhen) || d == Find.ResearchManager.currentProj
					select d);
				}
				return this.cachedVisibleResearchProjects;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06009A21 RID: 39457 RVA: 0x002D539C File Offset: 0x002D359C
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float b2 = this.Margin + 10f + 32f + 10f + DefDatabase<ResearchTabDef>.AllDefs.Max((ResearchTabDef tab) => this.ViewSize(tab).y) + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x06009A22 RID: 39458 RVA: 0x002D541C File Offset: 0x002D361C
		private Vector2 ViewSize(ResearchTabDef tab)
		{
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			float num = 0f;
			float num2 = 0f;
			Text.Font = GameFont.Small;
			for (int i = 0; i < visibleResearchProjects.Count; i++)
			{
				ResearchProjectDef researchProjectDef = visibleResearchProjects[i];
				if (researchProjectDef.tab == tab)
				{
					Rect rect = new Rect(0f, 0f, 140f, 0f);
					Widgets.LabelCacheHeight(ref rect, this.GetLabel(researchProjectDef) + "\n", false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f, num2 + 20f);
		}

		// Token: 0x06009A23 RID: 39459 RVA: 0x002D54E0 File Offset: 0x002D36E0
		public override void PreOpen()
		{
			base.PreOpen();
			this.selectedProject = Find.ResearchManager.currentProj;
			this.cachedVisibleResearchProjects = null;
			this.cachedUnlockedDefsGroupedByPrerequisites = null;
			if (this.CurTab == null)
			{
				if (this.selectedProject != null)
				{
					this.CurTab = this.selectedProject.tab;
					return;
				}
				this.CurTab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x06009A24 RID: 39460 RVA: 0x002D5540 File Offset: 0x002D3740
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			this.windowRect.width = (float)UI.screenWidth;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			float width = Mathf.Max(200f, inRect.width * 0.22f);
			Rect leftOutRect = new Rect(0f, 0f, width, inRect.height);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect);
		}

		// Token: 0x06009A25 RID: 39461 RVA: 0x002D55E8 File Offset: 0x002D37E8
		private void DrawLeftRect(Rect leftOutRect)
		{
			Rect position = leftOutRect;
			GUI.BeginGroup(position);
			if (this.selectedProject != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, 520f);
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.leftScrollViewHeight);
				Widgets.BeginScrollView(outRect, ref this.leftScrollPosition, viewRect, true);
				float num = 0f;
				Text.Font = GameFont.Medium;
				GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
				Rect rect = new Rect(0f, num, viewRect.width - 0f, 50f);
				Widgets.LabelCacheHeight(ref rect, this.selectedProject.LabelCap, true, false);
				GenUI.ResetLabelAlign();
				Text.Font = GameFont.Small;
				num += rect.height;
				Rect rect2 = new Rect(0f, num, viewRect.width, 0f);
				Widgets.LabelCacheHeight(ref rect2, this.selectedProject.description, true, false);
				num += rect2.height;
				Rect rect3 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawTechprintInfo(rect3, this.selectedProject);
				if (this.selectedProject.techLevel > Faction.OfPlayer.def.techLevel)
				{
					float num2 = this.selectedProject.CostFactor(Faction.OfPlayer.def.techLevel);
					Rect rect4 = new Rect(0f, num, viewRect.width, 0f);
					string text = "TechLevelTooLow".Translate(Faction.OfPlayer.def.techLevel.ToStringHuman(), this.selectedProject.techLevel.ToStringHuman(), (1f / num2).ToStringPercent());
					if (num2 != 1f)
					{
						text += " " + "ResearchCostComparison".Translate(this.selectedProject.baseCost.ToString("F0"), this.selectedProject.CostApparent.ToString("F0"));
					}
					Widgets.LabelCacheHeight(ref rect4, text, true, false);
					num += rect4.height;
				}
				Rect rect5 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchPrereqs(this.selectedProject, rect5);
				Rect rect6 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawResearchBenchRequirements(this.selectedProject, rect6);
				Rect rect7 = new Rect(0f, num, viewRect.width, 500f);
				num += this.DrawUnlockableHyperlinks(rect7, this.selectedProject);
				num += 3f;
				this.leftScrollViewHeight = num;
				Widgets.EndScrollView();
				Rect rect8 = new Rect(0f, outRect.yMax + 10f, position.width, 68f);
				if (this.selectedProject.CanStartNow && this.selectedProject != Find.ResearchManager.currentProj)
				{
					if (Widgets.ButtonText(rect8, "Research".Translate(), true, true, true))
					{
						SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
						Find.ResearchManager.currentProj = this.selectedProject;
						TutorSystem.Notify_Event("StartResearchProject");
						if (!this.ColonistsHaveResearchBench)
						{
							Messages.Message("MessageResearchMenuWithoutBench".Translate(), MessageTypeDefOf.CautionInput, true);
						}
					}
				}
				else
				{
					string text2;
					if (this.selectedProject.IsFinished)
					{
						text2 = "Finished".Translate();
						Text.Anchor = TextAnchor.MiddleCenter;
					}
					else if (this.selectedProject == Find.ResearchManager.currentProj)
					{
						text2 = "InProgress".Translate();
						Text.Anchor = TextAnchor.MiddleCenter;
					}
					else
					{
						text2 = "Locked".Translate() + ":";
						if (!this.selectedProject.PrerequisitesCompleted)
						{
							text2 += "\n  " + "PrerequisitesNotCompleted".Translate();
						}
						if (!this.selectedProject.TechprintRequirementMet)
						{
							text2 += "\n  " + "InsufficientTechprintsApplied".Translate(this.selectedProject.TechprintsApplied, this.selectedProject.TechprintCount);
						}
					}
					Widgets.DrawHighlight(rect8);
					Widgets.Label(rect8.ContractedBy(5f), text2);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				Rect rect9 = new Rect(0f, rect8.yMax + 10f, position.width, 35f);
				Widgets.FillableBar(rect9, this.selectedProject.ProgressPercent, MainTabWindow_Research.ResearchBarFillTex, MainTabWindow_Research.ResearchBarBGTex, true);
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect9, this.selectedProject.ProgressApparent.ToString("F0") + " / " + this.selectedProject.CostApparent.ToString("F0"));
				Text.Anchor = TextAnchor.UpperLeft;
				if (Prefs.DevMode && this.selectedProject != Find.ResearchManager.currentProj && !this.selectedProject.IsFinished && Widgets.ButtonText(new Rect(rect8.x, rect8.y - 30f, 120f, 30f), "Debug: Finish now", true, true, true))
				{
					Find.ResearchManager.currentProj = this.selectedProject;
					Find.ResearchManager.FinishProject(this.selectedProject, false, null);
				}
				if (Prefs.DevMode && !this.selectedProject.TechprintRequirementMet && Widgets.ButtonText(new Rect(rect8.x + 120f, rect8.y - 30f, 120f, 30f), "Debug: Apply techprint", true, true, true))
				{
					Find.ResearchManager.ApplyTechprint(this.selectedProject, null);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x06009A26 RID: 39462 RVA: 0x000669F8 File Offset: 0x00064BF8
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x06009A27 RID: 39463 RVA: 0x00066A01 File Offset: 0x00064C01
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x06009A28 RID: 39464 RVA: 0x00066A0A File Offset: 0x00064C0A
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x06009A29 RID: 39465 RVA: 0x00066A13 File Offset: 0x00064C13
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x06009A2A RID: 39466 RVA: 0x00066A1C File Offset: 0x00064C1C
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x06009A2B RID: 39467 RVA: 0x00066A2A File Offset: 0x00064C2A
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x06009A2C RID: 39468 RVA: 0x002D5C20 File Offset: 0x002D3E20
		public override void PostOpen()
		{
			base.PostOpen();
			this.tabs.Clear();
			foreach (ResearchTabDef localTabDef2 in DefDatabase<ResearchTabDef>.AllDefs)
			{
				ResearchTabDef localTabDef = localTabDef2;
				this.tabs.Add(new TabRecord(localTabDef.LabelCap, delegate()
				{
					this.CurTab = localTabDef;
				}, () => this.CurTab == localTabDef));
			}
		}

		// Token: 0x06009A2D RID: 39469 RVA: 0x002D5CC4 File Offset: 0x002D3EC4
		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Widgets.DrawMenuSection(rightOutRect);
			TabDrawer.DrawTabs(rightOutRect, this.tabs, 200f);
			if (Prefs.DevMode)
			{
				Rect rect = rightOutRect;
				rect.yMax = rect.yMin + 20f;
				rect.xMin = rect.xMax - 80f;
				Rect butRect = rect.RightPartPixels(30f);
				rect = rect.LeftPartPixels(rect.width - 30f);
				Widgets.CheckboxLabeled(rect, "Edit", ref this.editMode, false, null, null, false);
				if (Widgets.ButtonImageFitted(butRect, TexButton.Copy))
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (ResearchProjectDef researchProjectDef in from def in this.VisibleResearchProjects
					where def.Debug_IsPositionModified()
					select def)
					{
						stringBuilder.AppendLine(researchProjectDef.defName);
						stringBuilder.AppendLine(string.Format("  <researchViewX>{0}</researchViewX>", researchProjectDef.ResearchViewX.ToString("F2")));
						stringBuilder.AppendLine(string.Format("  <researchViewY>{0}</researchViewY>", researchProjectDef.ResearchViewY.ToString("F2")));
						stringBuilder.AppendLine();
					}
					GUIUtility.systemCopyBuffer = stringBuilder.ToString();
					Messages.Message("Modified data copied to clipboard.", MessageTypeDefOf.SituationResolved, false);
				}
			}
			else
			{
				this.editMode = false;
			}
			bool flag = false;
			Rect outRect = rightOutRect.ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, this.rightViewWidth, this.rightViewHeight);
			rect2.ContractedBy(10f);
			rect2.width = this.rightViewWidth;
			Rect position = rect2.ContractedBy(10f);
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			Widgets.ScrollHorizontal(outRect, ref this.rightScrollPosition, rect2, 20f);
			Widgets.BeginScrollView(outRect, ref this.rightScrollPosition, rect2, true);
			GUI.BeginGroup(position);
			List<ResearchProjectDef> visibleResearchProjects = this.VisibleResearchProjects;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < visibleResearchProjects.Count; j++)
				{
					ResearchProjectDef researchProjectDef2 = visibleResearchProjects[j];
					if (researchProjectDef2.tab == this.CurTab)
					{
						start.x = this.PosX(researchProjectDef2);
						start.y = this.PosY(researchProjectDef2) + 25f;
						for (int k = 0; k < researchProjectDef2.prerequisites.CountAllowNull<ResearchProjectDef>(); k++)
						{
							ResearchProjectDef researchProjectDef3 = researchProjectDef2.prerequisites[k];
							if (researchProjectDef3 != null && researchProjectDef3.tab == this.CurTab)
							{
								end.x = this.PosX(researchProjectDef3) + 140f;
								end.y = this.PosY(researchProjectDef3) + 25f;
								if (this.selectedProject == researchProjectDef2 || this.selectedProject == researchProjectDef3)
								{
									if (i == 1)
									{
										Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
									}
								}
								else if (i == 0)
								{
									Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
								}
							}
						}
					}
				}
			}
			Rect other = new Rect(this.rightScrollPosition.x, this.rightScrollPosition.y, outRect.width, outRect.height).ExpandedBy(10f);
			for (int l = 0; l < visibleResearchProjects.Count; l++)
			{
				ResearchProjectDef researchProjectDef4 = visibleResearchProjects[l];
				if (researchProjectDef4.tab == this.CurTab)
				{
					Rect source = new Rect(this.PosX(researchProjectDef4), this.PosY(researchProjectDef4), 140f, 50f);
					Rect rect3 = new Rect(source);
					string label = this.GetLabel(researchProjectDef4);
					Widgets.LabelCacheHeight(ref rect3, this.GetLabelWithNewlineCached(label), true, false);
					if (rect3.Overlaps(other))
					{
						Color color = Widgets.NormalOptionColor;
						Color color2 = default(Color);
						Color borderColor = default(Color);
						bool flag2 = !researchProjectDef4.IsFinished && !researchProjectDef4.CanStartNow;
						if (researchProjectDef4 == Find.ResearchManager.currentProj)
						{
							color2 = TexUI.ActiveResearchColor;
						}
						else if (researchProjectDef4.IsFinished)
						{
							color2 = TexUI.FinishedResearchColor;
						}
						else if (flag2)
						{
							color2 = TexUI.LockedResearchColor;
						}
						else if (researchProjectDef4.CanStartNow)
						{
							color2 = TexUI.AvailResearchColor;
						}
						if (this.editMode && this.draggingTabs.Contains(researchProjectDef4))
						{
							borderColor = Color.yellow;
						}
						else if (this.selectedProject == researchProjectDef4)
						{
							color2 += TexUI.HighlightBgResearchColor;
							borderColor = TexUI.HighlightBorderResearchColor;
						}
						else
						{
							borderColor = TexUI.DefaultBorderResearchColor;
						}
						if (flag2)
						{
							color = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
						}
						if (this.selectedProject != null)
						{
							if ((researchProjectDef4.prerequisites != null && researchProjectDef4.prerequisites.Contains(this.selectedProject)) || (researchProjectDef4.hiddenPrerequisites != null && researchProjectDef4.hiddenPrerequisites.Contains(this.selectedProject)))
							{
								borderColor = TexUI.HighlightLineResearchColor;
							}
							if (!researchProjectDef4.IsFinished && ((this.selectedProject.prerequisites != null && this.selectedProject.prerequisites.Contains(researchProjectDef4)) || (this.selectedProject.hiddenPrerequisites != null && this.selectedProject.hiddenPrerequisites.Contains(researchProjectDef4))))
							{
								borderColor = TexUI.DependencyOutlineResearchColor;
							}
						}
						if (this.requiredByThisFound)
						{
							for (int m = 0; m < researchProjectDef4.requiredByThis.CountAllowNull<ResearchProjectDef>(); m++)
							{
								ResearchProjectDef researchProjectDef5 = researchProjectDef4.requiredByThis[m];
								if (this.selectedProject == researchProjectDef5)
								{
									borderColor = TexUI.HighlightLineResearchColor;
								}
							}
						}
						Rect rect4 = rect3;
						Widgets.LabelCacheHeight(ref rect4, " ", true, false);
						if (Widgets.CustomButtonText(ref rect3, "", color2, color, borderColor, false, 1, true, true))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							this.selectedProject = researchProjectDef4;
						}
						rect4.y = rect3.y + rect3.height - rect4.height;
						Rect rect5 = rect4;
						rect5.x += 10f;
						rect5.width = rect5.width / 2f - 10f;
						Rect rect6 = rect5;
						rect6.x += rect5.width;
						TextAnchor anchor = Text.Anchor;
						Color color3 = GUI.color;
						GUI.color = color;
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.Label(rect3, label);
						GUI.color = color;
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect5, researchProjectDef4.CostApparent.ToString());
						if (researchProjectDef4.TechprintCount > 0)
						{
							GUI.color = (researchProjectDef4.TechprintRequirementMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor);
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect6, this.GetTechprintsInfoCached(researchProjectDef4.TechprintsApplied, researchProjectDef4.TechprintCount));
						}
						GUI.color = color3;
						Text.Anchor = anchor;
						if (this.editMode && Mouse.IsOver(rect3) && Input.GetMouseButtonDown(0))
						{
							flag = true;
							if (Input.GetKey(KeyCode.LeftShift))
							{
								if (!this.draggingTabs.Contains(researchProjectDef4))
								{
									this.draggingTabs.Add(researchProjectDef4);
								}
							}
							else if (!Input.GetKey(KeyCode.LeftControl) && !this.draggingTabs.Contains(researchProjectDef4))
							{
								this.draggingTabs.Clear();
								this.draggingTabs.Add(researchProjectDef4);
							}
							if (Input.GetKey(KeyCode.LeftControl) && this.draggingTabs.Contains(researchProjectDef4))
							{
								this.draggingTabs.Remove(researchProjectDef4);
							}
						}
					}
				}
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
			if (this.editMode)
			{
				if (!flag && Input.GetMouseButtonDown(0))
				{
					this.draggingTabs.Clear();
				}
				if (!this.draggingTabs.NullOrEmpty<ResearchProjectDef>())
				{
					if (Input.GetMouseButtonUp(0))
					{
						for (int n = 0; n < this.draggingTabs.Count; n++)
						{
							this.draggingTabs[n].Debug_SnapPositionData();
						}
						return;
					}
					if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && Event.current.type == EventType.Layout)
					{
						for (int num = 0; num < this.draggingTabs.Count; num++)
						{
							this.draggingTabs[num].Debug_ApplyPositionDelta(new Vector2(this.PixelsToCoordX(Event.current.delta.x), this.PixelsToCoordY(Event.current.delta.y)));
						}
					}
				}
			}
		}

		// Token: 0x06009A2E RID: 39470 RVA: 0x002D6554 File Offset: 0x002D4754
		private float DrawResearchPrereqs(ResearchProjectDef project, Rect rect)
		{
			if (project.prerequisites.NullOrEmpty<ResearchProjectDef>())
			{
				return 0f;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			Widgets.LabelCacheHeight(ref rect, "ResearchPrerequisites".Translate() + ":", true, false);
			rect.yMin += rect.height;
			rect.xMin += 6f;
			for (int i = 0; i < project.prerequisites.Count; i++)
			{
				this.SetPrerequisiteStatusColor(project.prerequisites[i].IsFinished, project);
				Widgets.LabelCacheHeight(ref rect, project.prerequisites[i].LabelCap, true, false);
				rect.yMin += rect.height;
			}
			if (project.hiddenPrerequisites != null)
			{
				for (int j = 0; j < project.hiddenPrerequisites.Count; j++)
				{
					this.SetPrerequisiteStatusColor(project.hiddenPrerequisites[j].IsFinished, project);
					Widgets.LabelCacheHeight(ref rect, project.hiddenPrerequisites[j].LabelCap, true, false);
					rect.yMin += rect.height;
				}
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x06009A2F RID: 39471 RVA: 0x00066A38 File Offset: 0x00064C38
		private string GetLabelWithNewlineCached(string label)
		{
			if (!MainTabWindow_Research.labelsWithNewlineCached.ContainsKey(label))
			{
				MainTabWindow_Research.labelsWithNewlineCached.Add(label, label + "\n");
			}
			return MainTabWindow_Research.labelsWithNewlineCached[label];
		}

		// Token: 0x06009A30 RID: 39472 RVA: 0x002D66B8 File Offset: 0x002D48B8
		private string GetTechprintsInfoCached(int applied, int total)
		{
			Pair<int, int> key = new Pair<int, int>(applied, total);
			if (!MainTabWindow_Research.techprintsInfoCached.ContainsKey(key))
			{
				MainTabWindow_Research.techprintsInfoCached.Add(key, string.Format("{0} / {1}", applied.ToString(), total.ToString()));
			}
			return MainTabWindow_Research.techprintsInfoCached[key];
		}

		// Token: 0x06009A31 RID: 39473 RVA: 0x002D670C File Offset: 0x002D490C
		private float DrawResearchBenchRequirements(ResearchProjectDef project, Rect rect)
		{
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			if (project.requiredResearchBuilding != null)
			{
				bool present = false;
				List<Map> maps = Find.Maps;
				Predicate<Building> <>9__0;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					Predicate<Building> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Building x) => x.def == project.requiredResearchBuilding));
					}
					if (allBuildingsColonist.Find(match) != null)
					{
						present = true;
						break;
					}
				}
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBench".Translate() + ":", true, false);
				rect.xMin += 6f;
				rect.yMin += rect.height;
				this.SetPrerequisiteStatusColor(present, project);
				rect.height = Text.CalcHeight(project.requiredResearchBuilding.LabelCap, rect.width - 24f - 6f);
				Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(project.requiredResearchBuilding, -1), null, 2f, 6f);
				rect.yMin += rect.height + 4f;
				GUI.color = Color.white;
				rect.xMin = xMin;
			}
			if (!project.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				Widgets.LabelCacheHeight(ref rect, "RequiredResearchBenchFacilities".Translate() + ":", true, false);
				rect.yMin += rect.height;
				Building_ResearchBench building_ResearchBench = this.FindBenchFulfillingMostRequirements(project.requiredResearchBuilding, project.requiredResearchFacilities);
				CompAffectedByFacilities bestMatchingBench = null;
				if (building_ResearchBench != null)
				{
					bestMatchingBench = building_ResearchBench.TryGetComp<CompAffectedByFacilities>();
				}
				rect.xMin += 6f;
				for (int j = 0; j < project.requiredResearchFacilities.Count; j++)
				{
					this.DrawResearchBenchFacilityRequirement(project.requiredResearchFacilities[j], bestMatchingBench, project, ref rect);
					rect.yMin += rect.height;
				}
				rect.yMin += 4f;
			}
			GUI.color = Color.white;
			rect.xMin = xMin;
			return rect.yMin - yMin;
		}

		// Token: 0x06009A32 RID: 39474 RVA: 0x002D6990 File Offset: 0x002D4B90
		private float DrawUnlockableHyperlinks(Rect rect, ResearchProjectDef project)
		{
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list = this.UnlockedDefsGroupedByPrerequisites(project);
			if (list.NullOrEmpty<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>())
			{
				return 0f;
			}
			float yMin = rect.yMin;
			float x = rect.x;
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in list)
			{
				ResearchPrerequisitesUtility.UnlockedHeader first = pair.First;
				rect.x = x;
				if (!first.unlockedBy.Any<ResearchProjectDef>())
				{
					Widgets.LabelCacheHeight(ref rect, "Unlocks".Translate() + ":", true, false);
				}
				else
				{
					Widgets.LabelCacheHeight(ref rect, "UnlockedWith".Translate() + " " + this.HeaderLabel(first) + ":", true, false);
				}
				rect.x += 6f;
				rect.yMin += rect.height;
				foreach (Def def in pair.Second)
				{
					Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(def, -1);
					Widgets.HyperlinkWithIcon(new Rect(rect.x, rect.yMin, rect.width, 24f), hyperlink, null, 2f, 6f);
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x06009A33 RID: 39475 RVA: 0x002D6B4C File Offset: 0x002D4D4C
		private List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef project)
		{
			if (this.cachedUnlockedDefsGroupedByPrerequisites == null)
			{
				this.cachedUnlockedDefsGroupedByPrerequisites = new Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>>();
			}
			List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> list;
			if (!this.cachedUnlockedDefsGroupedByPrerequisites.TryGetValue(project, out list))
			{
				list = ResearchPrerequisitesUtility.UnlockedDefsGroupedByPrerequisites(project);
				this.cachedUnlockedDefsGroupedByPrerequisites.Add(project, list);
			}
			return list;
		}

		// Token: 0x06009A34 RID: 39476 RVA: 0x002D6B94 File Offset: 0x002D4D94
		private string HeaderLabel(ResearchPrerequisitesUtility.UnlockedHeader headerProject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			for (int i = 0; i < headerProject.unlockedBy.Count; i++)
			{
				ResearchProjectDef researchProjectDef = headerProject.unlockedBy[i];
				string text = researchProjectDef.LabelCap;
				if (!researchProjectDef.IsFinished)
				{
					text = text.Colorize(MainTabWindow_Research.MissingPrerequisiteColor);
				}
				stringBuilder.Append(text).Append(value);
				value = ", ";
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009A35 RID: 39477 RVA: 0x002D6C08 File Offset: 0x002D4E08
		private float DrawTechprintInfo(Rect rect, ResearchProjectDef project)
		{
			if (this.selectedProject.TechprintCount == 0)
			{
				return 0f;
			}
			float xMin = rect.xMin;
			float yMin = rect.yMin;
			string text = "ResearchTechprintsFromFactions".Translate();
			float num = Text.CalcHeight(text, rect.width);
			Widgets.Label(new Rect(rect.x, yMin, rect.width, num), text);
			rect.x += 6f;
			if (this.selectedProject.heldByFactionCategoryTags != null)
			{
				foreach (string b in this.selectedProject.heldByFactionCategoryTags)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
					{
						if (faction.def.categoryTag == b)
						{
							string name = faction.Name;
							Rect position = new Rect(rect.x, yMin + num, rect.width, Mathf.Max(24f, Text.CalcHeight(name, rect.width - 24f - 6f)));
							GUI.BeginGroup(position);
							Rect r = new Rect(0f, 0f, 24f, 24f).ContractedBy(2f);
							FactionUIUtility.DrawFactionIconWithTooltip(r, faction);
							Rect rect2 = new Rect(r.xMax + 6f, 0f, position.width - r.width - 6f, position.height);
							Text.Anchor = TextAnchor.MiddleLeft;
							Text.WordWrap = false;
							Widgets.Label(rect2, faction.Name);
							Text.Anchor = TextAnchor.UpperLeft;
							Text.WordWrap = true;
							GUI.EndGroup();
							num += position.height;
						}
					}
				}
			}
			rect.xMin = xMin;
			return num;
		}

		// Token: 0x06009A36 RID: 39478 RVA: 0x0001F65F File Offset: 0x0001D85F
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap;
		}

		// Token: 0x06009A37 RID: 39479 RVA: 0x00066A68 File Offset: 0x00064C68
		private void SetPrerequisiteStatusColor(bool present, ResearchProjectDef project)
		{
			if (project.IsFinished)
			{
				return;
			}
			if (present)
			{
				GUI.color = MainTabWindow_Research.FulfilledPrerequisiteColor;
				return;
			}
			GUI.color = MainTabWindow_Research.MissingPrerequisiteColor;
		}

		// Token: 0x06009A38 RID: 39480 RVA: 0x002D6E40 File Offset: 0x002D5040
		private void DrawResearchBenchFacilityRequirement(ThingDef requiredFacility, CompAffectedByFacilities bestMatchingBench, ResearchProjectDef project, ref Rect rect)
		{
			Thing thing = null;
			Thing thing2 = null;
			if (bestMatchingBench != null)
			{
				thing = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility);
				thing2 = bestMatchingBench.LinkedFacilitiesListForReading.Find((Thing x) => x.def == requiredFacility && bestMatchingBench.IsFacilityActive(x));
			}
			this.SetPrerequisiteStatusColor(thing2 != null, project);
			string text = requiredFacility.LabelCap;
			if (thing != null && thing2 == null)
			{
				text += " (" + "InactiveFacility".Translate() + ")";
			}
			rect.height = Text.CalcHeight(text, rect.width - 24f - 6f);
			Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(requiredFacility, -1), text, 2f, 6f);
		}

		// Token: 0x06009A39 RID: 39481 RVA: 0x002D6F38 File Offset: 0x002D5138
		private Building_ResearchBench FindBenchFulfillingMostRequirements(ThingDef requiredResearchBench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.tmpAllBuildings.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				MainTabWindow_Research.tmpAllBuildings.AddRange(maps[i].listerBuildings.allBuildingsColonist);
			}
			float num = 0f;
			Building_ResearchBench building_ResearchBench = null;
			for (int j = 0; j < MainTabWindow_Research.tmpAllBuildings.Count; j++)
			{
				Building_ResearchBench building_ResearchBench2 = MainTabWindow_Research.tmpAllBuildings[j] as Building_ResearchBench;
				if (building_ResearchBench2 != null && (requiredResearchBench == null || building_ResearchBench2.def == requiredResearchBench))
				{
					float researchBenchRequirementsScore = this.GetResearchBenchRequirementsScore(building_ResearchBench2, requiredFacilities);
					if (building_ResearchBench == null || researchBenchRequirementsScore > num)
					{
						num = researchBenchRequirementsScore;
						building_ResearchBench = building_ResearchBench2;
					}
				}
			}
			MainTabWindow_Research.tmpAllBuildings.Clear();
			return building_ResearchBench;
		}

		// Token: 0x06009A3A RID: 39482 RVA: 0x002D6FEC File Offset: 0x002D51EC
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.<>c__DisplayClass69_0 CS$<>8__locals1 = new MainTabWindow_Research.<>c__DisplayClass69_0();
			CS$<>8__locals1.requiredFacilities = requiredFacilities;
			float num = 0f;
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.requiredFacilities.Count; i = j + 1)
			{
				CompAffectedByFacilities benchComp = bench.GetComp<CompAffectedByFacilities>();
				if (benchComp != null)
				{
					List<Thing> linkedFacilitiesListForReading = benchComp.LinkedFacilitiesListForReading;
					if (linkedFacilitiesListForReading.Find((Thing x) => x.def == CS$<>8__locals1.requiredFacilities[i] && benchComp.IsFacilityActive(x)) != null)
					{
						num += 1f;
					}
					else if (linkedFacilitiesListForReading.Find((Thing x) => x.def == CS$<>8__locals1.requiredFacilities[i]) != null)
					{
						num += 0.6f;
					}
				}
				j = i;
			}
			return num;
		}

		// Token: 0x0400627F RID: 25215
		protected ResearchProjectDef selectedProject;

		// Token: 0x04006280 RID: 25216
		private bool requiredByThisFound;

		// Token: 0x04006281 RID: 25217
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x04006282 RID: 25218
		private float leftScrollViewHeight;

		// Token: 0x04006283 RID: 25219
		private Vector2 rightScrollPosition;

		// Token: 0x04006284 RID: 25220
		private float rightViewWidth;

		// Token: 0x04006285 RID: 25221
		private float rightViewHeight;

		// Token: 0x04006286 RID: 25222
		private ResearchTabDef curTabInt;

		// Token: 0x04006287 RID: 25223
		private bool editMode;

		// Token: 0x04006288 RID: 25224
		private List<ResearchProjectDef> draggingTabs = new List<ResearchProjectDef>();

		// Token: 0x04006289 RID: 25225
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400628A RID: 25226
		private List<ResearchProjectDef> cachedVisibleResearchProjects;

		// Token: 0x0400628B RID: 25227
		private Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>> cachedUnlockedDefsGroupedByPrerequisites;

		// Token: 0x0400628C RID: 25228
		private const float leftAreaWidthPercent = 0.22f;

		// Token: 0x0400628D RID: 25229
		private const float LeftAreaWidthMin = 200f;

		// Token: 0x0400628E RID: 25230
		private const int ModeSelectButHeight = 40;

		// Token: 0x0400628F RID: 25231
		private const float ProjectTitleHeight = 50f;

		// Token: 0x04006290 RID: 25232
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x04006291 RID: 25233
		private const int ResearchItemW = 140;

		// Token: 0x04006292 RID: 25234
		private const int ResearchItemH = 50;

		// Token: 0x04006293 RID: 25235
		private const int ResearchItemPaddingW = 50;

		// Token: 0x04006294 RID: 25236
		private const int ResearchItemPaddingH = 50;

		// Token: 0x04006295 RID: 25237
		private const int ColumnMaxProjects = 6;

		// Token: 0x04006296 RID: 25238
		private const float LineOffsetFactor = 0.48f;

		// Token: 0x04006297 RID: 25239
		private const float IndentSpacing = 6f;

		// Token: 0x04006298 RID: 25240
		private const float RowHeight = 24f;

		// Token: 0x04006299 RID: 25241
		private const KeyCode SelectMultipleKey = KeyCode.LeftShift;

		// Token: 0x0400629A RID: 25242
		private const KeyCode DeselectKey = KeyCode.LeftControl;

		// Token: 0x0400629B RID: 25243
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x0400629C RID: 25244
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x0400629D RID: 25245
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x0400629E RID: 25246
		private static readonly Color MissingPrerequisiteColor = ColoredText.RedReadable;

		// Token: 0x0400629F RID: 25247
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x040062A0 RID: 25248
		private static Dictionary<string, string> labelsWithNewlineCached = new Dictionary<string, string>();

		// Token: 0x040062A1 RID: 25249
		private static Dictionary<Pair<int, int>, string> techprintsInfoCached = new Dictionary<Pair<int, int>, string>();

		// Token: 0x040062A2 RID: 25250
		private static List<Building> tmpAllBuildings = new List<Building>();
	}
}
