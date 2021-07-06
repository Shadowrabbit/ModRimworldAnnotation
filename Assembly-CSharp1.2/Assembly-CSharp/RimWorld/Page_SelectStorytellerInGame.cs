using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA7 RID: 6823
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x060096BD RID: 38589 RVA: 0x00064A29 File Offset: 0x00062C29
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x060096BE RID: 38590 RVA: 0x00064A74 File Offset: 0x00062C74
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x060096BF RID: 38591 RVA: 0x00064A95 File Offset: 0x00062C95
		public override void PreOpen()
		{
			base.PreOpen();
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x060096C0 RID: 38592 RVA: 0x002BE464 File Offset: 0x002BC664
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface_NewTemp(mainRect, ref storyteller.def, ref storyteller.difficulty, ref storyteller.difficultyValues, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x04006024 RID: 24612
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
