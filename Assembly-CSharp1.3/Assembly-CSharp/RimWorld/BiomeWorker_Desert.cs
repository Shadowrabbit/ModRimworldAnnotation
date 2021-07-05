using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EAB RID: 3755
	public class BiomeWorker_Desert : BiomeWorker
	{
		// Token: 0x06005853 RID: 22611 RVA: 0x001E037F File Offset: 0x001DE57F
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
