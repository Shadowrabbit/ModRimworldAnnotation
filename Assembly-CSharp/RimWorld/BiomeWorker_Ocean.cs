using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200157F RID: 5503
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x06007765 RID: 30565 RVA: 0x000508B6 File Offset: 0x0004EAB6
		public override float GetScore(Tile tile, int tileID)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return 0f;
		}
	}
}
