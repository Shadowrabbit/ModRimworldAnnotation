using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000090 RID: 144
	public class DamageDefAdditionalHediff
	{
		// Token: 0x0400024E RID: 590
		public HediffDef hediff;

		// Token: 0x0400024F RID: 591
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x04000250 RID: 592
		public float severityFixed;

		// Token: 0x04000251 RID: 593
		public StatDef victimSeverityScaling;

		// Token: 0x04000252 RID: 594
		public bool victimSeverityScalingByInvBodySize;
	}
}
