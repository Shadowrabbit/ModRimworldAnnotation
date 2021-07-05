using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001146 RID: 4422
	public class CompProperties_Hatcher : CompProperties
	{
		// Token: 0x06006A2D RID: 27181 RVA: 0x0023BA6D File Offset: 0x00239C6D
		public CompProperties_Hatcher()
		{
			this.compClass = typeof(CompHatcher);
		}

		// Token: 0x04003B3C RID: 15164
		public float hatcherDaystoHatch = 1f;

		// Token: 0x04003B3D RID: 15165
		public PawnKindDef hatcherPawn;
	}
}
