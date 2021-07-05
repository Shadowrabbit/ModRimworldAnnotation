using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001580 RID: 5504
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x06007767 RID: 30567 RVA: 0x00244590 File Offset: 0x00242790
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
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f;
		}
	}
}
