using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200157D RID: 5501
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x0600775F RID: 30559 RVA: 0x00050860 File Offset: 0x0004EA60
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x00050876 File Offset: 0x0004EA76
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
