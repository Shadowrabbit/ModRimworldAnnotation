using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001250 RID: 4688
	public class Alert_FuelNodeIgnition : Alert_ActionDelay
	{
		// Token: 0x0600706D RID: 28781 RVA: 0x002570EF File Offset: 0x002552EF
		public Alert_FuelNodeIgnition()
		{
		}

		// Token: 0x0600706E RID: 28782 RVA: 0x00257322 File Offset: 0x00255522
		public Alert_FuelNodeIgnition(SignalAction_StartWick startWickAction)
		{
			this.startWickAction = startWickAction;
		}

		// Token: 0x0600706F RID: 28783 RVA: 0x00257331 File Offset: 0x00255531
		public override AlertReport GetReport()
		{
			if (this.startWickAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritIs(this.startWickAction.thingWithWick);
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x00257356 File Offset: 0x00255556
		public override string GetLabel()
		{
			return "AlertFuelNodeIgniting".Translate(this.startWickAction.delayTicks.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x00257380 File Offset: 0x00255580
		public override TaggedString GetExplanation()
		{
			return "AlertFuelNodeIgnitingDesc".Translate();
		}

		// Token: 0x04003E0E RID: 15886
		private SignalAction_StartWick startWickAction;
	}
}
