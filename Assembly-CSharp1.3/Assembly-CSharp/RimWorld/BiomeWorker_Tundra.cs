using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EB4 RID: 3764
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x06005867 RID: 22631 RVA: 0x001E0764 File Offset: 0x001DE964
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return -tile.temperature;
		}
	}
}
