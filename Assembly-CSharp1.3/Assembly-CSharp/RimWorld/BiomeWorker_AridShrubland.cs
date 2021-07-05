using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EA8 RID: 3752
	public class BiomeWorker_AridShrubland : BiomeWorker
	{
		// Token: 0x0600584D RID: 22605 RVA: 0x001E0268 File Offset: 0x001DE468
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
