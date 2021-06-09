using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F0 RID: 240
	public class DamageDefAdditionalHediff
	{
		// Token: 0x04000417 RID: 1047
		public HediffDef hediff;

		// Token: 0x04000418 RID: 1048
		public float severityPerDamageDealt = 0.1f;

		// Token: 0x04000419 RID: 1049
		public float severityFixed;

		// Token: 0x0400041A RID: 1050
		public StatDef victimSeverityScaling;

		// Token: 0x0400041B RID: 1051
		public bool victimSeverityScalingByInvBodySize;
	}
}
