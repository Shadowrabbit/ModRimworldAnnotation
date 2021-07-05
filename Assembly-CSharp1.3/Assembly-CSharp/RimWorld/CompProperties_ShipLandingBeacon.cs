using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001193 RID: 4499
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		// Token: 0x06006C3B RID: 27707 RVA: 0x0024464C File Offset: 0x0024284C
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		// Token: 0x04003C25 RID: 15397
		public FloatRange edgeLengthRange;
	}
}
