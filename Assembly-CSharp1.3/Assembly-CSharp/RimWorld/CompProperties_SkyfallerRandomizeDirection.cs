using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011DE RID: 4574
	public class CompProperties_SkyfallerRandomizeDirection : CompProperties
	{
		// Token: 0x06006E61 RID: 28257 RVA: 0x0025006E File Offset: 0x0024E26E
		public CompProperties_SkyfallerRandomizeDirection()
		{
			this.compClass = typeof(CompSkyfallerRandomizeDirection);
		}

		// Token: 0x04003D36 RID: 15670
		public IntRange directionChangeInterval;

		// Token: 0x04003D37 RID: 15671
		public float maxDeviationFromStartingAngle;
	}
}
