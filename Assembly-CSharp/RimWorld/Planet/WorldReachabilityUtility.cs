using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002012 RID: 8210
	public static class WorldReachabilityUtility
	{
		// Token: 0x0600ADE5 RID: 44517 RVA: 0x00071381 File Offset: 0x0006F581
		public static bool CanReach(this Caravan c, int tile)
		{
			return Find.WorldReachability.CanReach(c, tile);
		}
	}
}
