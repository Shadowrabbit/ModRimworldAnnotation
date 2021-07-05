using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000F7E RID: 3966
	public abstract class BiomeWorker
	{
		// Token: 0x0600570C RID: 22284
		public abstract float GetScore(Tile tile, int tileID);
	}
}
