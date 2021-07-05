using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001119 RID: 4377
	public class CompProperties_CauseHediff_Apparel : CompProperties
	{
		// Token: 0x06006921 RID: 26913 RVA: 0x002375E3 File Offset: 0x002357E3
		public CompProperties_CauseHediff_Apparel()
		{
			this.compClass = typeof(CompCauseHediff_Apparel);
		}

		// Token: 0x04003ADB RID: 15067
		public HediffDef hediff;

		// Token: 0x04003ADC RID: 15068
		public BodyPartDef part;
	}
}
