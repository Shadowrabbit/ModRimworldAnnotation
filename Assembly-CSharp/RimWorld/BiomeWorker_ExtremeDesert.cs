using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200157C RID: 5500
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x0600775D RID: 30557 RVA: 0x0024442C File Offset: 0x0024262C
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340f)
			{
				return 0f;
			}
			return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
		}
	}
}
