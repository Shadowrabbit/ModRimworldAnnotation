using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113E RID: 4414
	public class CompProperties_GiveHediffSeverity : CompProperties
	{
		// Token: 0x06006A0A RID: 27146 RVA: 0x0023B147 File Offset: 0x00239347
		public CompProperties_GiveHediffSeverity()
		{
			this.compClass = typeof(CompGiveHediffSeverity);
		}

		// Token: 0x04003B26 RID: 15142
		public HediffDef hediff;

		// Token: 0x04003B27 RID: 15143
		public float range;

		// Token: 0x04003B28 RID: 15144
		public float severityPerSecond;

		// Token: 0x04003B29 RID: 15145
		public bool drugExposure;
	}
}
