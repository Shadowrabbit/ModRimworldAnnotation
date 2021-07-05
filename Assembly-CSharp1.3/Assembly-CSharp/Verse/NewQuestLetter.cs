using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000409 RID: 1033
	public class NewQuestLetter : ChoiceLetter
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000C13BF File Offset: 0x000BF5BF
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

		// Token: 0x06001EFD RID: 7933 RVA: 0x000C13D0 File Offset: 0x000BF5D0
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
