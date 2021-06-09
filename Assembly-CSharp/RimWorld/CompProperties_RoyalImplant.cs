using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182E RID: 6190
	public class CompProperties_RoyalImplant : CompProperties
	{
		// Token: 0x06008940 RID: 35136 RVA: 0x0005C2CF File Offset: 0x0005A4CF
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		// Token: 0x0400580A RID: 22538
		public HediffDef implantHediff;
	}
}
