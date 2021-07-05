using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001329 RID: 4905
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x06007697 RID: 30359 RVA: 0x002923AB File Offset: 0x002905AB
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x002925EB File Offset: 0x002907EB
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x0029260C File Offset: 0x0029080C
		public override void PreOpen()
		{
			base.PreOpen();
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x0029261C File Offset: 0x0029081C
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficultyDef, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x00292684 File Offset: 0x00290884
		public override void PreClose()
		{
			foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
			where x.costListForDifficulty != null
			select x)
			{
				thingDef.costListForDifficulty.RecacheApplies();
			}
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
		}

		// Token: 0x040041D9 RID: 16857
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
