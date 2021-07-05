using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124F RID: 4687
	public class Alert_CasketOpening : Alert_ActionDelay
	{
		// Token: 0x06007068 RID: 28776 RVA: 0x00257293 File Offset: 0x00255493
		public Alert_CasketOpening()
		{
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x002572A6 File Offset: 0x002554A6
		public Alert_CasketOpening(SignalAction_OpenCasket openCasketAction)
		{
			this.openCasketAction = openCasketAction;
			this.culprints.AddRange(openCasketAction.caskets);
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x002572D1 File Offset: 0x002554D1
		public override AlertReport GetReport()
		{
			if (this.openCasketAction == null)
			{
				return AlertReport.Inactive;
			}
			return AlertReport.CulpritsAre(this.culprints);
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x002572EC File Offset: 0x002554EC
		public override string GetLabel()
		{
			return "AlertCasketOpening".Translate(this.openCasketAction.delayTicks.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x00257316 File Offset: 0x00255516
		public override TaggedString GetExplanation()
		{
			return "AlertCasketOpeningDesc".Translate();
		}

		// Token: 0x04003E0C RID: 15884
		private SignalAction_OpenCasket openCasketAction;

		// Token: 0x04003E0D RID: 15885
		private List<Thing> culprints = new List<Thing>();
	}
}
