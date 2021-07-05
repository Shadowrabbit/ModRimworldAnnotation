using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EB3 RID: 3763
	public class BiomeWorker_TropicalSwamp : BiomeWorker
	{
		// Token: 0x06005865 RID: 22629 RVA: 0x001E06D8 File Offset: 0x001DE8D8
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
