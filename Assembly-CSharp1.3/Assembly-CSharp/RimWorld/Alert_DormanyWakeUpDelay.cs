using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124D RID: 4685
	public class Alert_DormanyWakeUpDelay : Alert_ActionDelay
	{
		// Token: 0x0600705E RID: 28766 RVA: 0x002570EF File Offset: 0x002552EF
		public Alert_DormanyWakeUpDelay()
		{
		}

		// Token: 0x0600705F RID: 28767 RVA: 0x002570F7 File Offset: 0x002552F7
		public Alert_DormanyWakeUpDelay(SignalAction_DormancyWakeUp wakeUpDelay)
		{
			this.wakeUpDelay = wakeUpDelay;
		}

		// Token: 0x06007060 RID: 28768 RVA: 0x00257108 File Offset: 0x00255308
		public override AlertReport GetReport()
		{
			if (this.wakeUpDelay == null)
			{
				return AlertReport.Inactive;
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = this.wakeUpDelay;
			if (((signalAction_DormancyWakeUp != null) ? signalAction_DormancyWakeUp.lord : null) != null && !this.wakeUpDelay.lord.ownedPawns.NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(this.wakeUpDelay.lord.ownedPawns);
			}
			return AlertReport.Active;
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x0025716C File Offset: 0x0025536C
		public override string GetLabel()
		{
			return "AlertHostilesWakingUp".Translate(this.wakeUpDelay.lord.faction, this.wakeUpDelay.delayTicks.ToStringTicksToPeriod(true, false, true, true)).CapitalizeFirst();
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x002571C0 File Offset: 0x002553C0
		public override TaggedString GetExplanation()
		{
			return "AlertHostilesWakingUpDesc".Translate(this.wakeUpDelay.lord.faction).CapitalizeFirst();
		}

		// Token: 0x04003E0A RID: 15882
		private SignalAction_DormancyWakeUp wakeUpDelay;
	}
}
