using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200029C RID: 668
	public class HediffCompProperties_GiveHediffsInRange : HediffCompProperties
	{
		// Token: 0x06001280 RID: 4736 RVA: 0x0006A777 File Offset: 0x00068977
		public HediffCompProperties_GiveHediffsInRange()
		{
			this.compClass = typeof(HediffComp_GiveHediffsInRange);
		}

		// Token: 0x04000DFC RID: 3580
		public float range;

		// Token: 0x04000DFD RID: 3581
		public TargetingParameters targetingParameters;

		// Token: 0x04000DFE RID: 3582
		public HediffDef hediff;

		// Token: 0x04000DFF RID: 3583
		public ThingDef mote;

		// Token: 0x04000E00 RID: 3584
		public bool hideMoteWhenNotDrafted;

		// Token: 0x04000E01 RID: 3585
		public float initialSeverity = 1f;

		// Token: 0x04000E02 RID: 3586
		public bool onlyPawnsInSameFaction = true;
	}
}
