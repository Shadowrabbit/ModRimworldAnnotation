using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183A RID: 6202
	public class CompProperties_ShipLandingBeacon : CompProperties
	{
		// Token: 0x06008982 RID: 35202 RVA: 0x0005C593 File Offset: 0x0005A793
		public CompProperties_ShipLandingBeacon()
		{
			this.compClass = typeof(CompShipLandingBeacon);
		}

		// Token: 0x04005825 RID: 22565
		public FloatRange edgeLengthRange;
	}
}
