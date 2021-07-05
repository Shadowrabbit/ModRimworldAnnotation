using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001255 RID: 4693
	public class Alert_QuestExpiresSoon : Alert
	{
		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06007083 RID: 28803 RVA: 0x0025776C File Offset: 0x0025596C
		private Quest QuestExpiring
		{
			get
			{
				foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
				{
					if (!quest.dismissed && !quest.Historical && !quest.initiallyAccepted && quest.State == QuestState.NotYetAccepted && quest.ticksUntilAcceptanceExpiry > 0 && quest.ticksUntilAcceptanceExpiry < 60000)
					{
						return quest;
					}
				}
				return null;
			}
		}

		// Token: 0x06007084 RID: 28804 RVA: 0x002574EB File Offset: 0x002556EB
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x002577FC File Offset: 0x002559FC
		protected override void OnClick()
		{
			if (this.QuestExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.QuestExpiring);
			}
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x00257830 File Offset: 0x00255A30
		public override string GetLabel()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x00257870 File Offset: 0x00255A70
		public override TaggedString GetExplanation()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(questExpiring.name, questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x002578C5 File Offset: 0x00255AC5
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		// Token: 0x04003E11 RID: 15889
		private const int TicksToAlert = 60000;
	}
}
