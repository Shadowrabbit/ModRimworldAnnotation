using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02001776 RID: 6006
	public class TileTemperaturesComp : WorldComponent
	{
		// Token: 0x06008A7E RID: 35454 RVA: 0x0031B728 File Offset: 0x00319928
		public TileTemperaturesComp(World world) : base(world)
		{
			this.ClearCaches();
		}

		// Token: 0x06008A7F RID: 35455 RVA: 0x0031B738 File Offset: 0x00319938
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

		// Token: 0x06008A80 RID: 35456 RVA: 0x0031B7B4 File Offset: 0x003199B4
		public float GetOutdoorTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetOutdoorTemp();
		}

		// Token: 0x06008A81 RID: 35457 RVA: 0x0031B7C2 File Offset: 0x003199C2
		public float GetSeasonalTemp(int tile)
		{
			return this.RetrieveCachedData(tile).GetSeasonalTemp();
		}

		// Token: 0x06008A82 RID: 35458 RVA: 0x0031B7D0 File Offset: 0x003199D0
		public float OutdoorTemperatureAt(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OutdoorTemperatureAt(absTick);
		}

		// Token: 0x06008A83 RID: 35459 RVA: 0x0031B7DF File Offset: 0x003199DF
		public float OffsetFromDailyRandomVariation(int tile, int absTick)
		{
			return this.RetrieveCachedData(tile).OffsetFromDailyRandomVariation(absTick);
		}

		// Token: 0x06008A84 RID: 35460 RVA: 0x0031B7EE File Offset: 0x003199EE
		public float AverageTemperatureForTwelfth(int tile, Twelfth twelfth)
		{
			return this.RetrieveCachedData(tile).AverageTemperatureForTwelfth(twelfth);
		}

		// Token: 0x06008A85 RID: 35461 RVA: 0x0031B800 File Offset: 0x00319A00
		public bool SeasonAcceptableFor(int tile, ThingDef animalRace)
		{
			float seasonalTemp = this.GetSeasonalTemp(tile);
			return seasonalTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && seasonalTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06008A86 RID: 35462 RVA: 0x0031B838 File Offset: 0x00319A38
		public bool OutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			float outdoorTemp = this.GetOutdoorTemp(tile);
			return outdoorTemp > animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) && outdoorTemp < animalRace.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
		}

		// Token: 0x06008A87 RID: 35463 RVA: 0x0031B86D File Offset: 0x00319A6D
		public bool SeasonAndOutdoorTemperatureAcceptableFor(int tile, ThingDef animalRace)
		{
			return this.SeasonAcceptableFor(tile, animalRace) && this.OutdoorTemperatureAcceptableFor(tile, animalRace);
		}

		// Token: 0x06008A88 RID: 35464 RVA: 0x0031B883 File Offset: 0x00319A83
		public void ClearCaches()
		{
			this.cache = new TileTemperaturesComp.CachedTileTemperatureData[Find.WorldGrid.TilesCount];
			this.usedSlots = new List<int>();
		}

		// Token: 0x06008A89 RID: 35465 RVA: 0x0031B8A5 File Offset: 0x00319AA5
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

		// Token: 0x04005829 RID: 22569
		private TileTemperaturesComp.CachedTileTemperatureData[] cache;

		// Token: 0x0400582A RID: 22570
		private List<int> usedSlots;

		// Token: 0x020029BC RID: 10684
		private class CachedTileTemperatureData
		{
			// Token: 0x0600E2C3 RID: 58051 RVA: 0x00427874 File Offset: 0x00425A74
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

			// Token: 0x0600E2C4 RID: 58052 RVA: 0x00427914 File Offset: 0x00425B14
			public float GetOutdoorTemp()
			{
				return this.cachedOutdoorTemp;
			}

			// Token: 0x0600E2C5 RID: 58053 RVA: 0x0042791C File Offset: 0x00425B1C
			public float GetSeasonalTemp()
			{
				return this.cachedSeasonalTemp;
			}

			// Token: 0x0600E2C6 RID: 58054 RVA: 0x00427924 File Offset: 0x00425B24
			public float OutdoorTemperatureAt(int absTick)
			{
				return this.CalculateOutdoorTemperatureAtTile(absTick, true);
			}

			// Token: 0x0600E2C7 RID: 58055 RVA: 0x0042792E File Offset: 0x00425B2E
			public float OffsetFromDailyRandomVariation(int absTick)
			{
				return (float)this.dailyVariationPerlinCached.GetValue((double)absTick, 0.0, 0.0) * 7f;
			}

			// Token: 0x0600E2C8 RID: 58056 RVA: 0x00427956 File Offset: 0x00425B56
			public float AverageTemperatureForTwelfth(Twelfth twelfth)
			{
				return this.twelfthlyTempAverages[(int)twelfth];
			}

			// Token: 0x0600E2C9 RID: 58057 RVA: 0x00427960 File Offset: 0x00425B60
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

			// Token: 0x0600E2CA RID: 58058 RVA: 0x004279EC File Offset: 0x00425BEC
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

			// Token: 0x04009CF9 RID: 40185
			private int tile;

			// Token: 0x04009CFA RID: 40186
			private int tickCachesNeedReset = int.MinValue;

			// Token: 0x04009CFB RID: 40187
			private float cachedOutdoorTemp = float.MinValue;

			// Token: 0x04009CFC RID: 40188
			private float cachedSeasonalTemp = float.MinValue;

			// Token: 0x04009CFD RID: 40189
			private float[] twelfthlyTempAverages;

			// Token: 0x04009CFE RID: 40190
			private Perlin dailyVariationPerlinCached;

			// Token: 0x04009CFF RID: 40191
			private const int CachedTempUpdateInterval = 60;
		}
	}
}
