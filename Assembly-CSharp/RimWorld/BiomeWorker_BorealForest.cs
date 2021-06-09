using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001579 RID: 5497
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x06007757 RID: 30551 RVA: 0x000507F6 File Offset: 0x0004E9F6
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < -10f)
			{
				return 0f;
			}
			if (tile.rainfall < 600f)
			{
				return 0f;
			}
			return 15f;
		}
	}
}
