using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EA9 RID: 3753
	public class BiomeWorker_BorealForest : BiomeWorker
	{
		// Token: 0x0600584F RID: 22607 RVA: 0x001E02E9 File Offset: 0x001DE4E9
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
			return 15f;
		}
	}
}
