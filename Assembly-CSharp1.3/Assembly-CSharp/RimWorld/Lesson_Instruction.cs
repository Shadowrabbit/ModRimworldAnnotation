using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013D6 RID: 5078
	public abstract class Lesson_Instruction : Lesson
	{
		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x06007B79 RID: 31609 RVA: 0x002B86BB File Offset: 0x002B68BB
		protected Map Map
		{
			get
			{
				return Find.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06007B7A RID: 31610 RVA: 0x00059779 File Offset: 0x00057979
		protected virtual float ProgressPercent
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06007B7B RID: 31611 RVA: 0x002B86C2 File Offset: 0x002B68C2
		protected virtual bool ShowProgressBar
		{
			get
			{
				return this.ProgressPercent >= 0f;
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06007B7C RID: 31612 RVA: 0x002B86D4 File Offset: 0x002B68D4
		public override string DefaultRejectInputMessage
		{
			get
			{
				return this.def.rejectInputMessage;
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06007B7D RID: 31613 RVA: 0x002B86E1 File Offset: 0x002B68E1
		public override InstructionDef Instruction
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x06007B7E RID: 31614 RVA: 0x002B86E9 File Offset: 0x002B68E9
		public override void ExposeData()
		{
			Scribe_Defs.Look<InstructionDef>(ref this.def, "def");
			base.ExposeData();
		}

		// Token: 0x06007B7F RID: 31615 RVA: 0x002B8704 File Offset: 0x002B6904
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

		// Token: 0x06007B80 RID: 31616 RVA: 0x002B87F8 File Offset: 0x002B69F8
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
					}, false, true, 0f, null);
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
			}, false, false, 1f, null);
			if (this.def.highlightTags != null)
			{
				for (int i = 0; i < this.def.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.def.highlightTags[i]);
				}
			}
		}

		// Token: 0x06007B81 RID: 31617 RVA: 0x002B8AB5 File Offset: 0x002B6CB5
		public override void Notify_Event(EventPack ep)
		{
			if (this.def.eventTagsEnd != null && this.def.eventTagsEnd.Contains(ep.Tag))
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06007B82 RID: 31618 RVA: 0x002B8AE7 File Offset: 0x002B6CE7
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			return this.def.actionTagsAllowed != null && this.def.actionTagsAllowed.Contains(ep.Tag);
		}

		// Token: 0x06007B83 RID: 31619 RVA: 0x002B8B18 File Offset: 0x002B6D18
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

		// Token: 0x0400444B RID: 17483
		public InstructionDef def;

		// Token: 0x0400444C RID: 17484
		private const float RectWidth = 310f;

		// Token: 0x0400444D RID: 17485
		private const float BarHeight = 30f;
	}
}
