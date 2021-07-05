using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001328 RID: 4904
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x06007691 RID: 30353 RVA: 0x002923AB File Offset: 0x002905AB
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x002923BC File Offset: 0x002905BC
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

		// Token: 0x06007693 RID: 30355 RVA: 0x00292434 File Offset: 0x00290634
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			StorytellerUI.DrawStorytellerSelectionInterface(base.GetMainRect(rect, 0f, false), ref this.storyteller, ref this.difficulty, ref this.difficultyValues, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true, true);
			Rect rect2 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007694 RID: 30356 RVA: 0x0000313F File Offset: 0x0000133F
		[Obsolete]
		private void OpenDifficultyUnlockConfirmation()
		{
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x002924E4 File Offset: 0x002906E4
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

		// Token: 0x040041D5 RID: 16853
		private StorytellerDef storyteller;

		// Token: 0x040041D6 RID: 16854
		private DifficultyDef difficulty;

		// Token: 0x040041D7 RID: 16855
		private Difficulty difficultyValues = new Difficulty();

		// Token: 0x040041D8 RID: 16856
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
