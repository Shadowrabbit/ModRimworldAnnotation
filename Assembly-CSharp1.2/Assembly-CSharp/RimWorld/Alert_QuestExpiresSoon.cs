using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001944 RID: 6468
	public class Alert_QuestExpiresSoon : Alert
	{
		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x06008F53 RID: 36691 RVA: 0x00294074 File Offset: 0x00292274
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

		// Token: 0x06008F54 RID: 36692 RVA: 0x0005FEDB File Offset: 0x0005E0DB
		public Alert_QuestExpiresSoon()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F55 RID: 36693 RVA: 0x0005FF92 File Offset: 0x0005E192
		protected override void OnClick()
		{
			if (this.QuestExpiring != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
				((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.QuestExpiring);
			}
		}

		// Token: 0x06008F56 RID: 36694 RVA: 0x00294104 File Offset: 0x00292304
		public override string GetLabel()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoon".Translate(questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06008F57 RID: 36695 RVA: 0x00294144 File Offset: 0x00292344
		public override TaggedString GetExplanation()
		{
			Quest questExpiring = this.QuestExpiring;
			if (questExpiring == null)
			{
				return string.Empty;
			}
			return "QuestExpiresSoonDesc".Translate(questExpiring.name, questExpiring.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor));
		}

		// Token: 0x06008F58 RID: 36696 RVA: 0x0005FFC6 File Offset: 0x0005E1C6
		public override AlertReport GetReport()
		{
			return this.QuestExpiring != null;
		}

		// Token: 0x04005B5E RID: 23390
		private const int TicksToAlert = 60000;
	}
}
