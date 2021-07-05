using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200157A RID: 5498
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06007759 RID: 30553 RVA: 0x002443D0 File Offset: 0x002425D0
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
			if (tile.swampiness < 0.5f)
			{
				return 0f;
			}
			return -tile.temperature + 13f + tile.swampiness * 8f;
		}
	}
}
