using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EAA RID: 3754
	public class BiomeWorker_ColdBog : BiomeWorker
	{
		// Token: 0x06005851 RID: 22609 RVA: 0x001E0324 File Offset: 0x001DE524
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
