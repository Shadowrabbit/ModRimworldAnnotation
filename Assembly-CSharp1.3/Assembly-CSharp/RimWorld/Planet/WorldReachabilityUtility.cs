using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001731 RID: 5937
	public static class WorldReachabilityUtility
	{
		// Token: 0x060088F3 RID: 35059 RVA: 0x00313889 File Offset: 0x00311A89
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
