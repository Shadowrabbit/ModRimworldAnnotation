using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EAD RID: 3757
	public class BiomeWorker_IceSheet : BiomeWorker
	{
		// Token: 0x06005857 RID: 22615 RVA: 0x001E03FD File Offset: 0x001DE5FD
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile);
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x001E0413 File Offset: 0x001DE613
		public static float PermaIceScore(Tile tile)
		{
			return -20f + -tile.temperature * 2f;
		}
	}
}
