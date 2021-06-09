using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001582 RID: 5506
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x0600776B RID: 30571 RVA: 0x00244680 File Offset: 0x00242880
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.temperature < 15f)
			{
				return 0f;
			}
			if (tile.rainfall < 2000f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f;
		}
	}
}
