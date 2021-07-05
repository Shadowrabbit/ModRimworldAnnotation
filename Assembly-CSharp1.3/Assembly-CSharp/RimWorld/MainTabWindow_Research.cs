using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200136F RID: 4975
	[StaticConstructorOnStartup]
	public class MainTabWindow_Research : MainTabWindow
	{
		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x060078EE RID: 30958 RVA: 0x002AC047 File Offset: 0x002AA247
		// (set) Token: 0x060078EF RID: 30959 RVA: 0x002AC050 File Offset: 0x002AA250
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

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x060078F0 RID: 30960 RVA: 0x002AC0A0 File Offset: 0x002AA2A0
		private MainTabWindow_Research.ResearchTabRecord CurTabRecord
		{
			get
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
				{
					if (researchTabRecord.def == this.CurTab)
					{
						return researchTabRecord;
					}
				}
				return null;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x060078F1 RID: 30961 RVA: 0x002AC104 File Offset: 0x002AA304
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

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x060078F2 RID: 30962 RVA: 0x002AC144 File Offset: 0x002AA344
		public List<ResearchProjectDef> VisibleResearchProjects
		{
			get
			{
				if (this.cachedVisibleResearchProjects == null)
				{
					this.cachedVisibleResearchProjects = new List<ResearchProjectDef>(from d in DefDatabase<ResearchProjectDef>.AllDefsListForReading
					where Find.Storyteller.difficulty.AllowedBy(d.hideWhen) || d == Find.ResearchManager.currentProj
					select d);
				}
				return this.cachedVisibleResearchProjects;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x060078F3 RID: 30963 RVA: 0x002AC194 File Offset: 0x002AA394
		public override Vector2 InitialSize
		{
			get
			{
				Vector2 initialSize = base.InitialSize;
				float b = (float)(UI.screenHeight - 35);
				float num = 0f;
				foreach (ResearchTabDef tab in DefDatabase<ResearchTabDef>.AllDefs)
				{
					num = Mathf.Max(num, this.ViewSize(tab).y);
				}
				float b2 = this.Margin + 10f + 32f + 10f + num + 10f + 10f + this.Margin;
				float a = Mathf.Max(initialSize.y, b2);
				initialSize.y = Mathf.Min(a, b);
				return initialSize;
			}
		}

		// Token: 0x060078F4 RID: 30964 RVA: 0x002AC258 File Offset: 0x002AA458
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
					Widgets.LabelCacheHeight(ref rect, this.GetLabelWithNewlineCached(this.GetLabel(researchProjectDef)), false, false);
					num = Mathf.Max(num, this.PosX(researchProjectDef) + 140f);
					num2 = Mathf.Max(num2, this.PosY(researchProjectDef) + rect.height);
				}
			}
			return new Vector2(num + 20f + 12f, num2 + 20f + 12f);
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x002AC320 File Offset: 0x002AA520
		public override void PreOpen()
		{
			base.PreOpen();
			this.selectedProject = Find.ResearchManager.currentProj;
			this.scrollPositioner.Arm(true);
			this.cachedVisibleResearchProjects = null;
			this.cachedUnlockedDefsGroupedByPrerequisites = null;
			this.quickSearchWidget.Reset();
			if (this.CurTab == null)
			{
				if (this.selectedProject != null)
				{
					this.CurTab = this.selectedProject.tab;
				}
				else
				{
					this.CurTab = ResearchTabDefOf.Main;
				}
			}
			this.UpdateSearchResults();
		}

		// Token: 0x060078F6 RID: 30966 RVA: 0x002AC39C File Offset: 0x002AA59C
		public override void DoWindowContents(Rect inRect)
		{
			this.windowRect.width = (float)UI.screenWidth;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			float width = Mathf.Max(200f, inRect.width * 0.22f);
			Rect leftOutRect = new Rect(0f, 0f, width, inRect.height - 24f - 10f);
			Rect searchRect = new Rect(0f, leftOutRect.yMax + 10f, width, 24f);
			Rect rightOutRect = new Rect(leftOutRect.xMax + 10f, 0f, inRect.width - leftOutRect.width - 10f, inRect.height);
			this.DrawSearchRect(searchRect);
			this.DrawLeftRect(leftOutRect);
			this.DrawRightRect(rightOutRect);
		}

		// Token: 0x060078F7 RID: 30967 RVA: 0x002AC46C File Offset: 0x002AA66C
		private void DrawSearchRect(Rect searchRect)
		{
			this.quickSearchWidget.OnGUI(searchRect, new Action(this.UpdateSearchResults));
		}

		// Token: 0x060078F8 RID: 30968 RVA: 0x002AC488 File Offset: 0x002AA688
		private void DrawLeftRect(Rect leftOutRect)
		{
			float height = leftOutRect.height - 78f - 45f;
			Rect position = leftOutRect;
			GUI.BeginGroup(position);
			if (this.selectedProject != null)
			{
				Rect outRect = new Rect(0f, 0f, position.width, height);
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
						this.AttemptBeginResearch(this.selectedProject);
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

		// Token: 0x060078F9 RID: 30969 RVA: 0x002ACA90 File Offset: 0x002AAC90
		private void AttemptBeginResearch(ResearchProjectDef projectToStart)
		{
			List<ValueTuple<BuildableDef, List<string>>> list = this.ComputeUnlockedDefsThatHaveMissingMemes(projectToStart);
			if (!list.Any<ValueTuple<BuildableDef, List<string>>>())
			{
				this.DoBeginResearch(projectToStart);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ResearchProjectHasDefsWithMissingMemes".Translate(projectToStart.LabelCap)).Append(":");
			stringBuilder.AppendLine();
			foreach (ValueTuple<BuildableDef, List<string>> valueTuple in list)
			{
				BuildableDef item = valueTuple.Item1;
				List<string> item2 = valueTuple.Item2;
				stringBuilder.AppendLine();
				stringBuilder.Append("  - ").Append(item.LabelCap.Colorize(ColoredText.NameColor)).Append(" (").Append(item2.ToCommaList(false, false)).Append(")");
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate
			{
				this.DoBeginResearch(projectToStart);
			}, false, null));
			SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
		}

		// Token: 0x060078FA RID: 30970 RVA: 0x002ACBD8 File Offset: 0x002AADD8
		private List<ValueTuple<BuildableDef, List<string>>> ComputeUnlockedDefsThatHaveMissingMemes(ResearchProjectDef project)
		{
			this.cachedDefsWithMissingMemes.Clear();
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
			if (ideo == null)
			{
				return this.cachedDefsWithMissingMemes;
			}
			foreach (Def def in project.UnlockedDefs)
			{
				BuildableDef buildableDef = def as BuildableDef;
				if (buildableDef != null && !buildableDef.canGenerateDefaultDesignator)
				{
					List<string> list = null;
					foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefsListForReading)
					{
						if (!ideo.HasMeme(memeDef) && memeDef.AllDesignatorBuildables.Contains(buildableDef))
						{
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(memeDef.LabelCap);
						}
					}
					if (list != null)
					{
						this.cachedDefsWithMissingMemes.Add(new ValueTuple<BuildableDef, List<string>>(buildableDef, list));
					}
				}
			}
			return this.cachedDefsWithMissingMemes;
		}

		// Token: 0x060078FB RID: 30971 RVA: 0x002ACCF8 File Offset: 0x002AAEF8
		private void DoBeginResearch(ResearchProjectDef projectToStart)
		{
			SoundDefOf.ResearchStart.PlayOneShotOnCamera(null);
			Find.ResearchManager.currentProj = projectToStart;
			TutorSystem.Notify_Event("StartResearchProject");
			if (!this.ColonistsHaveResearchBench)
			{
				Messages.Message("MessageResearchMenuWithoutBench".Translate(), MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x060078FC RID: 30972 RVA: 0x002ACD4C File Offset: 0x002AAF4C
		private float CoordToPixelsX(float x)
		{
			return x * 190f;
		}

		// Token: 0x060078FD RID: 30973 RVA: 0x002ACD55 File Offset: 0x002AAF55
		private float CoordToPixelsY(float y)
		{
			return y * 100f;
		}

		// Token: 0x060078FE RID: 30974 RVA: 0x002ACD5E File Offset: 0x002AAF5E
		private float PixelsToCoordX(float x)
		{
			return x / 190f;
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x002ACD67 File Offset: 0x002AAF67
		private float PixelsToCoordY(float y)
		{
			return y / 100f;
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x002ACD70 File Offset: 0x002AAF70
		private float PosX(ResearchProjectDef d)
		{
			return this.CoordToPixelsX(d.ResearchViewX);
		}

		// Token: 0x06007901 RID: 30977 RVA: 0x002ACD7E File Offset: 0x002AAF7E
		private float PosY(ResearchProjectDef d)
		{
			return this.CoordToPixelsY(d.ResearchViewY);
		}

		// Token: 0x06007902 RID: 30978 RVA: 0x002ACD8C File Offset: 0x002AAF8C
		public override void PostOpen()
		{
			base.PostOpen();
			this.tabs.Clear();
			using (IEnumerator<ResearchTabDef> enumerator = DefDatabase<ResearchTabDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ResearchTabDef tabDef = enumerator.Current;
					this.tabs.Add(new MainTabWindow_Research.ResearchTabRecord(tabDef, tabDef.LabelCap, delegate()
					{
						this.CurTab = tabDef;
					}, () => this.CurTab == tabDef));
				}
			}
		}

		// Token: 0x06007903 RID: 30979 RVA: 0x002ACE34 File Offset: 0x002AB034
		private void DrawRightRect(Rect rightOutRect)
		{
			rightOutRect.yMin += 32f;
			Widgets.DrawMenuSection(rightOutRect);
			TabDrawer.DrawTabs<MainTabWindow_Research.ResearchTabRecord>(rightOutRect, this.tabs, 200f);
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
			this.scrollPositioner.ClearInterestRects();
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
					Rect rect3 = new Rect(this.PosX(researchProjectDef4), this.PosY(researchProjectDef4), 140f, 50f);
					Rect rect4 = new Rect(rect3);
					bool flag2 = this.quickSearchWidget.filter.Active && !this.matchingProjects.Contains(researchProjectDef4);
					bool flag3 = this.quickSearchWidget.filter.Active && this.matchingProjects.Contains(researchProjectDef4);
					if (flag3 || this.selectedProject == researchProjectDef4)
					{
						this.scrollPositioner.RegisterInterestRect(rect3);
					}
					string label = this.GetLabel(researchProjectDef4);
					Widgets.LabelCacheHeight(ref rect4, this.GetLabelWithNewlineCached(label), true, false);
					if (rect4.Overlaps(other))
					{
						Color color = Widgets.NormalOptionColor;
						Color color2 = default(Color);
						Color color3 = default(Color);
						bool flag4 = !researchProjectDef4.IsFinished && !researchProjectDef4.CanStartNow;
						if (researchProjectDef4 == Find.ResearchManager.currentProj)
						{
							color2 = TexUI.ActiveResearchColor;
						}
						else if (researchProjectDef4.IsFinished)
						{
							color2 = TexUI.FinishedResearchColor;
						}
						else if (flag4)
						{
							color2 = TexUI.LockedResearchColor;
						}
						else if (researchProjectDef4.CanStartNow)
						{
							color2 = TexUI.AvailResearchColor;
						}
						if (this.editMode && this.draggingTabs.Contains(researchProjectDef4))
						{
							color3 = Color.yellow;
						}
						else if (this.selectedProject == researchProjectDef4)
						{
							color2 += TexUI.HighlightBgResearchColor;
							color3 = TexUI.HighlightBorderResearchColor;
						}
						else
						{
							color3 = TexUI.DefaultBorderResearchColor;
						}
						if (flag4)
						{
							color = MainTabWindow_Research.ProjectWithMissingPrerequisiteLabelColor;
						}
						if (this.selectedProject != null)
						{
							if ((researchProjectDef4.prerequisites != null && researchProjectDef4.prerequisites.Contains(this.selectedProject)) || (researchProjectDef4.hiddenPrerequisites != null && researchProjectDef4.hiddenPrerequisites.Contains(this.selectedProject)))
							{
								color3 = TexUI.HighlightLineResearchColor;
							}
							if (!researchProjectDef4.IsFinished && ((this.selectedProject.prerequisites != null && this.selectedProject.prerequisites.Contains(researchProjectDef4)) || (this.selectedProject.hiddenPrerequisites != null && this.selectedProject.hiddenPrerequisites.Contains(researchProjectDef4))))
							{
								color3 = TexUI.DependencyOutlineResearchColor;
							}
						}
						if (this.requiredByThisFound)
						{
							for (int m = 0; m < researchProjectDef4.requiredByThis.CountAllowNull<ResearchProjectDef>(); m++)
							{
								ResearchProjectDef researchProjectDef5 = researchProjectDef4.requiredByThis[m];
								if (this.selectedProject == researchProjectDef5)
								{
									color3 = TexUI.HighlightLineResearchColor;
								}
							}
						}
						Color color4 = researchProjectDef4.TechprintRequirementMet ? MainTabWindow_Research.FulfilledPrerequisiteColor : MainTabWindow_Research.MissingPrerequisiteColor;
						if (flag2)
						{
							color = this.NoMatchTint(color);
							color2 = this.NoMatchTint(color2);
							color3 = this.NoMatchTint(color3);
							color4 = this.NoMatchTint(color4);
						}
						Rect rect5 = rect4;
						Widgets.LabelCacheHeight(ref rect5, " ", true, false);
						if (flag3)
						{
							QuickSearchWidget.DrawStrongHighlight(rect4.ExpandedBy(12f));
						}
						if (Widgets.CustomButtonText(ref rect4, "", color2, color, color3, false, 1, true, true))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							this.selectedProject = researchProjectDef4;
						}
						rect5.y = rect4.y + rect4.height - rect5.height;
						Rect rect6 = rect5;
						rect6.x += 10f;
						rect6.width = rect6.width / 2f - 10f;
						Rect rect7 = rect6;
						rect7.x += rect6.width;
						Color color5 = GUI.color;
						TextAnchor anchor = Text.Anchor;
						GUI.color = color;
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.Label(rect4, label);
						GUI.color = color;
						Text.Anchor = TextAnchor.MiddleLeft;
						Widgets.Label(rect6, researchProjectDef4.CostApparent.ToString());
						if (researchProjectDef4.TechprintCount > 0)
						{
							GUI.color = color4;
							Text.Anchor = TextAnchor.MiddleRight;
							Widgets.Label(rect7, this.GetTechprintsInfoCached(researchProjectDef4.TechprintsApplied, researchProjectDef4.TechprintCount));
						}
						GUI.color = color5;
						Text.Anchor = anchor;
						if (this.editMode && Mouse.IsOver(rect4) && Input.GetMouseButtonDown(0))
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
			this.scrollPositioner.ScrollHorizontally(ref this.rightScrollPosition, outRect.size);
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

		// Token: 0x06007904 RID: 30980 RVA: 0x002AD794 File Offset: 0x002AB994
		private Color NoMatchTint(Color color)
		{
			return Color.Lerp(color, MainTabWindow_Research.NoMatchTintColor, 0.4f);
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x002AD7A8 File Offset: 0x002AB9A8
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

		// Token: 0x06007906 RID: 30982 RVA: 0x002AD909 File Offset: 0x002ABB09
		private string GetLabelWithNewlineCached(string label)
		{
			if (!MainTabWindow_Research.labelsWithNewlineCached.ContainsKey(label))
			{
				MainTabWindow_Research.labelsWithNewlineCached.Add(label, label + "\n");
			}
			return MainTabWindow_Research.labelsWithNewlineCached[label];
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x002AD93C File Offset: 0x002ABB3C
		private string GetTechprintsInfoCached(int applied, int total)
		{
			Pair<int, int> key = new Pair<int, int>(applied, total);
			if (!MainTabWindow_Research.techprintsInfoCached.ContainsKey(key))
			{
				MainTabWindow_Research.techprintsInfoCached.Add(key, string.Format("{0} / {1}", applied.ToString(), total.ToString()));
			}
			return MainTabWindow_Research.techprintsInfoCached[key];
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x002AD990 File Offset: 0x002ABB90
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
				Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(project.requiredResearchBuilding, -1), null, 2f, 6f, null, false, null);
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

		// Token: 0x06007909 RID: 30985 RVA: 0x002ADC20 File Offset: 0x002ABE20
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
					Rect rect2 = new Rect(rect.x, rect.yMin, rect.width, 24f);
					Color? color = null;
					if (this.quickSearchWidget.filter.Active)
					{
						if (this.MatchesUnlockedDef(def))
						{
							QuickSearchWidget.DrawTextHighlight(rect2, 4f);
						}
						else
						{
							color = new Color?(this.NoMatchTint(Widgets.NormalOptionColor));
						}
					}
					Dialog_InfoCard.Hyperlink hyperlink = new Dialog_InfoCard.Hyperlink(def, -1);
					Widgets.HyperlinkWithIcon(rect2, hyperlink, null, 2f, 6f, color, false, this.LabelSuffixForUnlocked(def));
					rect.yMin += 24f;
				}
			}
			return rect.yMin - yMin;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x002ADE34 File Offset: 0x002AC034
		private string LabelSuffixForUnlocked(Def unlocked)
		{
			if (!ModLister.IdeologyInstalled)
			{
				return null;
			}
			this.tmpSuffixesForUnlocked.Clear();
			foreach (MemeDef memeDef in DefDatabase<MemeDef>.AllDefs)
			{
				if (memeDef.AllDesignatorBuildables.Contains(unlocked))
				{
					this.tmpSuffixesForUnlocked.AddDistinct(memeDef.LabelCap);
				}
				if (!memeDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = memeDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddDistinct(memeDef.LabelCap);
							}
						}
					}
				}
			}
			foreach (CultureDef cultureDef in DefDatabase<CultureDef>.AllDefs)
			{
				if (!cultureDef.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
				{
					using (List<ThingStyleCategoryWithPriority>.Enumerator enumerator2 = cultureDef.thingStyleCategories.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.category.AllDesignatorBuildables.Contains(unlocked))
							{
								this.tmpSuffixesForUnlocked.AddDistinct(cultureDef.LabelCap);
							}
						}
					}
				}
			}
			if (!this.tmpSuffixesForUnlocked.Any<string>())
			{
				return null;
			}
			return " (" + this.tmpSuffixesForUnlocked.ToCommaList(false, false) + ")";
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x002ADFF8 File Offset: 0x002AC1F8
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

		// Token: 0x0600790C RID: 30988 RVA: 0x002AE040 File Offset: 0x002AC240
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

		// Token: 0x0600790D RID: 30989 RVA: 0x002AE0B4 File Offset: 0x002AC2B4
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

		// Token: 0x0600790E RID: 30990 RVA: 0x002AE2EC File Offset: 0x002AC4EC
		private string GetLabel(ResearchProjectDef r)
		{
			return r.LabelCap;
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x002AE2F9 File Offset: 0x002AC4F9
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

		// Token: 0x06007910 RID: 30992 RVA: 0x002AE31C File Offset: 0x002AC51C
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
			Widgets.HyperlinkWithIcon(rect, new Dialog_InfoCard.Hyperlink(requiredFacility, -1), text, 2f, 6f, null, false, null);
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x002AE420 File Offset: 0x002AC620
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

		// Token: 0x06007912 RID: 30994 RVA: 0x002AE4D4 File Offset: 0x002AC6D4
		private float GetResearchBenchRequirementsScore(Building_ResearchBench bench, List<ThingDef> requiredFacilities)
		{
			MainTabWindow_Research.<>c__DisplayClass90_0 CS$<>8__locals1 = new MainTabWindow_Research.<>c__DisplayClass90_0();
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

		// Token: 0x06007913 RID: 30995 RVA: 0x002AE5AC File Offset: 0x002AC7AC
		private void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingProjects.Clear();
			foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord in this.tabs)
			{
				researchTabRecord.Reset();
			}
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (ResearchProjectDef researchProjectDef in this.VisibleResearchProjects)
			{
				if (this.quickSearchWidget.filter.Matches(this.GetLabel(researchProjectDef)) || this.<UpdateSearchResults>g__MatchesUnlockedDefs|91_0(researchProjectDef))
				{
					this.matchingProjects.Add(researchProjectDef);
				}
			}
			this.quickSearchWidget.noResultsMatched = !this.matchingProjects.Any<ResearchProjectDef>();
			using (List<MainTabWindow_Research.ResearchTabRecord>.Enumerator enumerator = this.tabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MainTabWindow_Research.ResearchTabRecord tab = enumerator.Current;
					tab.firstMatch = (from p in this.matchingProjects
					where tab.def == p.tab
					orderby p.ResearchViewX
					select p).FirstOrDefault<ResearchProjectDef>();
					if (!tab.AnyMatches)
					{
						tab.labelColor = new Color?(Color.grey);
					}
				}
			}
			if (!this.CurTabRecord.AnyMatches)
			{
				foreach (MainTabWindow_Research.ResearchTabRecord researchTabRecord2 in this.tabs)
				{
					if (researchTabRecord2.AnyMatches)
					{
						this.CurTab = researchTabRecord2.def;
						break;
					}
				}
			}
			this.scrollPositioner.Arm(true);
			if (this.CurTabRecord.firstMatch != null)
			{
				this.selectedProject = this.CurTabRecord.firstMatch;
			}
		}

		// Token: 0x06007914 RID: 30996 RVA: 0x002AE7F0 File Offset: 0x002AC9F0
		private bool MatchesUnlockedDef(Def unlocked)
		{
			return this.quickSearchWidget.filter.Matches(unlocked.label);
		}

		// Token: 0x06007915 RID: 30997 RVA: 0x002AE808 File Offset: 0x002ACA08
		public override void Notify_ClickOutsideWindow()
		{
			base.Notify_ClickOutsideWindow();
			this.quickSearchWidget.Unfocus();
		}

		// Token: 0x06007918 RID: 31000 RVA: 0x002AE918 File Offset: 0x002ACB18
		[CompilerGenerated]
		private bool <UpdateSearchResults>g__MatchesUnlockedDefs|91_0(ResearchProjectDef proj)
		{
			foreach (Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>> pair in this.UnlockedDefsGroupedByPrerequisites(proj))
			{
				foreach (Def unlocked in pair.Second)
				{
					if (this.MatchesUnlockedDef(unlocked))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004341 RID: 17217
		protected ResearchProjectDef selectedProject;

		// Token: 0x04004342 RID: 17218
		private ScrollPositioner scrollPositioner = new ScrollPositioner();

		// Token: 0x04004343 RID: 17219
		private bool requiredByThisFound;

		// Token: 0x04004344 RID: 17220
		private Vector2 leftScrollPosition = Vector2.zero;

		// Token: 0x04004345 RID: 17221
		private float leftScrollViewHeight;

		// Token: 0x04004346 RID: 17222
		private Vector2 rightScrollPosition;

		// Token: 0x04004347 RID: 17223
		private float rightViewWidth;

		// Token: 0x04004348 RID: 17224
		private float rightViewHeight;

		// Token: 0x04004349 RID: 17225
		private ResearchTabDef curTabInt;

		// Token: 0x0400434A RID: 17226
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400434B RID: 17227
		private bool editMode;

		// Token: 0x0400434C RID: 17228
		private List<ResearchProjectDef> draggingTabs = new List<ResearchProjectDef>();

		// Token: 0x0400434D RID: 17229
		private List<MainTabWindow_Research.ResearchTabRecord> tabs = new List<MainTabWindow_Research.ResearchTabRecord>();

		// Token: 0x0400434E RID: 17230
		private List<ResearchProjectDef> cachedVisibleResearchProjects;

		// Token: 0x0400434F RID: 17231
		private Dictionary<ResearchProjectDef, List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>> cachedUnlockedDefsGroupedByPrerequisites;

		// Token: 0x04004350 RID: 17232
		private readonly HashSet<ResearchProjectDef> matchingProjects = new HashSet<ResearchProjectDef>();

		// Token: 0x04004351 RID: 17233
		private const float leftAreaWidthPercent = 0.22f;

		// Token: 0x04004352 RID: 17234
		private const float LeftAreaWidthMin = 200f;

		// Token: 0x04004353 RID: 17235
		private const int ModeSelectButHeight = 40;

		// Token: 0x04004354 RID: 17236
		private const float ProjectTitleHeight = 50f;

		// Token: 0x04004355 RID: 17237
		private const float ProjectTitleLeftMargin = 0f;

		// Token: 0x04004356 RID: 17238
		private const int ResearchItemW = 140;

		// Token: 0x04004357 RID: 17239
		private const int ResearchItemH = 50;

		// Token: 0x04004358 RID: 17240
		private const int ResearchItemPaddingW = 50;

		// Token: 0x04004359 RID: 17241
		private const int ResearchItemPaddingH = 50;

		// Token: 0x0400435A RID: 17242
		private const int ColumnMaxProjects = 6;

		// Token: 0x0400435B RID: 17243
		private const float LineOffsetFactor = 0.48f;

		// Token: 0x0400435C RID: 17244
		private const float IndentSpacing = 6f;

		// Token: 0x0400435D RID: 17245
		private const float RowHeight = 24f;

		// Token: 0x0400435E RID: 17246
		private const float LeftVerticalPadding = 10f;

		// Token: 0x0400435F RID: 17247
		private const float LeftStartButHeight = 68f;

		// Token: 0x04004360 RID: 17248
		private const float LeftProgressBarHeight = 35f;

		// Token: 0x04004361 RID: 17249
		private const float SearchBoxHeight = 24f;

		// Token: 0x04004362 RID: 17250
		private const int SearchHighlightMargin = 12;

		// Token: 0x04004363 RID: 17251
		private const KeyCode SelectMultipleKey = KeyCode.LeftShift;

		// Token: 0x04004364 RID: 17252
		private const KeyCode DeselectKey = KeyCode.LeftControl;

		// Token: 0x04004365 RID: 17253
		private static readonly Texture2D ResearchBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04004366 RID: 17254
		private static readonly Texture2D ResearchBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f));

		// Token: 0x04004367 RID: 17255
		private static readonly Color FulfilledPrerequisiteColor = Color.green;

		// Token: 0x04004368 RID: 17256
		private static readonly Color MissingPrerequisiteColor = ColorLibrary.RedReadable;

		// Token: 0x04004369 RID: 17257
		private static readonly Color ProjectWithMissingPrerequisiteLabelColor = Color.gray;

		// Token: 0x0400436A RID: 17258
		private static readonly Color NoMatchTintColor = Widgets.MenuSectionBGFillColor;

		// Token: 0x0400436B RID: 17259
		private const float NoMatchTintFactor = 0.4f;

		// Token: 0x0400436C RID: 17260
		private List<ValueTuple<BuildableDef, List<string>>> cachedDefsWithMissingMemes = new List<ValueTuple<BuildableDef, List<string>>>();

		// Token: 0x0400436D RID: 17261
		private static Dictionary<string, string> labelsWithNewlineCached = new Dictionary<string, string>();

		// Token: 0x0400436E RID: 17262
		private static Dictionary<Pair<int, int>, string> techprintsInfoCached = new Dictionary<Pair<int, int>, string>();

		// Token: 0x0400436F RID: 17263
		private List<string> tmpSuffixesForUnlocked = new List<string>();

		// Token: 0x04004370 RID: 17264
		private static List<Building> tmpAllBuildings = new List<Building>();

		// Token: 0x02002785 RID: 10117
		private class ResearchTabRecord : TabRecord
		{
			// Token: 0x170020EA RID: 8426
			// (get) Token: 0x0600D9F4 RID: 55796 RVA: 0x004143D0 File Offset: 0x004125D0
			public bool AnyMatches
			{
				get
				{
					return this.firstMatch != null;
				}
			}

			// Token: 0x0600D9F5 RID: 55797 RVA: 0x004143DB File Offset: 0x004125DB
			public ResearchTabRecord(ResearchTabDef def, string label, Action clickedAction, Func<bool> selected) : base(label, clickedAction, selected)
			{
				this.def = def;
			}

			// Token: 0x0600D9F6 RID: 55798 RVA: 0x004143EE File Offset: 0x004125EE
			public void Reset()
			{
				this.firstMatch = null;
				this.labelColor = null;
			}

			// Token: 0x040095A5 RID: 38309
			public readonly ResearchTabDef def;

			// Token: 0x040095A6 RID: 38310
			public ResearchProjectDef firstMatch;
		}
	}
}
