using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0E RID: 3342
	public class WildPlantSpawner : IExposable
	{
		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x001A31DD File Offset: 0x001A13DD
		public float CurrentPlantDensity
		{
			get
			{
				return this.map.Biome.plantDensity * this.map.gameConditionManager.AggregatePlantDensityFactor(this.map);
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06004E1B RID: 19995 RVA: 0x001A3208 File Offset: 0x001A1408
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

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x001A327C File Offset: 0x001A147C
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

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06004E1D RID: 19997 RVA: 0x001A32F0 File Offset: 0x001A14F0
		public float CachedChanceFromDensity
		{
			get
			{
				this.CacheWholeMapNumDesiredPlants();
				return this.calculatedWholeMapNumDesiredPlants / (float)this.calculatedWholeMapNumNonZeroFertilityCells;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06004E1E RID: 19998 RVA: 0x001A3308 File Offset: 0x001A1508
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

		// Token: 0x06004E1F RID: 19999 RVA: 0x001A3393 File Offset: 0x001A1593
		public WildPlantSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x001A33A2 File Offset: 0x001A15A2
		public static void ResetStaticData()
		{
			WildPlantSpawner.allCavePlants.Clear();
			WildPlantSpawner.allCavePlants.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant && x.plant.cavePlantWeight > 0f
			select x);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x001A33E4 File Offset: 0x001A15E4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.cycleIndex, "cycleIndex", 0, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlants, "calculatedWholeMapNumDesiredPlants", 0f, false);
			Scribe_Values.Look<float>(ref this.calculatedWholeMapNumDesiredPlantsTmp, "calculatedWholeMapNumDesiredPlantsTmp", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasWholeMapNumDesiredPlantsCalculated, "hasWholeMapNumDesiredPlantsCalculated", true, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCells, "calculatedWholeMapNumNonZeroFertilityCells", 0, false);
			Scribe_Values.Look<int>(ref this.calculatedWholeMapNumNonZeroFertilityCellsTmp, "calculatedWholeMapNumNonZeroFertilityCellsTmp", 0, false);
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x001A3468 File Offset: 0x001A1668
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

		// Token: 0x06004E23 RID: 20003 RVA: 0x001A34A0 File Offset: 0x001A16A0
		private void WildPlantSpawnerTickInternal()
		{
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.0001f);
			float currentPlantDensity = this.CurrentPlantDensity;
			this.CacheWholeMapNumDesiredPlants();
			int num2 = Mathf.CeilToInt(10000f);
			float cachedChanceFromDensity = this.CachedChanceFromDensity;
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
				if (Rand.Chance(cachedChanceFromDensity) && Rand.MTBEventOccurs(mtb, 60000f, (float)num2) && this.CanRegrowAt(intVec))
				{
					this.CheckSpawnWildPlantAt(intVec, currentPlantDensity, this.calculatedWholeMapNumDesiredPlants, false);
				}
				this.cycleIndex++;
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x001A35F3 File Offset: 0x001A17F3
		private void CacheWholeMapNumDesiredPlants()
		{
			if (!this.hasWholeMapNumDesiredPlantsCalculated)
			{
				this.calculatedWholeMapNumDesiredPlants = this.CurrentWholeMapNumDesiredPlants;
				this.calculatedWholeMapNumNonZeroFertilityCells = this.CurrentWholeMapNumNonZeroFertilityCells;
				this.hasWholeMapNumDesiredPlantsCalculated = true;
			}
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001A361C File Offset: 0x001A181C
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
				plant.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
				if (plant.def.plant.LimitedLifespan)
				{
					plant.Age = Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 50, 0));
				}
			}
			GenSpawn.Spawn(plant, c, this.map, WipeMode.Vanish);
			return true;
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x001A37B8 File Offset: 0x001A19B8
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

		// Token: 0x06004E27 RID: 20007 RVA: 0x001A39EC File Offset: 0x001A1BEC
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

		// Token: 0x06004E28 RID: 20008 RVA: 0x001A3AC0 File Offset: 0x001A1CC0
		private void CalculatePlantsWhichCanGrowAt(IntVec3 c, List<ThingDef> outPlants, bool cavePlants, float plantDensity)
		{
			outPlants.Clear();
			if (cavePlants)
			{
				for (int i = 0; i < WildPlantSpawner.allCavePlants.Count; i++)
				{
					if (WildPlantSpawner.allCavePlants[i].CanEverPlantAt(c, this.map, false))
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
				if (thingDef.CanEverPlantAt(c, this.map, false))
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

		// Token: 0x06004E29 RID: 20009 RVA: 0x001A3BB8 File Offset: 0x001A1DB8
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

		// Token: 0x06004E2A RID: 20010 RVA: 0x001A3CC8 File Offset: 0x001A1EC8
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

		// Token: 0x06004E2B RID: 20011 RVA: 0x001A3D9C File Offset: 0x001A1F9C
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

		// Token: 0x06004E2C RID: 20012 RVA: 0x001A3F3C File Offset: 0x001A213C
		private bool CanRegrowAt(IntVec3 c)
		{
			return c.GetTemperature(this.map) > 0f && (!c.Roofed(this.map) || this.GoodRoofForCavePlant(c));
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x001A3F6C File Offset: 0x001A216C
		private bool GoodRoofForCavePlant(IntVec3 c)
		{
			RoofDef roof = c.GetRoof(this.map);
			return roof != null && roof.isNatural;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x001A3F91 File Offset: 0x001A2191
		private float GetCommonalityOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityOfPlant(plant);
			}
			return plant.plant.cavePlantWeight;
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x001A3FBD File Offset: 0x001A21BD
		private float GetCommonalityPctOfPlant(ThingDef plant)
		{
			if (!plant.plant.cavePlant)
			{
				return this.map.Biome.CommonalityPctOfPlant(plant);
			}
			return this.GetCommonalityOfPlant(plant) / this.CavePlantsCommonalitiesSum;
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x001A3FEC File Offset: 0x001A21EC
		public float GetBaseDesiredPlantsCountAt(IntVec3 c)
		{
			float num = c.GetTerrain(this.map).fertility;
			if (this.GoodRoofForCavePlant(c))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x001A401D File Offset: 0x001A221D
		public float GetDesiredPlantsCountAt(IntVec3 c, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(this.GetBaseDesiredPlantsCountAt(c) * plantDensity * forCell.GetTerrain(this.map).fertility, 1f);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x001A4044 File Offset: 0x001A2244
		public float GetDesiredPlantsCountIn(Region reg, IntVec3 forCell, float plantDensity)
		{
			return Mathf.Min(reg.GetBaseDesiredPlantsCount(true) * plantDensity * forCell.GetTerrain(this.map).fertility, (float)reg.CellCount);
		}

		// Token: 0x04002F21 RID: 12065
		private Map map;

		// Token: 0x04002F22 RID: 12066
		private int cycleIndex;

		// Token: 0x04002F23 RID: 12067
		private float calculatedWholeMapNumDesiredPlants;

		// Token: 0x04002F24 RID: 12068
		private float calculatedWholeMapNumDesiredPlantsTmp;

		// Token: 0x04002F25 RID: 12069
		private int calculatedWholeMapNumNonZeroFertilityCells;

		// Token: 0x04002F26 RID: 12070
		private int calculatedWholeMapNumNonZeroFertilityCellsTmp;

		// Token: 0x04002F27 RID: 12071
		private bool hasWholeMapNumDesiredPlantsCalculated;

		// Token: 0x04002F28 RID: 12072
		private float? cachedCavePlantsCommonalitiesSum;

		// Token: 0x04002F29 RID: 12073
		private static List<ThingDef> allCavePlants = new List<ThingDef>();

		// Token: 0x04002F2A RID: 12074
		private static List<ThingDef> tmpPossiblePlants = new List<ThingDef>();

		// Token: 0x04002F2B RID: 12075
		private static List<KeyValuePair<ThingDef, float>> tmpPossiblePlantsWithWeight = new List<KeyValuePair<ThingDef, float>>();

		// Token: 0x04002F2C RID: 12076
		private static Dictionary<ThingDef, float> distanceSqToNearbyClusters = new Dictionary<ThingDef, float>();

		// Token: 0x04002F2D RID: 12077
		private static Dictionary<ThingDef, List<float>> nearbyClusters = new Dictionary<ThingDef, List<float>>();

		// Token: 0x04002F2E RID: 12078
		private static List<KeyValuePair<ThingDef, List<float>>> nearbyClustersList = new List<KeyValuePair<ThingDef, List<float>>>();

		// Token: 0x04002F2F RID: 12079
		private static readonly FloatRange InitialGrowthRandomRange = new FloatRange(0.15f, 1.5f);

		// Token: 0x04002F30 RID: 12080
		private const float CavePlantsDensityFactor = 0.5f;

		// Token: 0x04002F31 RID: 12081
		private const int PlantSaturationScanRadius = 20;

		// Token: 0x04002F32 RID: 12082
		private const float MapFractionCheckPerTick = 0.0001f;

		// Token: 0x04002F33 RID: 12083
		private const float ChanceToRegrow = 0.012f;

		// Token: 0x04002F34 RID: 12084
		private const float CavePlantChanceToRegrow = 0.0001f;

		// Token: 0x04002F35 RID: 12085
		private const float BaseLowerOrderScanRadius = 7f;

		// Token: 0x04002F36 RID: 12086
		private const float LowerOrderScanRadiusWildClusterRadiusFactor = 1.5f;

		// Token: 0x04002F37 RID: 12087
		private const float MinDesiredLowerOrderPlantsToConsiderSkipping = 4f;

		// Token: 0x04002F38 RID: 12088
		private const float MinLowerOrderPlantsPct = 0.57f;

		// Token: 0x04002F39 RID: 12089
		private const float LocalPlantProportionsMaxScanRadius = 25f;

		// Token: 0x04002F3A RID: 12090
		private const float MaxLocalProportionsBias = 7f;

		// Token: 0x04002F3B RID: 12091
		private const float CavePlantRegrowDays = 130f;

		// Token: 0x04002F3C RID: 12092
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

		// Token: 0x04002F3D RID: 12093
		private static List<ThingDef> tmpPlantDefsLowerOrder = new List<ThingDef>();
	}
}
