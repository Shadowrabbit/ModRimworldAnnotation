using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200157E RID: 5502
	public class BiomeWorker_SeaIce : BiomeWorker
	{
		// Token: 0x06007762 RID: 30562 RVA: 0x0005088B File Offset: 0x0004EA8B
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

		// Token: 0x06007763 RID: 30563 RVA: 0x0024447C File Offset: 0x0024267C
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

		// Token: 0x04004E9C RID: 20124
		private ModuleBase cachedSeaIceAllowedNoise;

		// Token: 0x04004E9D RID: 20125
		private int cachedSeaIceAllowedNoiseForSeed;
	}
}
