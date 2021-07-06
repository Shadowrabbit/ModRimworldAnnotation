using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA5 RID: 6821
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x060096B3 RID: 38579 RVA: 0x00064A29 File Offset: 0x00062C29
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x060096B4 RID: 38580 RVA: 0x002BE250 File Offset: 0x002BC450
		public override void PreOpen()
		{
			base.PreOpen();
			if (this.storyteller == null)
			{
				this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
				where d.listVisible
				orderby d.listOrder
				select d).First<StorytellerDef>();
			}
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x060096B5 RID: 38581 RVA: 0x002BE2C8 File Offset: 0x002BC4C8
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			StorytellerUI.DrawStorytellerSelectionInterface_NewTemp(base.GetMainRect(rect, 0f, false), ref this.storyteller, ref this.difficulty, ref this.difficultyValues, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true, true);
			Rect rect2 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060096B6 RID: 38582 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete]
		private void OpenDifficultyUnlockConfirmation()
		{
		}

		// Token: 0x060096B7 RID: 38583 RVA: 0x002BE378 File Offset: 0x002BC578
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.difficulty == null)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				this.difficulty = DifficultyDefOf.Rough;
				this.difficultyValues = new Difficulty(this.difficulty);
			}
			if (!Find.GameInitData.permadeathChosen)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Reload anytime mode has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				Find.GameInitData.permadeathChosen = true;
				Find.GameInitData.permadeath = false;
			}
			Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty, this.difficultyValues);
			return true;
		}

		// Token: 0x0400601D RID: 24605
		private StorytellerDef storyteller;

		// Token: 0x0400601E RID: 24606
		private DifficultyDef difficulty;

		// Token: 0x0400601F RID: 24607
		private Difficulty difficultyValues = new Difficulty();

		// Token: 0x04006020 RID: 24608
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
