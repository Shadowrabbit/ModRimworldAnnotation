using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001BDF RID: 7135
	public abstract class Lesson_Instruction : Lesson
	{
		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06009D13 RID: 40211 RVA: 0x00068852 File Offset: 0x00066A52
		protected Map Map
		{
			get
			{
				return Find.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06009D14 RID: 40212 RVA: 0x00014941 File Offset: 0x00012B41
		protected virtual float ProgressPercent
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06009D15 RID: 40213 RVA: 0x00068859 File Offset: 0x00066A59
		protected virtual bool ShowProgressBar
		{
			get
			{
				return this.ProgressPercent >= 0f;
			}
		}

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x06009D16 RID: 40214 RVA: 0x0006886B File Offset: 0x00066A6B
		public override string DefaultRejectInputMessage
		{
			get
			{
				return this.def.rejectInputMessage;
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06009D17 RID: 40215 RVA: 0x00068878 File Offset: 0x00066A78
		public override InstructionDef Instruction
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x06009D18 RID: 40216 RVA: 0x00068880 File Offset: 0x00066A80
		public override void ExposeData()
		{
			Scribe_Defs.Look<InstructionDef>(ref this.def, "def");
			base.ExposeData();
		}

		// Token: 0x06009D19 RID: 40217 RVA: 0x002DF268 File Offset: 0x002DD468
		public override void OnActivated()
		{
			base.OnActivated();
			if (this.def.giveOnActivateCount > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.def.giveOnActivateDef, null);
				thing.stackCount = this.def.giveOnActivateCount;
				GenSpawn.Spawn(thing, TutorUtility.FindUsableRect(2, 2, this.Map, 0f, false).CenterCell, this.Map, WipeMode.Vanish);
			}
			if (this.def.resetBuildDesignatorStuffs)
			{
				foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
				{
					foreach (Designator designator in designationCategoryDef.ResolvedAllowedDesignators)
					{
						Designator_Build designator_Build = designator as Designator_Build;
						if (designator_Build != null)
						{
							designator_Build.ResetStuffToDefault();
						}
					}
				}
			}
		}

		// Token: 0x06009D1A RID: 40218 RVA: 0x002DF35C File Offset: 0x002DD55C
		public override void LessonOnGUI()
		{
			Text.Font = GameFont.Small;
			string textAdj = this.def.text.AdjustedForKeys(null, true);
			float num = Text.CalcHeight(textAdj, 290f) + 20f;
			if (this.ShowProgressBar)
			{
				num += 47f;
			}
			Vector2 vector = new Vector2((float)UI.screenWidth - 17f - 155f, 17f + num / 2f);
			if (!Find.TutorialState.introDone)
			{
				float screenOverlayAlpha = 0f;
				if (this.def.startCentered)
				{
					Vector2 vector2 = new Vector2((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
					if (base.AgeSeconds < 4f)
					{
						vector = vector2;
						screenOverlayAlpha = 0.9f;
					}
					else if (base.AgeSeconds < 5f)
					{
						float t = (base.AgeSeconds - 4f) / 1f;
						vector = Vector2.Lerp(vector2, vector, t);
						screenOverlayAlpha = Mathf.Lerp(0.9f, 0f, t);
					}
				}
				if (screenOverlayAlpha > 0f)
				{
					Rect fullScreenRect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
					Find.WindowStack.ImmediateWindow(972651, fullScreenRect, WindowLayer.SubSuper, delegate
					{
						GUI.color = new Color(1f, 1f, 1f, screenOverlayAlpha);
						GUI.DrawTexture(fullScreenRect, BaseContent.BlackTex);
						GUI.color = Color.white;
					}, false, true, 0f);
				}
				else
				{
					Find.TutorialState.introDone = true;
				}
			}
			Rect mainRect = new Rect(vector.x - 155f, vector.y - num / 2f - 10f, 310f, num);
			if (Find.TutorialState.introDone && Find.WindowStack.IsOpen<Page_ConfigureStartingPawns>())
			{
				Rect mainRect2 = mainRect;
				mainRect2.x = 17f;
				if ((mainRect.Contains(Event.current.mousePosition) || (this.def == InstructionDefOf.RandomizeCharacter && UI.screenHeight <= 768)) && !mainRect2.Contains(Event.current.mousePosition))
				{
					mainRect.x = 17f;
				}
			}
			Find.WindowStack.ImmediateWindow(177706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Text.Font = GameFont.Small;
				Rect rect3 = rect2;
				if (this.ShowProgressBar)
				{
					rect3.height -= 47f;
				}
				Widgets.Label(rect3, textAdj);
				if (this.ShowProgressBar)
				{
					Widgets.FillableBar(new Rect(rect2.x, rect2.yMax - 30f, rect2.width, 30f), this.ProgressPercent, LearningReadout.ProgressBarFillTex);
				}
				if (this.AgeSeconds < 0.5f)
				{
					GUI.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, this.AgeSeconds / 0.5f));
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = Color.white;
				}
			}, false, false, 1f);
			if (this.def.highlightTags != null)
			{
				for (int i = 0; i < this.def.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.def.highlightTags[i]);
				}
			}
		}

		// Token: 0x06009D1B RID: 40219 RVA: 0x00068898 File Offset: 0x00066A98
		public override void Notify_Event(EventPack ep)
		{
			if (this.def.eventTagsEnd != null && this.def.eventTagsEnd.Contains(ep.Tag))
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06009D1C RID: 40220 RVA: 0x000688CA File Offset: 0x00066ACA
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return this.def.actionTagsAllowed != null && this.def.actionTagsAllowed.Contains(ep.Tag);
		}

		// Token: 0x06009D1D RID: 40221 RVA: 0x002DF618 File Offset: 0x002DD818
		public override void PostDeactivated()
		{
			SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			TutorSystem.Notify_Event("InstructionDeactivated-" + this.def.defName);
			if (this.def.endTutorial)
			{
				Find.ActiveLesson.Deactivate();
				Find.TutorialState.Notify_TutorialEnding();
				LessonAutoActivator.Notify_TutorialEnding();
			}
		}

		// Token: 0x040063EE RID: 25582
		public InstructionDef def;

		// Token: 0x040063EF RID: 25583
		private const float RectWidth = 310f;

		// Token: 0x040063F0 RID: 25584
		private const float BarHeight = 30f;
	}
}
