using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EB1 RID: 3761
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		// Token: 0x06005861 RID: 22625 RVA: 0x001E05E4 File Offset: 0x001DE7E4
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return 15f + (tile.temperature - 7f) + (tile.rainfall - 600f) / 180f + tile.swampiness * 3f;
		}
	}
}
