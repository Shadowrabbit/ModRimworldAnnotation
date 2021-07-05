using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000179 RID: 377
	public class MapPastureNutritionCalculator
	{
		// Token: 0x06000A8E RID: 2702 RVA: 0x00039DE8 File Offset: 0x00037FE8
		public void Reset(Map map)
		{
			this.Reset(map.Tile, map.wildPlantSpawner.CachedChanceFromDensity, map.plantGrowthRateCalculator);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00039E08 File Offset: 0x00038008
		public void Reset(int tile, float newMapChanceRegrowth, MapPlantGrowthRateCalculator growthRateCalculator)
		{
			newMapChanceRegrowth = (float)Math.Round((double)newMapChanceRegrowth, 7);
			if (this.tile == tile && Mathf.Approximately(this.mapChanceRegrowth, newMapChanceRegrowth))
			{
				return;
			}
			this.tile = tile;
			this.biome = Find.WorldGrid[tile].biome;
			this.mapChanceRegrowth = newMapChanceRegrowth;
			this.plantGrowthRateCalculator = growthRateCalculator;
			this.cachedSeasonalDetailed.Clear();
			this.cachedSeasonalByTerrain.Clear();
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00039E7C File Offset: 0x0003807C
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(TerrainDef terrain)
		{
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!this.cachedSeasonalByTerrain.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
				foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
				{
					MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.CalculateAverageNutritionPerDay(plantDef, terrain);
					nutritionPerDayPerQuadrum.AddFrom(other);
				}
				this.cachedSeasonalByTerrain.Add(terrain, nutritionPerDayPerQuadrum);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00039F04 File Offset: 0x00038104
		private MapPastureNutritionCalculator.NutritionPerDayPerQuadrum CalculateAverageNutritionPerDay(ThingDef plantDef, TerrainDef terrain)
		{
			Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> dictionary;
			if (!this.cachedSeasonalDetailed.TryGetValue(plantDef, out dictionary))
			{
				dictionary = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();
				this.cachedSeasonalDetailed.Add(plantDef, dictionary);
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum;
			if (!dictionary.TryGetValue(terrain, out nutritionPerDayPerQuadrum))
			{
				nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
				dictionary.Add(terrain, nutritionPerDayPerQuadrum);
				nutritionPerDayPerQuadrum.quadrum[0] = this.GetAverageNutritionPerDay(Quadrum.Aprimay, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[3] = this.GetAverageNutritionPerDay(Quadrum.Decembary, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[1] = this.GetAverageNutritionPerDay(Quadrum.Jugust, plantDef, terrain);
				nutritionPerDayPerQuadrum.quadrum[2] = this.GetAverageNutritionPerDay(Quadrum.Septober, plantDef, terrain);
			}
			return nutritionPerDayPerQuadrum;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00039F94 File Offset: 0x00038194
		public float GetAverageNutritionPerDayToday(TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDayToday(plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00039FF8 File Offset: 0x000381F8
		private float GetAverageNutritionPerDayToday(ThingDef plantDef, TerrainDef terrainDef)
		{
			if (terrainDef.fertility <= 0f)
			{
				return 0f;
			}
			int ticksAbs = Find.TickManager.TicksAbs;
			int nowTicks = ticksAbs - ticksAbs % 60000;
			float growthRate = this.plantGrowthRateCalculator.GrowthRateForDay(nowTicks, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0003A044 File Offset: 0x00038244
		public float GetAverageNutritionPerDay(Quadrum quadrum, TerrainDef terrainDef)
		{
			float num = 0f;
			foreach (ThingDef plantDef in this.plantGrowthRateCalculator.WildGrazingPlants)
			{
				num += this.GetAverageNutritionPerDay(quadrum, plantDef, terrainDef);
			}
			return num;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0003A0A8 File Offset: 0x000382A8
		public float GetAverageNutritionPerDay(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			float growthRate = this.plantGrowthRateCalculator.QuadrumGrowthRateFor(quadrum, plantDef, terrainDef);
			return this.ComputeNutritionProducedPerDay(plantDef, growthRate);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0003A0CC File Offset: 0x000382CC
		private float ComputeNutritionProducedPerDay(ThingDef plantDef, float growthRate)
		{
			return SimplifiedPastureNutritionSimulator.NutritionProducedPerDay(this.biome, plantDef, growthRate, this.mapChanceRegrowth);
		}

		// Token: 0x040008F3 RID: 2291
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x040008F4 RID: 2292
		public BiomeDef biome;

		// Token: 0x040008F5 RID: 2293
		public int tile;

		// Token: 0x040008F6 RID: 2294
		public float mapChanceRegrowth;

		// Token: 0x040008F7 RID: 2295
		private Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>> cachedSeasonalDetailed = new Dictionary<ThingDef, Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>>();

		// Token: 0x040008F8 RID: 2296
		private Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum> cachedSeasonalByTerrain = new Dictionary<TerrainDef, MapPastureNutritionCalculator.NutritionPerDayPerQuadrum>();

		// Token: 0x02001946 RID: 6470
		public class NutritionPerDayPerQuadrum
		{
			// Token: 0x060097CD RID: 38861 RVA: 0x0035D87C File Offset: 0x0035BA7C
			public float ForQuadrum(Quadrum q)
			{
				return this.quadrum[(int)q];
			}

			// Token: 0x060097CE RID: 38862 RVA: 0x0035D888 File Offset: 0x0035BA88
			public void AddFrom(MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other)
			{
				this.quadrum[0] += other.quadrum[0];
				this.quadrum[3] += other.quadrum[3];
				this.quadrum[1] += other.quadrum[1];
				this.quadrum[2] += other.quadrum[2];
			}

			// Token: 0x04006107 RID: 24839
			public float[] quadrum = new float[4];
		}
	}
}
