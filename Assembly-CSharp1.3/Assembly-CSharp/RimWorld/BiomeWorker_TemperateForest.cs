using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EB0 RID: 3760
	public class BiomeWorker_TemperateForest : BiomeWorker
	{
		// Token: 0x0600585F RID: 22623 RVA: 0x001E057C File Offset: 0x001DE77C
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
