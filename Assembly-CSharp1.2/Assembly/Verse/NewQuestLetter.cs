using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000731 RID: 1841
	public class NewQuestLetter : ChoiceLetter
	{
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002E5D RID: 11869 RVA: 0x00024673 File Offset: 0x00022873
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewQuest", false);
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				yield return base.Option_Close;
				yield break;
			}
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x001374A0 File Offset: 0x001356A0
		public override void OpenLetter()
		{
			if (this.quest != null && !base.ArchivedOnly)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				Find.LetterStack.RemoveLetter(this);
				return;
			}
			base.OpenLetter();
		}
	}
}
