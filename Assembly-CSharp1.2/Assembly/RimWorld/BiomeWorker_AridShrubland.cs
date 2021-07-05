using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001578 RID: 5496
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		// Token: 0x06007755 RID: 30549 RVA: 0x00244354 File Offset: 0x00242554
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
			if (tile.rainfall < 600f || tile.rainfall >= 2000f)
			{
				return 0f;
			}
			return 22.5f + (tile.temperature - 20f) * 2.2f + (tile.rainfall - 600f) / 100f;
		}
	}
}
