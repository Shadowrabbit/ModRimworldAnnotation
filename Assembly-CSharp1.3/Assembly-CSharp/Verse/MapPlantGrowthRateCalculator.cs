using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017A RID: 378
	public class MapPlantGrowthRateCalculator
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0003A0FF File Offset: 0x000382FF
		public List<TerrainDef> TerrainDefs
		{
			get
			{
				this.ComputeIfDirty();
				return this.terrainDefs;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0003A10D File Offset: 0x0003830D
		public List<ThingDef> WildGrazingPlants
		{
			get
			{
				this.ComputeIfDirty();
				return this.wildGrazingPlants;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0003A11B File Offset: 0x0003831B
		public List<ThingDef> GrazingAnimals
		{
			get
			{
				this.ComputeIfDirty();
				return this.includeAnimalTypes;
			}
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0003A129 File Offset: 0x00038329
		public void BuildFor(int tile)
		{
			this.tile = tile;
			this.longLat = Find.WorldGrid.LongLatOf(tile);
			this.biome = Find.WorldGrid[tile].biome;
			this.ComputeIfDirty();
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0003A160 File Offset: 0x00038360
		public float GrowthRateForDay(int nowTicks, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			int index = nowTicks / 60000 % 60;
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.dailyGrowthRates[index]);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0003A191 File Offset: 0x00038391
		public float QuadrumGrowthRateFor(Quadrum quadrum, ThingDef plantDef, TerrainDef terrainDef)
		{
			this.ComputeIfDirty();
			return MapPlantGrowthRateCalculator.ComputeGrowthRate(plantDef, terrainDef, this.seasonalGrowthRates[quadrum]);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0003A1AC File Offset: 0x000383AC
		private static float ComputeGrowthRate(ThingDef plantDef, TerrainDef terrainDef, MapPlantGrowthRateCalculator.PlantGrowthRates rates)
		{
			if (terrainDef.fertility < plantDef.plant.fertilityMin)
			{
				return 0f;
			}
			MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator = rates.For(plantDef);
			return PlantUtility.GrowthRateFactorFor_Fertility(plantDef, terrainDef.fertility) * growthRateAccumulator.GrowthRateForTemperature * growthRateAccumulator.GrowthRateForGlow;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0003A1F4 File Offset: 0x000383F4
		private void ComputeIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.includeAnimalTypes.Clear();
			this.seasonalGrowthRates.Clear();
			this.dailyGrowthRates.Clear();
			this.seasonalGrowthRates.Add(Quadrum.Aprimay, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Decembary, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Jugust, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.seasonalGrowthRates.Add(Quadrum.Septober, new MapPlantGrowthRateCalculator.PlantGrowthRates());
			this.AddIncludedAnimals();
			this.terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
			this.wildGrazingPlants = (from plantDef in this.biome.AllWildPlants
			where MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(plantDef)
			select plantDef).ToList<ThingDef>();
			this.CalculateDailyFertility();
			this.CalculateSeasonalFertility();
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0003A2D0 File Offset: 0x000384D0
		private void CalculateDailyFertility()
		{
			for (int i = 0; i < 60; i++)
			{
				int num = i * 60000;
				int nowTicks = num - num % 60000;
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = new MapPlantGrowthRateCalculator.PlantGrowthRates();
				this.dailyGrowthRates.Add(plantGrowthRates);
				foreach (ThingDef plantDef in this.wildGrazingPlants)
				{
					this.SimulateGrowthRateForDay(nowTicks, plantGrowthRates.For(plantDef));
				}
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0003A35C File Offset: 0x0003855C
		private void CalculateSeasonalFertility()
		{
			for (int i = 0; i < this.dailyGrowthRates.Count; i++)
			{
				Quadrum key = GenDate.Quadrum((long)(i * 60000), this.longLat.x);
				MapPlantGrowthRateCalculator.PlantGrowthRates plantGrowthRates = this.seasonalGrowthRates[key];
				foreach (KeyValuePair<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> keyValuePair in this.dailyGrowthRates[i].byPlant)
				{
					plantGrowthRates.For(keyValuePair.Value.plantDef).Accumulate(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0003A414 File Offset: 0x00038614
		private void AddIncludedAnimals()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (MapPlantGrowthRateCalculator.IsPastureAnimal(thingDef))
				{
					this.includeAnimalTypes.Add(thingDef);
				}
			}
			this.includeAnimalTypes.Sort((ThingDef a, ThingDef b) => string.CompareOrdinal(a.label, b.label));
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0003A498 File Offset: 0x00038698
		public static bool IsPastureAnimal(ThingDef td)
		{
			return td.race != null && td.race.Animal && td.race.Roamer;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0003A4BC File Offset: 0x000386BC
		public static bool IsEdibleByPastureAnimals(ThingDef foodDef)
		{
			return foodDef.ingestible != null && foodDef.ingestible.preferability != FoodPreferability.Undefined && (FoodTypeFlags.VegetarianRoughAnimal & foodDef.ingestible.foodType) > FoodTypeFlags.None;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0003A4EC File Offset: 0x000386EC
		private void SimulateGrowthRateForDay(int nowTicks, MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRates)
		{
			int num = nowTicks - nowTicks % 60000;
			int num2 = 24;
			int num3 = 60000 / num2;
			for (int i = 0; i < num2; i++)
			{
				int num4 = num + i * num3;
				float cellTemp = Find.World.tileTemperatures.OutdoorTemperatureAt(this.tile, num4);
				float glow = GenCelestial.CelestialSunGlow(this.tile, num4);
				float grTemp = PlantUtility.GrowthRateFactorFor_Temperature(cellTemp);
				float grGlow = PlantUtility.GrowthRateFactorFor_Light(growthRates.plantDef, glow);
				growthRates.Accumulate(grTemp, grGlow);
			}
		}

		// Token: 0x040008F9 RID: 2297
		private Vector2 longLat;

		// Token: 0x040008FA RID: 2298
		private int tile;

		// Token: 0x040008FB RID: 2299
		private BiomeDef biome;

		// Token: 0x040008FC RID: 2300
		private bool dirty = true;

		// Token: 0x040008FD RID: 2301
		private readonly List<ThingDef> includeAnimalTypes = new List<ThingDef>();

		// Token: 0x040008FE RID: 2302
		private List<TerrainDef> terrainDefs;

		// Token: 0x040008FF RID: 2303
		private List<ThingDef> wildGrazingPlants;

		// Token: 0x04000900 RID: 2304
		private readonly Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates> seasonalGrowthRates = new Dictionary<Quadrum, MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x04000901 RID: 2305
		private readonly List<MapPlantGrowthRateCalculator.PlantGrowthRates> dailyGrowthRates = new List<MapPlantGrowthRateCalculator.PlantGrowthRates>();

		// Token: 0x02001947 RID: 6471
		private class GrowthRateAccumulator
		{
			// Token: 0x060097D0 RID: 38864 RVA: 0x0035D909 File Offset: 0x0035BB09
			public GrowthRateAccumulator(ThingDef plantDef)
			{
				this.plantDef = plantDef;
			}

			// Token: 0x17001910 RID: 6416
			// (get) Token: 0x060097D1 RID: 38865 RVA: 0x0035D918 File Offset: 0x0035BB18
			public float GrowthRateForTemperature
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForTemperature / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x17001911 RID: 6417
			// (get) Token: 0x060097D2 RID: 38866 RVA: 0x0035D936 File Offset: 0x0035BB36
			public float GrowthRateForGlow
			{
				get
				{
					if (this.numSamples != 0)
					{
						return this.sumGrowthRateForGlow / (float)this.numSamples;
					}
					return 0f;
				}
			}

			// Token: 0x060097D3 RID: 38867 RVA: 0x0035D954 File Offset: 0x0035BB54
			public void Accumulate(float grTemp, float grGlow)
			{
				this.sumGrowthRateForTemperature += grTemp;
				this.sumGrowthRateForGlow += grGlow;
				this.numSamples++;
			}

			// Token: 0x060097D4 RID: 38868 RVA: 0x0035D980 File Offset: 0x0035BB80
			public void Accumulate(MapPlantGrowthRateCalculator.GrowthRateAccumulator other)
			{
				this.sumGrowthRateForTemperature += other.sumGrowthRateForTemperature;
				this.sumGrowthRateForGlow += other.sumGrowthRateForGlow;
				this.numSamples += other.numSamples;
			}

			// Token: 0x04006108 RID: 24840
			public readonly ThingDef plantDef;

			// Token: 0x04006109 RID: 24841
			private float sumGrowthRateForTemperature;

			// Token: 0x0400610A RID: 24842
			private float sumGrowthRateForGlow;

			// Token: 0x0400610B RID: 24843
			private int numSamples;
		}

		// Token: 0x02001948 RID: 6472
		private class PlantGrowthRates
		{
			// Token: 0x060097D5 RID: 38869 RVA: 0x0035D9BC File Offset: 0x0035BBBC
			public MapPlantGrowthRateCalculator.GrowthRateAccumulator For(ThingDef plantDef)
			{
				MapPlantGrowthRateCalculator.GrowthRateAccumulator growthRateAccumulator;
				if (!this.byPlant.TryGetValue(plantDef, out growthRateAccumulator))
				{
					growthRateAccumulator = new MapPlantGrowthRateCalculator.GrowthRateAccumulator(plantDef);
					this.byPlant.Add(plantDef, growthRateAccumulator);
				}
				return growthRateAccumulator;
			}

			// Token: 0x0400610C RID: 24844
			public Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator> byPlant = new Dictionary<ThingDef, MapPlantGrowthRateCalculator.GrowthRateAccumulator>();
		}
	}
}
