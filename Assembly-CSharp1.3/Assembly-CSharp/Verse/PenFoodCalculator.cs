using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017C RID: 380
	public class PenFoodCalculator
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0003A672 File Offset: 0x00038872
		public float NutritionPerDayToday
		{
			get
			{
				return this.sumNutritionPerDayToday;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0003A67A File Offset: 0x0003887A
		public List<PenFoodCalculator.PenAnimalInfo> ActualAnimalInfos
		{
			get
			{
				return this.animals;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0003A682 File Offset: 0x00038882
		public List<PenFoodCalculator.PenFoodItemInfo> AllStockpiledInfos
		{
			get
			{
				return this.stockpiled;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0003A68A File Offset: 0x0003888A
		public bool Unenclosed
		{
			get
			{
				return this.numCells == 0;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003A698 File Offset: 0x00038898
		public float SumNutritionConsumptionPerDay
		{
			get
			{
				float num = 0f;
				foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
				{
					num += penAnimalInfo.TotalNutritionConsumptionPerDay;
				}
				return num;
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0003A6F4 File Offset: 0x000388F4
		public PenFoodCalculator.PenAnimalInfo GetAnimalInfo(ThingDef animalDef)
		{
			foreach (PenFoodCalculator.PenAnimalInfo penAnimalInfo in this.animals)
			{
				if (penAnimalInfo.animalDef == animalDef)
				{
					return penAnimalInfo;
				}
			}
			PenFoodCalculator.PenAnimalInfo penAnimalInfo2 = new PenFoodCalculator.PenAnimalInfo(animalDef);
			this.animals.Add(penAnimalInfo2);
			return penAnimalInfo2;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0003A764 File Offset: 0x00038964
		public PenFoodCalculator.PenFoodItemInfo GetStockpiledInfo(ThingDef itemDef)
		{
			foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
			{
				if (penFoodItemInfo.itemDef == itemDef)
				{
					return penFoodItemInfo;
				}
			}
			PenFoodCalculator.PenFoodItemInfo penFoodItemInfo2 = new PenFoodCalculator.PenFoodItemInfo(itemDef);
			this.stockpiled.Add(penFoodItemInfo2);
			return penFoodItemInfo2;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0003A7D4 File Offset: 0x000389D4
		public string NaturalGrowthRateTooltip()
		{
			if (this.cachedNaturalGrowthRateTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateDescription".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("PenFoodTab_NaturalNutritionGrowthRateSeasonal".Translate());
				stringBuilder.AppendLine();
				stringBuilder.Append("PenFoodTab_GrowthPerSeason".Translate()).AppendLine(":");
				Vector2 vector = Find.WorldGrid.LongLatOf(this.mapCalc.tile);
				for (int i = 0; i < 4; i++)
				{
					Quadrum quadrum = (Quadrum)i;
					stringBuilder.Append("- ").Append(quadrum.Label()).Append(" (").Append(quadrum.GetSeason(vector.y).Label()).Append("): ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionPerDayToString(this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum), false));
				}
				this.cachedNaturalGrowthRateTooltip = stringBuilder.ToString();
			}
			return this.cachedNaturalGrowthRateTooltip;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0003A8E0 File Offset: 0x00038AE0
		public string TotalConsumedToolTop()
		{
			if (this.cachedTotalConsumedTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_NutritionConsumptionDescription".Translate());
				this.cachedTotalConsumedTooltip = stringBuilder.ToString();
			}
			return this.cachedTotalConsumedTooltip;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0003A924 File Offset: 0x00038B24
		public string StockpileToolTip()
		{
			if (this.cachedStockpileTooltip == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("PenFoodTab_StockpileTotalDescription".Translate());
				stringBuilder.AppendLine();
				foreach (PenFoodCalculator.PenFoodItemInfo penFoodItemInfo in this.stockpiled)
				{
					stringBuilder.Append("- ").Append(penFoodItemInfo.itemDef.LabelCap).Append(" x").Append(penFoodItemInfo.totalCount).Append(": ");
					stringBuilder.AppendLine(PenFoodCalculator.NutritionToString(penFoodItemInfo.totalNutritionAvailable, true));
				}
				this.cachedStockpileTooltip = stringBuilder.ToString();
			}
			return this.cachedStockpileTooltip;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0003AA04 File Offset: 0x00038C04
		private void Reset(Map map)
		{
			this.mapCalc.Reset(map);
			this.animals.Clear();
			this.stockpiled.Clear();
			this.cachedNaturalGrowthRateTooltip = null;
			this.cachedTotalConsumedTooltip = null;
			this.cachedStockpileTooltip = null;
			this.nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();
			this.sumNutritionPerDayToday = 0f;
			this.sumStockpiledNutritionAvailableNow = 0f;
			this.numCells = 0;
			this.numCellsSoil = 0;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0003AA78 File Offset: 0x00038C78
		public List<PenFoodCalculator.PenAnimalInfo> ComputeExampleAnimals(List<ThingDef> animalDefs)
		{
			this.tmpAddedExampleAnimals.Clear();
			foreach (ThingDef thingDef in animalDefs)
			{
				LifeStageAge lifeStageAge = thingDef.race.lifeStageAges.Last<LifeStageAge>();
				PenFoodCalculator.PenAnimalInfo penAnimalInfo = new PenFoodCalculator.PenAnimalInfo(thingDef);
				penAnimalInfo.example = true;
				penAnimalInfo.nutritionConsumptionPerDay = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(thingDef, lifeStageAge.def);
				this.tmpAddedExampleAnimals.Add(penAnimalInfo);
			}
			PenFoodCalculator.SortAnimals(this.tmpAddedExampleAnimals);
			return this.tmpAddedExampleAnimals;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0003AB18 File Offset: 0x00038D18
		public void ResetAndProcessPen(CompAnimalPenMarker marker)
		{
			this.ResetAndProcessPen(marker.parent.Position, marker.parent.Map, false);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003AB37 File Offset: 0x00038D37
		public void ResetAndProcessPen(IntVec3 position, Map map, bool considerBlueprints)
		{
			this.Reset(map);
			if (considerBlueprints)
			{
				this.ProcessBlueprintPen(position, map);
			}
			else
			{
				this.ProcessRealPen(position, map);
			}
			this.SortResults(map);
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0003AB5C File Offset: 0x00038D5C
		private void SortResults(Map map)
		{
			this.stockpiled.Sort(new Comparison<PenFoodCalculator.PenFoodItemInfo>(PenFoodCalculator.<>c.<>9.<SortResults>g__FoodSorter|36_0));
			PenFoodCalculator.SortAnimals(this.animals);
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003AB84 File Offset: 0x00038D84
		private static void SortAnimals(List<PenFoodCalculator.PenAnimalInfo> infos)
		{
			infos.Sort(new Comparison<PenFoodCalculator.PenAnimalInfo>(PenFoodCalculator.<>c.<>9.<SortAnimals>g__AnimalSorter|37_0));
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0003AB9C File Offset: 0x00038D9C
		private void ProcessBlueprintPen(IntVec3 markerPos, Map map)
		{
			this.blueprintEnclosureCalc.VisitPen(markerPos, map);
			if (!this.blueprintEnclosureCalc.isEnclosed)
			{
				return;
			}
			foreach (IntVec3 c in this.blueprintEnclosureCalc.cellsFound)
			{
				this.ProcessCell(c, map);
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0003AC10 File Offset: 0x00038E10
		private void ProcessRealPen(IntVec3 markerPos, Map map)
		{
			foreach (District district in this.connectedDistrictsCalc.CalculateConnectedDistricts(markerPos, map))
			{
				foreach (Region region in district.Regions)
				{
					this.ProcessRegion(region);
				}
			}
			this.connectedDistrictsCalc.Reset();
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0003ACB0 File Offset: 0x00038EB0
		private void ProcessRegion(Region region)
		{
			foreach (IntVec3 c in region.Cells)
			{
				this.ProcessCell(c, region.Map);
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0003AD04 File Offset: 0x00038F04
		private void ProcessCell(IntVec3 c, Map map)
		{
			this.ProcessTerrain(c, map);
			foreach (Thing thing in c.GetThingList(map))
			{
				Pawn animal;
				if ((animal = (thing as Pawn)) != null && thing.def.race.Animal)
				{
					this.ProcessAnimal(animal);
				}
				else if (thing.def.category == ThingCategory.Item && thing.IngestibleNow && MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thing.def))
				{
					this.ProcessStockpiledFood(thing);
				}
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0003ADA8 File Offset: 0x00038FA8
		private void ProcessTerrain(IntVec3 c, Map map)
		{
			this.numCells++;
			if (c.GetEdifice(map) != null)
			{
				return;
			}
			TerrainDef terrain = c.GetTerrain(map);
			if (terrain.IsSoil)
			{
				this.numCellsSoil++;
			}
			MapPastureNutritionCalculator.NutritionPerDayPerQuadrum other = this.mapCalc.CalculateAverageNutritionPerDay(terrain);
			this.nutritionPerDayPerQuadrum.AddFrom(other);
			this.sumNutritionPerDayToday += this.mapCalc.GetAverageNutritionPerDayToday(terrain);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003AE20 File Offset: 0x00039020
		private void ProcessStockpiledFood(Thing thing)
		{
			PenFoodCalculator.PenFoodItemInfo stockpiledInfo = this.GetStockpiledInfo(thing.def);
			float num = thing.GetStatValue(StatDefOf.Nutrition, true) * (float)thing.stackCount;
			stockpiledInfo.totalCount += thing.stackCount;
			stockpiledInfo.totalNutritionAvailable += num;
			this.sumStockpiledNutritionAvailableNow += num;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0003AE7C File Offset: 0x0003907C
		private void ProcessAnimal(Pawn animal)
		{
			if (!MapPlantGrowthRateCalculator.IsPastureAnimal(animal.def) || !animal.Spawned)
			{
				return;
			}
			PenFoodCalculator.PenAnimalInfo animalInfo = this.GetAnimalInfo(animal.def);
			animalInfo.count++;
			animalInfo.nutritionConsumptionPerDay += SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0003AECC File Offset: 0x000390CC
		public static string NutritionToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_Nutrition_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0003AF08 File Offset: 0x00039108
		public static string NutritionPerDayToString(float value, bool withUnits = true)
		{
			string text = value.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
			if (withUnits)
			{
				return text + " " + "PenFoodTab_NutritionPerDay_Unit".Translate();
			}
			return text;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0003AF42 File Offset: 0x00039142
		public float CapacityOf(Quadrum q, ThingDef animal)
		{
			return this.nutritionPerDayPerQuadrum.ForQuadrum(q) / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003AF58 File Offset: 0x00039158
		public Quadrum GetSummerOrBestQuadrum()
		{
			Vector2 location = Find.WorldGrid.LongLatOf(this.mapCalc.tile);
			Quadrum? quadrum = null;
			float num = 0f;
			foreach (Quadrum quadrum2 in QuadrumUtility.Quadrums)
			{
				if (quadrum2.GetSeason(location) == Season.Summer)
				{
					return quadrum2;
				}
				float num2 = this.nutritionPerDayPerQuadrum.ForQuadrum(quadrum2);
				if (quadrum == null || num2 > num)
				{
					quadrum = new Quadrum?(quadrum2);
					num = num2;
				}
			}
			return quadrum.Value;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0003B00C File Offset: 0x0003920C
		public string PenSizeDescription()
		{
			string result;
			if (this.Unenclosed)
			{
				result = "PenSizeDesc_Unenclosed".Translate();
			}
			else if (this.numCellsSoil < 50)
			{
				result = "PenSizeDesc_VerySmall".Translate();
			}
			else if (this.numCellsSoil < 100)
			{
				result = "PenSizeDesc_Small".Translate();
			}
			else if (this.numCellsSoil < 400)
			{
				result = "PenSizeDesc_Medium".Translate();
			}
			else
			{
				result = "PenSizeDesc_Large".Translate();
			}
			return result;
		}

		// Token: 0x04000903 RID: 2307
		public const ToStringStyle NutritionStringStyle = ToStringStyle.FloatMaxTwo;

		// Token: 0x04000904 RID: 2308
		private AnimalPenConnectedDistrictsCalculator connectedDistrictsCalc = new AnimalPenConnectedDistrictsCalculator();

		// Token: 0x04000905 RID: 2309
		private AnimalPenBlueprintEnclosureCalculator blueprintEnclosureCalc = new AnimalPenBlueprintEnclosureCalculator();

		// Token: 0x04000906 RID: 2310
		private MapPastureNutritionCalculator mapCalc = new MapPastureNutritionCalculator();

		// Token: 0x04000907 RID: 2311
		private List<PenFoodCalculator.PenAnimalInfo> animals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x04000908 RID: 2312
		private List<PenFoodCalculator.PenFoodItemInfo> stockpiled = new List<PenFoodCalculator.PenFoodItemInfo>();

		// Token: 0x04000909 RID: 2313
		private string cachedNaturalGrowthRateTooltip;

		// Token: 0x0400090A RID: 2314
		private string cachedTotalConsumedTooltip;

		// Token: 0x0400090B RID: 2315
		private string cachedStockpileTooltip;

		// Token: 0x0400090C RID: 2316
		public MapPastureNutritionCalculator.NutritionPerDayPerQuadrum nutritionPerDayPerQuadrum = new MapPastureNutritionCalculator.NutritionPerDayPerQuadrum();

		// Token: 0x0400090D RID: 2317
		public float sumStockpiledNutritionAvailableNow;

		// Token: 0x0400090E RID: 2318
		public int numCells;

		// Token: 0x0400090F RID: 2319
		public int numCellsSoil;

		// Token: 0x04000910 RID: 2320
		private float sumNutritionPerDayToday;

		// Token: 0x04000911 RID: 2321
		private List<PenFoodCalculator.PenAnimalInfo> tmpAddedExampleAnimals = new List<PenFoodCalculator.PenAnimalInfo>();

		// Token: 0x0200194A RID: 6474
		public class PenAnimalInfo
		{
			// Token: 0x17001912 RID: 6418
			// (get) Token: 0x060097DB RID: 38875 RVA: 0x0035DA28 File Offset: 0x0035BC28
			public int TotalCount
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x17001913 RID: 6419
			// (get) Token: 0x060097DC RID: 38876 RVA: 0x0035DA30 File Offset: 0x0035BC30
			public float TotalNutritionConsumptionPerDay
			{
				get
				{
					return this.nutritionConsumptionPerDay;
				}
			}

			// Token: 0x060097DD RID: 38877 RVA: 0x0035DA38 File Offset: 0x0035BC38
			public PenAnimalInfo(ThingDef animalDef)
			{
				this.animalDef = animalDef;
			}

			// Token: 0x060097DE RID: 38878 RVA: 0x0035DA48 File Offset: 0x0035BC48
			public string ToolTip(PenFoodCalculator calc)
			{
				if (this.cachedToolTip == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					int value = Mathf.FloorToInt(calc.NutritionPerDayToday / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef));
					stringBuilder.Append("PenFoodTab_AnimalTypeAnimalCapacity".Translate()).Append(": ").Append(value).AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("PenFoodTab_NutritionConsumedPerDay".Translate(this.animalDef.Named("ANIMALDEF"))).AppendLine(":");
					List<LifeStageAge> lifeStageAges = this.animalDef.race.lifeStageAges;
					for (int i = 0; i < lifeStageAges.Count; i++)
					{
						LifeStageDef def = lifeStageAges[i].def;
						float value2 = SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(this.animalDef, def);
						stringBuilder.Append("- ").Append(def.LabelCap).Append(": ").AppendLine(PenFoodCalculator.NutritionPerDayToString(value2, false));
					}
					this.cachedToolTip = stringBuilder.ToString();
				}
				return this.cachedToolTip;
			}

			// Token: 0x04006110 RID: 24848
			public ThingDef animalDef;

			// Token: 0x04006111 RID: 24849
			public bool example;

			// Token: 0x04006112 RID: 24850
			public int count;

			// Token: 0x04006113 RID: 24851
			public float nutritionConsumptionPerDay;

			// Token: 0x04006114 RID: 24852
			private string cachedToolTip;
		}

		// Token: 0x0200194B RID: 6475
		public class PenFoodItemInfo
		{
			// Token: 0x060097DF RID: 38879 RVA: 0x0035DB65 File Offset: 0x0035BD65
			public PenFoodItemInfo(ThingDef itemDef)
			{
				this.itemDef = itemDef;
			}

			// Token: 0x04006115 RID: 24853
			public ThingDef itemDef;

			// Token: 0x04006116 RID: 24854
			public int totalCount;

			// Token: 0x04006117 RID: 24855
			public float totalNutritionAvailable;
		}
	}
}
