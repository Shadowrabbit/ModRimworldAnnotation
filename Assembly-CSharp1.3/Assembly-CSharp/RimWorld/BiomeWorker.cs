using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000A54 RID: 2644
	public abstract class BiomeWorker
	{
		// Token: 0x06003FB5 RID: 16309
		public abstract float GetScore(Tile tile, int tileID);
	}
}
