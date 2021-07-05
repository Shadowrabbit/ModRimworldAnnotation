using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118B RID: 4491
	public class CompProperties_RoyalImplant : CompProperties
	{
		// Token: 0x06006C0E RID: 27662 RVA: 0x00243BB6 File Offset: 0x00241DB6
		public CompProperties_RoyalImplant()
		{
			this.compClass = typeof(CompRoyalImplant);
		}

		// Token: 0x04003C15 RID: 15381
		public HediffDef implantHediff;
	}
}
