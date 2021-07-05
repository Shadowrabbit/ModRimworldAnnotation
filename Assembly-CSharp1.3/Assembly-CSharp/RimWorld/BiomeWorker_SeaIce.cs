using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000EAE RID: 3758
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		// Token: 0x0600585A RID: 22618 RVA: 0x001E0428 File Offset: 0x001DE628
		public override float GetScore(Tile tile, int tileID)
		{
			if (!tile.WaterCovered)
			{
				return -100f;
			}
			if (!this.AllowedAt(tileID))
			{
				return -100f;
			}
			return BiomeWorker_IceSheet.PermaIceScore(tile) - 23f;
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x001E0454 File Offset: 0x001DE654
		private bool AllowedAt(int tile)
		{
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			Vector3 viewCenter = Find.WorldGrid.viewCenter;
			float value = Vector3.Angle(viewCenter, tileCenter);
			float viewAngle = Find.WorldGrid.viewAngle;
			float num = Mathf.Min(7.5f, viewAngle * 0.12f);
			float num2 = Mathf.InverseLerp(viewAngle - num, viewAngle, value);
			if (num2 <= 0f)
			{
				return true;
			}
			if (this.cachedSeaIceAllowedNoise == null || this.cachedSeaIceAllowedNoiseForSeed != Find.World.info.Seed)
			{
				this.cachedSeaIceAllowedNoise = new Perlin(0.017000000923871994, 2.0, 0.5, 6, Find.World.info.Seed, QualityMode.Medium);
				this.cachedSeaIceAllowedNoiseForSeed = Find.World.info.Seed;
			}
			float headingFromTo = Find.WorldGrid.GetHeadingFromTo(viewCenter, tileCenter);
			float num3 = (float)this.cachedSeaIceAllowedNoise.GetValue((double)headingFromTo, 0.0, 0.0) * 0.5f + 0.5f;
			return num2 <= num3;
		}

		// Token: 0x040033E9 RID: 13289
		private ModuleBase cachedSeaIceAllowedNoise;

		// Token: 0x040033EA RID: 13290
		private int cachedSeaIceAllowedNoiseForSeed;
	}
}
