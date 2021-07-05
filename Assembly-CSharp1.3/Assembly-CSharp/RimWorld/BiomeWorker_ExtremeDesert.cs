using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EAC RID: 3756
	public class BiomeWorker_ExtremeDesert : BiomeWorker
	{
		// Token: 0x06005855 RID: 22613 RVA: 0x001E03B0 File Offset: 0x001DE5B0
		public override float GetScore(Tile tile, int tileID)
		{
			if (tile.WaterCovered)
			{
				return -100f;
			}
			if (tile.rainfall >= 340f)
			{
				return 0f;
			}
			return tile.temperature * 2.7f - 13f - tile.rainfall * 0.14f;
		}
	}
}
