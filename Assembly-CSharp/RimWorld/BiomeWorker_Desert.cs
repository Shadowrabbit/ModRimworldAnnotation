using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200157B RID: 5499
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x0600775B RID: 30555 RVA: 0x00050831 File Offset: 0x0004EA31
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 600f)
			{
				return 0f;
			}
			return tile.temperature + 0.0001f;
		}
	}
}
