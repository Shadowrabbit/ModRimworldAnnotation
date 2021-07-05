using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EB2 RID: 3762
	public class BiomeWorker_TropicalRainforest : BiomeWorker
	{
		// Token: 0x06005863 RID: 22627 RVA: 0x001E066C File Offset: 0x001DE86C
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
