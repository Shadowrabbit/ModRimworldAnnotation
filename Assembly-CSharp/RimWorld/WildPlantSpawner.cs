using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001346 RID: 4934
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06006B08 RID: 27400 RVA: 0x00048D9C File Offset: 0x00046F9C
		public float CurrentPlantDensity
		{
			get
			{
				return this.map.Biome.plantDensity * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x06006B09 RID: 27401 RVA: 0x00210824 File Offset: 0x0020EA24
		public float CurrentWholeMapNumDesiredPlants
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				float currentPlantDensity = this.CurrentPlantDensity;
				float num = 0f;
				foreach (IntVec3 intVec in cellRect)
				{
					num += this.GetDesiredPlantsCountAt(intVec, intVec, currentPlantDensity);
				}
				return num;
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06006B0A RID: 27402 RVA: 0x00210898 File Offset: 0x0020EA98
		public int CurrentWholeMapNumNonZeroFertilityCells
		{
			get
			{
				CellRect cellRect = CellRect.WholeMap(this.map);
				int num = 0;
				using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetTerrain(this.map).fertility > 0f)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06006B0B RID: 27403 RVA: 0x0021090C File Offset: 0x0020EB0C
		public float CavePlantsCommonalitiesSum
		{
			get
			{
				if (this.cachedCavePlantsCommonalitiesSum == null)
				{
					this.cachedCavePlantsCommonalitiesSum = new float?(0f);
					for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
					{
						this.cachedCavePlantsCommonalitiesSum += this.GetCommonalityOfPlant(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return this.cachedCavePlantsCommonalitiesSum.Value;
			}
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x00048DC5 File Offset: 0x00046FC5
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006B0D RID: 27405 RVA: 0x00048DD4 File Offset: 0x00046FD4
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant
			select x);
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x00210998 File Offset: 0x0020EB98
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x00210A1C File Offset: 0x0020EC1C
		public void WildPlantSpawnerTick()
		{
			if (DebugSettings.fastEcology || DebugSettings.fastEcologyRegrowRateOnly)
			{
				for (int i = 0; i < 2000; i++)
				{
					this.WildPlantSpawnerTickInternal();
				}
				return;
			}
			this.WildPlantSpawnerTickInternal();
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x00210A54 File Offset: 0x0020EC54
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensity = this.CurrentPlantDensity;
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
			int num2 = Mathf.CeilToInt(10000f);
			float chance = this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			for (int i = 0; i < num; i++)
			{
				if (this.cycleIndex >= area)
				{
					this.calculatedWholeMapNumDesiredPlants = this.calculatedWholeMapNumDesiredPlantsTmp;
					this.calculatedWholeMapNumDesiredPlantsTmp = 0f;
					this.calculatedWholeMapNumNonZeroFertilityCells = this.calculatedWholeMapNumNonZeroFertilityCellsTmp;
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp = 0;
					this.cycleIndex = 0;
				}
				IntVec3 intVec = this.map.cellsInRandomOrder.Get(this.cycleIndex);
				this.calculatedWholeMapNumDesiredPlantsTmp += this.GetDesiredPlantsCountAt(intVec, intVec, currentPlantDensity);
				if (intVec.GetTerrain(this.map).fertility > 0f)
				{
					this.calculatedWholeMapNumNonZeroFertilityCellsTmp++;
				}
				float mtb = this.GoodRoofForCavePlant(intVec) ? 130f : this.map.Biome.wildPlantRegrowDays;
				if (Rand.Chance(chance) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensity, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x00210BD0 File Offset: 0x0020EDD0
		public bool CheckSpawnWildPlantAt(IntVec3 c, float plantDensity, float wholeMapNumDesiredPlants, bool setRandomGrowth = false)
		{
			if (plantDensity <= 0f || c.GetPlant(this.map) != null || c.GetCover(this.map) != null || c.GetEdifice(this.map) != null || this.map.fertilityGrid.FertilityAt(c) <= 0f || !PlantUtility.SnowAllowsPlanting(c, this.map))
			{
				return false;
			}
			bool cavePlants = this.GoodRoofForCavePlant(c);
			if (this.SaturatedAt(c, plantDensity, cavePlants, wholeMapNumDesiredPlants))
			{
				return false;
			}
			this.CalculatePlantsWhichCanGrowAt(c, WildPlantSpawner.tmpPossiblePlants, cavePlants, plantDensity);
			if (!WildPlantSpawner.tmpPossiblePlants.Any<ThingDef>())
			{
				return false;
			}
			this.CalculateDistancesToNearbyClusters(c);
			WildPlantSpawner.tmpPossiblePlantsWithWeight.Clear();
			for (int i = 0; i < WildPlantSpawner.tmpPossiblePlants.Count; i++)
			{
				float value = this.PlantChoiceWeight(WildPlantSpawner.tmpPossiblePlants[i], c, WildPlantSpawner.distanceSqToNearbyClusters, wholeMapNumDesiredPlants, plantDensity);
				WildPlantSpawner.tmpPossiblePlantsWithWeight.Add(new KeyValuePair<ThingDef, float>(WildPlantSpawner.tmpPossiblePlants[i], value));
			}
			KeyValuePair<ThingDef, float> keyValuePair;
			if (!WildPlantSpawner.tmpPossiblePlantsWithWeight.TryRandomElementByWeight((KeyValuePair<ThingDef, float> x) => x.Value, out keyValuePair))
			{
				return false;
			}
			Plant plant = (Plant)ThingMaker.MakeThing(keyValuePair.Key, null);
			if (setRandomGrowth)
			{
				plant.Growth = Rand.Range(0.07f, 1f);
				if (plant.def.plant.LimitedLifespan)
				{
					plant.Age = Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 50, 0));
				}
			}
			GenSpawn.Spawn(plant, c, this.map, WipeMode.Vanish);
			return true;
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x00210D68 File Offset: 0x0020EF68
		private float PlantChoiceWeight(ThingDef plantDef, IntVec3 c, Dictionary<ThingDef, float> distanceSqToNearbyClusters, float wholeMapNumDesiredPlants, float plantDensity)
		{
			float commonalityOfPlant = this.GetCommonalityOfPlant(plantDef);
			float commonalityPctOfPlant = this.GetCommonalityPctOfPlant(plantDef);
			float num = commonalityOfPlant;
			if (num <= 0f)
			{
				return num;
			}
			float num2 = 0.5f;
			if ((float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count > wholeMapNumDesiredPlants / 2f && !plantDef.plant.cavePlant)
			{
				num2 = (float)this.map.listerThings.ThingsOfDef(plantDef).Count / (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count / commonalityPctOfPlant;
				num *= WildPlantSpawner.GlobalPctSelectionWeightBias.Evaluate(num2);
			}
			if (plantDef.plant.GrowsInClusters && num2 < 1.1f)
			{
				float num3 = plantDef.plant.cavePlant ? this.CavePlantsCommonalitiesSum : this.map.Biome.PlantCommonalitiesSum;
				float x = commonalityOfPlant * plantDef.plant.wildClusterWeight / (num3 - commonalityOfPlant + commonalityOfPlant * plantDef.plant.wildClusterWeight);
				float num4 = 1f / (3.1415927f * (float)plantDef.plant.wildClusterRadius * (float)plantDef.plant.wildClusterRadius);
				num4 = GenMath.LerpDoubleClamped(commonalityPctOfPlant, 1f, 1f, num4, x);
				float f;
				if (distanceSqToNearbyClusters.TryGetValue(plantDef, out f))
				{
					float x2 = Mathf.Sqrt(f);
					num *= GenMath.LerpDoubleClamped((float)plantDef.plant.wildClusterRadius * 0.9f, (float)plantDef.plant.wildClusterRadius * 1.1f, plantDef.plant.wildClusterWeight, num4, x2);
				}
				else
				{
					num *= num4;
				}
			}
			if (plantDef.plant.wildEqualLocalDistribution)
			{
				float f2 = wholeMapNumDesiredPlants * commonalityPctOfPlant;
				float num5 = (float)Mathf.Max(this.map.Size.x, this.map.Size.z) / Mathf.Sqrt(f2) * 2f;
				if (plantDef.plant.GrowsInClusters)
				{
					num5 = Mathf.Max(num5, (float)plantDef.plant.wildClusterRadius * 1.6f);
				}
				num5 = Mathf.Max(num5, 7f);
				if (num5 <= 25f)
				{
					num *= this.LocalPlantProportionsWeightFactor(c, commonalityPctOfPlant, plantDensity, num5, plantDef);
				}
			}
			return num;
		}

		// Token: 0x06006B13 RID: 27411 RVA: 0x00210F9C File Offset: 0x0020F19C
		private float LocalPlantProportionsWeightFactor(IntVec3 c, float commonalityPct, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			int numPlantsThisDef = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
				numPlantsThisDef += reg.ListerThings.ThingsOfDef(plantDef).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			if (numDesiredPlantsLocally * commonalityPct < 2f)
			{
				return 1f;
			}
			if ((float)numPlants <= numDesiredPlantsLocally * 0.5f)
			{
				return 1f;
			}
			float t = (float)numPlantsThisDef / (float)numPlants / commonalityPct;
			return Mathf.Lerp(7f, 1f, t);
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x00211070 File Offset: 0x0020F270
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensity)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt_NewTemp(c, this.map, false))
					{
						outPlants.Add(WildPlantSpawner.allCavePlants[i]);
					}
				}
				return;
			}
			List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
			for (int j = 0; j < allWildPlants.Count; j++)
			{
				ThingDef thingDef = allWildPlants[j];
				if (thingDef.CanEverPlantAt_NewTemp(c, this.map, false))
				{
					if (thingDef.plant.wildOrder != this.map.Biome.LowestWildAndCavePlantOrder)
					{
						float num = 7f;
						if (thingDef.plant.GrowsInClusters)
						{
							num = Math.Max(num, (float)thingDef.plant.wildClusterRadius * 1.5f);
						}
						if (!this.EnoughLowerOrderPlantsNearby(c, plantDensity, num, thingDef))
						{
							goto IL_D8;
						}
					}
					outPlants.Add(thingDef);
				}
				IL_D8:;
			}
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x00211168 File Offset: 0x0020F368
		private bool EnoughLowerOrderPlantsNearby(IntVec3 c, float plantDensity, float radiusToScan, ThingDef plantDef)
		{
			float num = 0f;
			WildPlantSpawner.tmpPlantDefsLowerOrder.Clear();
			List<ThingDef> allWildPlants = this.map.Biome.AllWildPlants;
			for (int i = 0; i < allWildPlants.Count; i++)
			{
				if (allWildPlants[i].plant.wildOrder < plantDef.plant.wildOrder)
				{
					num += this.GetCommonalityPctOfPlant(allWildPlants[i]);
					WildPlantSpawner.tmpPlantDefsLowerOrder.Add(allWildPlants[i]);
				}
			}
			float numDesiredPlantsLocally = 0f;
			int numPlantsLowerOrder = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), radiusToScan), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				for (int j = 0; j < WildPlantSpawner.tmpPlantDefsLowerOrder.Count; j++)
				{
					numPlantsLowerOrder += reg.ListerThings.ThingsOfDef(WildPlantSpawner.tmpPlantDefsLowerOrder[j]).Count;
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			float num2 = numDesiredPlantsLocally * num;
			return num2 < 4f || (float)numPlantsLowerOrder / num2 >= 0.57f;
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x00211278 File Offset: 0x0020F478
		private bool SaturatedAt(IntVec3 c, float plantDensity, bool cavePlants, float wholeMapNumDesiredPlants)
		{
			int num = GenRadial.NumCellsInRadius(20f);
			if (wholeMapNumDesiredPlants * ((float)num / (float)this.map.Area) <= 4f || !this.map.Biome.wildPlantsCareAboutLocalFertility)
			{
				return (float)this.map.listerThings.ThingsInGroup(ThingRequestGroup.Plant).Count >= wholeMapNumDesiredPlants;
			}
			float numDesiredPlantsLocally = 0f;
			int numPlants = 0;
			RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => c.InHorDistOf(to.extentsClose.ClosestCellTo(c), 20f), delegate(Region reg)
			{
				numDesiredPlantsLocally += this.GetDesiredPlantsCountIn(reg, c, plantDensity);
				numPlants += reg.ListerThings.ThingsInGroup(ThingRequestGroup.Plant).Count;
				return false;
			}, 999999, RegionType.Set_Passable);
			return (float)numPlants >= numDesiredPlantsLocally;
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x0021134C File Offset: 0x0020F54C
		private void CalculateDistancesToNearbyClusters(IntVec3 c)
		{
			WildPlantSpawner.nearbyClusters.Clear();
			WildPlantSpawner.nearbyClustersList.Clear();
			int num = GenRadial.NumCellsInRadius((float)(this.map.Biome.MaxWildAndCavePlantsClusterRadius * 2));
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = c + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map))
				{
					List<Thing> list = this.map.thingGrid.ThingsListAtFast(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.category == ThingCategory.Plant && thing.def.plant.GrowsInClusters)
						{
							float item = (float)intVec.DistanceToSquared(c);
							List<float> list2;
							if (!WildPlantSpawner.nearbyClusters.TryGetValue(thing.def, out list2))
							{
								list2 = SimplePool<List<float>>.Get();
								WildPlantSpawner.nearbyClusters.Add(thing.def, list2);
								WildPlantSpawner.nearbyClustersList.Add(new KeyValuePair<ThingDef, List<float>>(thing.def, list2));
							}
							list2.Add(item);
						}
					}
				}
			}
			WildPlantSpawner.distanceSqToNearbyClusters.Clear();
			for (int k = 0; k < WildPlantSpawner.nearbyClustersList.Count; k++)
			{
				List<float> value = WildPlantSpawner.nearbyClustersList[k].Value;
				value.Sort();
				WildPlantSpawner.distanceSqToNearbyClusters.Add(WildPlantSpawner.nearbyClustersList[k].Key, value[value.Count / 2]);
				value.Clear();
				SimplePool<List<float>>.Return(value);
			}
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x00048E13 File Offset: 0x00047013
		private bool CanRegrowAt(IntVec3 c)
		{
			return c.GetTemperature(this.map) > 0f && (!c.Roofed(this.map) || this.GoodRoofForCavePlant(c));
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x002114EC File Offset: 0x0020F6EC
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x00048E41 File Offset: 0x00047041
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityOfPlant(plant);
			}
			return plant.plant.cavePlantWeight;
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x00048E6D File Offset: 0x0004706D
		private float GetCommonalityPctOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityPctOfPlant(plant);
			}
			return this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum;
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x00211514 File Offset: 0x0020F714
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = c.GetTerrain(this.map).fertility;
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x00048E9C File Offset: 0x0004709C
		public float GetDesiredPlantsCountAt(IntVec3 c, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(c) * plantDensity * forCell.GetTerrain(this.map).fertility, 1f);
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x00048EC3 File Offset: 0x000470C3
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * plantDensity * forCell.GetTerrain(this.map).fertility, (float)reg.CellCount);
		}

		// Token: 0x04004731 RID: 18225
		private Map map;

		// Token: 0x04004732 RID: 18226
		private int cycleIndex;

		// Token: 0x04004733 RID: 18227
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x04004734 RID: 18228
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x04004735 RID: 18229
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x04004736 RID: 18230
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x04004737 RID: 18231
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x04004738 RID: 18232
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x04004739 RID: 18233
		private static List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x0400473A RID: 18234
		private static List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x0400473B RID: 18235
		private static List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x0400473C RID: 18236
		private static Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x0400473D RID: 18237
		private static Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x0400473E RID: 18238
		private static List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x0400473F RID: 18239
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x04004740 RID: 18240
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x04004741 RID: 18241
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x04004742 RID: 18242
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x04004743 RID: 18243
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x04004744 RID: 18244
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x04004745 RID: 18245
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x04004746 RID: 18246
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x04004747 RID: 18247
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x04004748 RID: 18248
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x04004749 RID: 18249
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x0400474A RID: 18250
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x0400474B RID: 18251
		private static readonly SimpleCurve GlobalPctSelectionWeightBias = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.5f, 0.25f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			}
		};

		// Token: 0x0400474C RID: 18252
		private static List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();
	}
}
