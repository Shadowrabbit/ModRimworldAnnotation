using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x0200207B RID: 8315
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x0600B040 RID: 45120 RVA: 0x0007290C File Offset: 0x00070B0C
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x0600B041 RID: 45121 RVA: 0x003331BC File Offset: 0x003313BC
		public override void WorldComponentTick()
		{
			for (int i = 0; i < this.usedSlots.Count; i++)
			{
				this.cache[this.usedSlots[i]].CheckCache();
			}
			if (Find.TickManager.TicksGame % 300 == 84 && this.usedSlots.Any<int>())
			{
				this.cache[this.usedSlots[0]] = null;
				this.usedSlots.RemoveAt(0);
			}
		}

		// Token: 0x0600B042 RID: 45122 RVA: 0x0007291B File Offset: 0x00070B1B
		public float GetOutdoorTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x0600B043 RID: 45123 RVA: 0x00072929 File Offset: 0x00070B29
		public float GetSeasonalTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x0600B044 RID: 45124 RVA: 0x00072937 File Offset: 0x00070B37
		public float OutdoorTemperatureAt(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x0600B045 RID: 45125 RVA: 0x00072946 File Offset: 0x00070B46
		public float OffsetFromDailyRandomVariation(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x0600B046 RID: 45126 RVA: 0x00072955 File Offset: 0x00070B55
		public float AverageTemperatureForTwelfth(int tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x0600B047 RID: 45127 RVA: 0x00333238 File Offset: 0x00331438
		public bool SeasonAcceptableFor(int tile, ThingDef animalRace)
		{
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x0600B048 RID: 45128 RVA: 0x00333270 File Offset: 0x00331470
		public bool OutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x0600B049 RID: 45129 RVA: 0x00072964 File Offset: 0x00070B64
		public bool SeasonAndOutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x0600B04A RID: 45130 RVA: 0x0007297A File Offset: 0x00070B7A
		public void ClearCaches()
		{
			this.cache = new TileTemperaturesComp.CachedTileTemperatureData[Find.WorldGrid.TilesCount];
			this.usedSlots = new List<int>();
		}

		// Token: 0x0600B04B RID: 45131 RVA: 0x0007299C File Offset: 0x00070B9C
		private TileTemperaturesComp.CachedTileTemperatureData RetrieveCachedData(int tile)
		{
			if (this.cache[tile] != null)
			{
				return this.cache[tile];
			}
			this.cache[tile] = new TileTemperaturesComp.CachedTileTemperatureData(tile);
			this.usedSlots.Add(tile);
			return this.cache[tile];
		}

		// Token: 0x0400794E RID: 31054
		private TileTemperaturesComp.CachedTileTemperatureData[] cache;

		// Token: 0x0400794F RID: 31055
		private List<int> usedSlots;

		// Token: 0x0200207C RID: 8316
		private class CachedTileTemperatureData
		{
			// Token: 0x0600B04C RID: 45132 RVA: 0x003332A8 File Offset: 0x003314A8
			public CachedTileTemperatureData(int tile)
			{
				this.tile = tile;
				int seed = Gen.HashCombineInt(tile, 199372327);
				this.dailyVariationPerlinCached = new Perlin(4.999999873689376E-06, 2.0, 0.5, 3, seed, QualityMode.Medium);
				this.twelfthlyTempAverages = new float[12];
				for (int i = 0; i < 12; i++)
				{
					this.twelfthlyTempAverages[i] = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, (Twelfth)i);
				}
				this.CheckCache();
			}

			// Token: 0x0600B04D RID: 45133 RVA: 0x000729D3 File Offset: 0x00070BD3
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x0600B04E RID: 45134 RVA: 0x000729DB File Offset: 0x00070BDB
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x0600B04F RID: 45135 RVA: 0x000729E3 File Offset: 0x00070BE3
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x0600B050 RID: 45136 RVA: 0x000729ED File Offset: 0x00070BED
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x0600B051 RID: 45137 RVA: 0x00072A15 File Offset: 0x00070C15
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x0600B052 RID: 45138 RVA: 0x00333348 File Offset: 0x00331548
			public void CheckCache()
			{
				if (this.tickCachesNeedReset <= Find.TickManager.TicksGame)
				{
					this.tickCachesNeedReset = Find.TickManager.TicksGame + 60;
					Map map = Current.Game.FindMap(this.tile);
					this.cachedOutdoorTemp = this.OutdoorTemperatureAt(Find.TickManager.TicksAbs);
					if (map != null)
					{
						this.cachedOutdoorTemp += map.gameConditionManager.AggregateTemperatureOffset();
					}
					this.cachedSeasonalTemp = this.CalculateOutdoorTemperatureAtTile(Find.TickManager.TicksAbs, false);
				}
			}

			// Token: 0x0600B053 RID: 45139 RVA: 0x003333D4 File Offset: 0x003315D4
			private float CalculateOutdoorTemperatureAtTile(int absTick, bool includeDailyVariations)
			{
				if (absTick == 0)
				{
					absTick = 1;
				}
				float num = Find.WorldGrid[this.tile].temperature + GenTemperature.OffsetFromSeasonCycle(absTick, this.tile);
				if (includeDailyVariations)
				{
					num += this.OffsetFromDailyRandomVariation(absTick) + GenTemperature.OffsetFromSunCycle(absTick, this.tile);
				}
				return num;
			}

			// Token: 0x04007950 RID: 31056
			private int tile;

			// Token: 0x04007951 RID: 31057
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x04007952 RID: 31058
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x04007953 RID: 31059
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x04007954 RID: 31060
			private float[] twelfthlyTempAverages;

			// Token: 0x04007955 RID: 31061
			private Perlin dailyVariationPerlinCached;

			// Token: 0x04007956 RID: 31062
			private const int CachedTempUpdateInterval = 60;
		}
	}
}
