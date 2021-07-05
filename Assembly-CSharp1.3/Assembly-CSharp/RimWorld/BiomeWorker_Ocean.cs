using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02000EAF RID: 3759
	public class BiomeWorker_Ocean : BiomeWorker
	{
		// Token: 0x0600585D RID: 22621 RVA: 0x001E0567 File Offset: 0x001DE767
		public override float GetScore(Tile tile, int tileID)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			return 0f;
		}
	}
}
