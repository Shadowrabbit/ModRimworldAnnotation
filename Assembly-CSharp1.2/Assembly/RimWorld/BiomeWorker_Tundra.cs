using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001584 RID: 5508
	public class BiomeWorker_Tundra : BiomeWorker
	{
		// Token: 0x0600776F RID: 30575 RVA: 0x000508CB File Offset: 0x0004EACB
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
