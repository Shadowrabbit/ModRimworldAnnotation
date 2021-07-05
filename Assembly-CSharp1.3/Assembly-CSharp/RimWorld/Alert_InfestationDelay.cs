using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124E RID: 4686
	public class Alert_InfestationDelay : Alert_ActionDelay
	{
		// Token: 0x06007063 RID: 28771 RVA: 0x002570EF File Offset: 0x002552EF
		public Alert_InfestationDelay()
		{
		}

		// Token: 0x06007064 RID: 28772 RVA: 0x002571F4 File Offset: 0x002553F4
		public Alert_InfestationDelay(SignalAction_Infestation infestationAction)
		{
			this.infestationAction = infestationAction;
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x00257204 File Offset: 0x00255404
		public override AlertReport GetReport()
		{
			if (this.infestationAction == null)
			{
				return AlertReport.Inactive;
			}
			if (this.infestationAction.overrideLoc != null)
			{
				return AlertReport.CulpritIs(new GlobalTargetInfo(this.infestationAction.overrideLoc.Value, this.infestationAction.Map, false));
			}
			return AlertReport.Active;
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x0025725D File Offset: 0x0025545D
		public override string GetLabel()
		{
			return "AlertInfestationArriving".Translate(this.infestationAction.delayTicks.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x00257287 File Offset: 0x00255487
		public override TaggedString GetExplanation()
		{
			return "AlertInfestationArrivingDesc".Translate();
		}

		// Token: 0x04003E0B RID: 15883
		private SignalAction_Infestation infestationAction;
	}
}
