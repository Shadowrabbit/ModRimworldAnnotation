using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001583 RID: 5507
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		// Token: 0x0600776D RID: 30573 RVA: 0x002446EC File Offset: 0x002428EC
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return 28f + (tile.temperature - 20f) * 1.5f + (tile.rainfall - 600f) / 165f + tile.swampiness * 3f;
		}
	}
}
