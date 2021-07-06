using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001581 RID: 5505
	public class BiomeWorker_TemperateSwamp : BiomeWorker
	{
		// Token: 0x06007769 RID: 30569 RVA: 0x002445F8 File Offset: 0x002427F8
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
