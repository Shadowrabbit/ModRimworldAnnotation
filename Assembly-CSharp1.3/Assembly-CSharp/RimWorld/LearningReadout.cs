using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001334 RID: 4916
	[StaticConstructorOnStartup]
	public class LearningReadout : IExposable
	{
		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x060076E4 RID: 30436 RVA: 0x0029C01F File Offset: 0x0029A21F
		public int ActiveConceptsCount
		{
			get
			{
				return this.activeConcepts.Count;
			}
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x060076E5 RID: 30437 RVA: 0x0029C02C File Offset: 0x0029A22C
		public bool ShowAllMode
		{
			get
			{
				return this.showAllMode;
			}
		}

		// Token: 0x060076E6 RID: 30438 RVA: 0x0029C034 File Offset: 0x0029A234
		public LearningReadout()
		{
			this.windowOnGUICached = new Action(this.WindowOnGUI);
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x0029C088 File Offset: 0x0029A288
		public void ExposeData()
		{
			Scribe_Collections.Look<ConceptDef>(ref this.activeConcepts, "activeConcepts", LookMode.Undefined, Array.Empty<object>());
			Scribe_Defs.Look<ConceptDef>(ref this.selectedConcept, "selectedConcept");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.activeConcepts.RemoveAll((ConceptDef c) => PlayerKnowledgeDatabase.IsComplete(c));
			}
		}

		// Token: 0x060076E8 RID: 30440 RVA: 0x0029C0EE File Offset: 0x0029A2EE
		public bool TryActivateConcept(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc))
			{
				return false;
			}
			this.activeConcepts.Add(conc);
			SoundDefOf.Lesson_Activated.PlayOneShotOnCamera(null);
			this.lastConceptActivateRealTime = RealTime.LastRealTime;
			return true;
		}

		// Token: 0x060076E9 RID: 30441 RVA: 0x0029C123 File Offset: 0x0029A323
		public bool IsActive(ConceptDef conc)
		{
			return this.activeConcepts.Contains(conc);
		}

		// Token: 0x060076EA RID: 30442 RVA: 0x0000313F File Offset: 0x0000133F
		public void LearningReadoutUpdate()
		{
		}

		// Token: 0x060076EB RID: 30443 RVA: 0x0029C134 File Offset: 0x0029A334
		public void Notify_ConceptNewlyLearned(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc) || this.selectedConcept == conc)
			{
				SoundDefOf.Lesson_Deactivated.PlayOneShotOnCamera(null);
				SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			}
			if (this.activeConcepts.Contains(conc))
			{
				this.activeConcepts.Remove(conc);
			}
			if (this.selectedConcept == conc)
			{
				this.selectedConcept = null;
			}
		}

		// Token: 0x060076EC RID: 30444 RVA: 0x0029C199 File Offset: 0x0029A399
		private string FilterSearchStringInput(string input)
		{
			if (input == this.searchString)
			{
				return input;
			}
			if (input.Length > 20)
			{
				input = input.Substring(0, 20);
			}
			return input;
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x0029C1C4 File Offset: 0x0029A3C4
		public void LearningReadoutOnGUI()
		{
			if (TutorSystem.TutorialMode || !TutorSystem.AdaptiveTrainingEnabled)
			{
				return;
			}
			if (!Find.PlaySettings.showLearningHelper && this.activeConcepts.Count == 0)
			{
				return;
			}
			if (Find.WindowStack.IsOpen<Screen_Credits>())
			{
				return;
			}
			float b = (float)UI.screenHeight / 2f;
			float a = this.contentHeight + 14f;
			this.windowRect = new Rect((float)UI.screenWidth - 8f - 200f, 8f, 200f, Mathf.Min(a, b));
			Rect rect = this.windowRect;
			Find.WindowStack.ImmediateWindow(76136312, this.windowRect, WindowLayer.Super, this.windowOnGUICached, false, false, 1f, null);
			float num = Time.realtimeSinceStartup - this.lastConceptActivateRealTime;
			if (num < 1f && num > 0f)
			{
				GenUI.DrawFlash(rect.x, rect.center.y, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num) * 0.85f, new Color(0.8f, 0.77f, 0.53f));
			}
			ConceptDef conceptDef = (this.selectedConcept != null) ? this.selectedConcept : this.mouseoverConcept;
			if (conceptDef != null)
			{
				this.DrawInfoPane(conceptDef);
				conceptDef.HighlightAllTags();
			}
			this.mouseoverConcept = null;
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x0029C318 File Offset: 0x0029A518
		private void WindowOnGUI()
		{
			Rect rect = this.windowRect.AtZero().ContractedBy(7f);
			Rect viewRect = rect.AtZero();
			bool flag = this.contentHeight > rect.height;
			Widgets.DrawWindowBackgroundTutor(this.windowRect.AtZero());
			if (flag)
			{
				viewRect.height = this.contentHeight + 40f;
				viewRect.width -= 20f;
				this.scrollPosition = GUI.BeginScrollView(rect, this.scrollPosition, viewRect);
			}
			else
			{
				GUI.BeginGroup(rect);
			}
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, viewRect.width - 24f, 24f);
			Widgets.Label(rect2, "LearningHelper".Translate());
			float num = rect2.yMax;
			if (Widgets.ButtonImage(new Rect(rect2.xMax, rect2.y, 24f, 24f), (!this.showAllMode) ? TexButton.Plus : TexButton.Minus, true))
			{
				this.showAllMode = !this.showAllMode;
				if (this.showAllMode)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (this.showAllMode)
			{
				Rect rect3 = new Rect(0f, num, viewRect.width - 20f - 2f, 28f);
				this.searchString = this.FilterSearchStringInput(Widgets.TextField(rect3, this.searchString));
				if (this.searchString == "")
				{
					GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
					Text.Anchor = TextAnchor.MiddleLeft;
					Rect rect4 = rect3;
					rect4.xMin += 7f;
					Widgets.Label(rect4, "Filter".Translate() + "...");
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				if (Widgets.ButtonImage(new Rect(viewRect.width - 20f, num + 14f - 10f, 20f, 20f), TexButton.CloseXSmall, true))
				{
					this.searchString = "";
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
				num = rect3.yMax + 4f;
			}
			LearningReadout.tmpConceptsToShow.Clear();
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.AddRange(DefDatabase<ConceptDef>.AllDefsListForReading);
			}
			else
			{
				LearningReadout.tmpConceptsToShow.AddRange(this.activeConcepts);
			}
			if (LearningReadout.tmpConceptsToShow.Any<ConceptDef>())
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(0f, num, viewRect.width);
				GUI.color = Color.white;
				num += 4f;
			}
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.SortBy((ConceptDef x) => -this.DisplayPriority(x), (ConceptDef x) => x.label);
			}
			for (int i = 0; i < LearningReadout.tmpConceptsToShow.Count; i++)
			{
				if (!LearningReadout.tmpConceptsToShow[i].TriggeredDirect)
				{
					num = this.DrawConceptListRow(0f, num, viewRect.width, LearningReadout.tmpConceptsToShow[i]).yMax;
				}
			}
			LearningReadout.tmpConceptsToShow.Clear();
			this.contentHeight = num;
			if (flag)
			{
				GUI.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x060076EF RID: 30447 RVA: 0x0029C6A4 File Offset: 0x0029A8A4
		private int DisplayPriority(ConceptDef conc)
		{
			int num = 1;
			if (this.MatchesSearchString(conc))
			{
				num += 10000;
			}
			return num;
		}

		// Token: 0x060076F0 RID: 30448 RVA: 0x0029C6C5 File Offset: 0x0029A8C5
		private bool MatchesSearchString(ConceptDef conc)
		{
			return this.searchString != "" && conc.label.IndexOf(this.searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x060076F1 RID: 30449 RVA: 0x0029C6F4 File Offset: 0x0029A8F4
		private Rect DrawConceptListRow(float x, float y, float width, ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool flag = PlayerKnowledgeDatabase.IsComplete(conc);
			object obj = !flag && knowledge > 0f;
			float num = Text.CalcHeight(conc.LabelCap, width);
			object obj2 = obj;
			if (obj2 != null)
			{
				num += 0f;
			}
			Rect rect = new Rect(x, y, width, num);
			if (obj2 != null)
			{
				Rect rect2 = new Rect(rect);
				rect2.yMin += 1f;
				rect2.yMax -= 1f;
				Widgets.FillableBar(rect2, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex, LearningReadout.ProgressBarBGTex, false);
			}
			if (flag)
			{
				GUI.DrawTexture(rect, BaseContent.GreyTex);
			}
			if (this.selectedConcept == conc)
			{
				GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.MatchesSearchString(conc))
			{
				Widgets.DrawHighlight(rect);
			}
			Widgets.Label(rect, conc.LabelCap);
			if (Mouse.IsOver(rect) && this.selectedConcept == null)
			{
				this.mouseoverConcept = conc;
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.selectedConcept == conc)
				{
					this.selectedConcept = null;
				}
				else
				{
					this.selectedConcept = conc;
				}
				SoundDefOf.PageChange.PlayOneShotOnCamera(null);
			}
			return rect;
		}

		// Token: 0x060076F2 RID: 30450 RVA: 0x0029C820 File Offset: 0x0029AA20
		private Rect DrawInfoPane(ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool complete = PlayerKnowledgeDatabase.IsComplete(conc);
			bool drawProgressBar = !complete && knowledge > 0f;
			Text.Font = GameFont.Medium;
			float titleHeight = Text.CalcHeight(conc.LabelCap, 276f);
			Text.Font = GameFont.Small;
			float textHeight = Text.CalcHeight(conc.HelpTextAdjusted, 296f);
			float num = titleHeight + textHeight + 14f + 5f;
			if (this.selectedConcept == conc)
			{
				num += 40f;
			}
			if (drawProgressBar)
			{
				num += 30f;
			}
			Rect outRect = new Rect((float)UI.screenWidth - 8f - 200f - 8f - 310f, 8f, 310f, num);
			Rect outRect2 = outRect;
			Find.WindowStack.ImmediateWindow(987612111, outRect, WindowLayer.Super, delegate
			{
				outRect = outRect.AtZero();
				Rect rect = outRect.ContractedBy(7f);
				Widgets.DrawShadowAround(outRect);
				Widgets.DrawWindowBackgroundTutor(outRect);
				Rect rect2 = rect;
				rect2.width -= 20f;
				rect2.height = titleHeight + 5f;
				Text.Font = GameFont.Medium;
				Widgets.Label(rect2, conc.LabelCap);
				Text.Font = GameFont.Small;
				Rect rect3 = rect;
				rect3.yMin = rect2.yMax;
				rect3.height = textHeight;
				Widgets.Label(rect3, conc.HelpTextAdjusted);
				if (drawProgressBar)
				{
					Rect rect4 = rect;
					rect4.yMin = rect3.yMax;
					rect4.height = 30f;
					Widgets.FillableBar(rect4, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex);
				}
				if (this.selectedConcept == conc)
				{
					if (Widgets.CloseButtonFor(outRect))
					{
						this.selectedConcept = null;
						SoundDefOf.PageChange.PlayOneShotOnCamera(null);
					}
					Rect rect5 = new Rect(rect.center.x - 70f, rect.yMax - 30f, 140f, 30f);
					if (!complete)
					{
						if (Widgets.ButtonText(rect5, "MarkLearned".Translate(), true, true, true))
						{
							this.selectedConcept = null;
							SoundDefOf.PageChange.PlayOneShotOnCamera(null);
							PlayerKnowledgeDatabase.SetKnowledge(conc, 1f);
							return;
						}
					}
					else
					{
						GUI.color = new Color(1f, 1f, 1f, 0.5f);
						Text.Anchor = TextAnchor.MiddleCenter;
						Widgets.Label(rect5, "AlreadyLearned".Translate());
						Text.Anchor = TextAnchor.UpperLeft;
						GUI.color = Color.white;
					}
				}
			}, false, false, 1f, null);
			return outRect2;
		}

		// Token: 0x04004211 RID: 16913
		private List<ConceptDef> activeConcepts = new List<ConceptDef>();

		// Token: 0x04004212 RID: 16914
		private ConceptDef selectedConcept;

		// Token: 0x04004213 RID: 16915
		private bool showAllMode;

		// Token: 0x04004214 RID: 16916
		private float contentHeight;

		// Token: 0x04004215 RID: 16917
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04004216 RID: 16918
		private string searchString = "";

		// Token: 0x04004217 RID: 16919
		private float lastConceptActivateRealTime = -999f;

		// Token: 0x04004218 RID: 16920
		private ConceptDef mouseoverConcept;

		// Token: 0x04004219 RID: 16921
		private Rect windowRect;

		// Token: 0x0400421A RID: 16922
		private Action windowOnGUICached;

		// Token: 0x0400421B RID: 16923
		private const float OuterMargin = 8f;

		// Token: 0x0400421C RID: 16924
		private const float InnerMargin = 7f;

		// Token: 0x0400421D RID: 16925
		private const float ReadoutWidth = 200f;

		// Token: 0x0400421E RID: 16926
		private const float InfoPaneWidth = 310f;

		// Token: 0x0400421F RID: 16927
		private const float OpenButtonSize = 24f;

		// Token: 0x04004220 RID: 16928
		public static readonly Texture2D ProgressBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74509805f, 0.6039216f, 0.2f));

		// Token: 0x04004221 RID: 16929
		public static readonly Texture2D ProgressBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.50980395f, 0.40784314f, 0.13333334f));

		// Token: 0x04004222 RID: 16930
		private static List<ConceptDef> tmpConceptsToShow = new List<ConceptDef>();
	}
}
