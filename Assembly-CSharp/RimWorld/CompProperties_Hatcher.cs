using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CD RID: 6093
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x060086C3 RID: 34499 RVA: 0x0005A678 File Offset: 0x00058878
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x040056A4 RID: 22180
		public float hatcherDaystoHatch = 1f;

		// Token: 0x040056A5 RID: 22181
		public PawnKindDef hatcherPawn;
	}
}
