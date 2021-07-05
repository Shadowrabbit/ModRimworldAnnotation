using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D89 RID: 3465
	public static class AnimalProductionUtility
	{
		// Token: 0x06005051 RID: 20561 RVA: 0x001ADC02 File Offset: 0x001ABE02
		public static IEnumerable<StatDrawEntry> AnimalProductionStats(ThingDef d)
		{
			float num = AnimalProductionUtility.GestationDaysLitter(d);
			if (num > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GestationTime".Translate(), "PeriodDays".Translate(num.ToString("#.##")), "Stat_Animal_GestationTimeDesc".Translate(), 10000, null, null, false);
			}
			IntRange lhs = AnimalProductionUtility.OffspringRange(d);
			if (lhs != IntRange.one)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_LitterSize".Translate(), lhs.ToString(), "Stat_Animal_LitterSizeDesc".Translate(), 9990, null, null, false);
			}
			float num2 = AnimalProductionUtility.YearsToAdulthood(d);
			if (num2 > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrowthTime".Translate(), "PeriodYears".Translate(num2), "Stat_Animal_GrowthTimeDesc".Translate(), 9980, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MeatPerDayDuringGrowth".Translate(), AnimalProductionUtility.MeatPerDayDuringGrowth(d).ToString("#.##"), "Stat_Animal_MeatPerDayDuringGrowthDesc".Translate(), 9960, null, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_AdultMeatAmount".Translate(), AnimalProductionUtility.AdultMeatAmount(d).ToString("F0"), "Stat_Animal_AdultMeatAmountDesc".Translate(), 9970, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(d.race.meatDef, -1)), false);
			if (AnimalProductionUtility.Herbivore(d))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_GrassToMaintain".Translate(), AnimalProductionUtility.GrassToMaintain(d).ToString("#.##"), "Stat_Animal_GrassToMaintainDesc".Translate(), 9950, null, null, false);
			}
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties != null)
			{
				ThingDef thingDef = compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef;
				if (thingDef != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggType".Translate(), thingDef.LabelCap, "Stat_Animal_EggTypeDesc".Translate(), 9940, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(thingDef, -1)), false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggsPerYear".Translate(), AnimalProductionUtility.EggsPerYear(d).ToString(), "Stat_Animal_EggsPerYearDesc".Translate(), 9930, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutrition".Translate(), AnimalProductionUtility.EggNutrition(d).ToString(), "Stat_Animal_EggNutritionDesc".Translate(), 9920, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggNutritionYearly".Translate(), AnimalProductionUtility.EggNutritionPerYear(d).ToString(), "Stat_Animal_EggNutritionYearlyDesc".Translate(), 9910, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValue".Translate(), AnimalProductionUtility.EggMarketValue(d).ToStringMoney(null), "Stat_Animal_EggMarketValueDesc".Translate(), 9900, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_EggMarketValueYearly".Translate(), AnimalProductionUtility.EggMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_EggMarketValueYearlyDesc".Translate(), 9890, null, null, false);
			}
			CompProperties_Milkable milkable = d.GetCompProperties<CompProperties_Milkable>();
			if (milkable != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkType".Translate(), milkable.milkDef.LabelCap, "Stat_Animal_MilkTypeDesc".Translate(), 9880, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(milkable.milkDef, -1)), false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkAmount".Translate(), milkable.milkAmount.ToString(), "Stat_Animal_MilkAmountDesc".Translate(), 9870, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkGrowthTime".Translate(), "PeriodDays".Translate(milkable.milkIntervalDays), "Stat_Animal_MilkGrowthTimeDesc".Translate(), 9860, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkPerYear".Translate(), AnimalProductionUtility.MilkPerYear(d).ToString("F0"), "Stat_Animal_MilkPerYearDesc".Translate(), 9850, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValue".Translate(), AnimalProductionUtility.MilkMarketValue(d).ToStringMoney(null), "Stat_Animal_MilkValueDesc".Translate(), 9840, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_MilkValuePerYear".Translate(), AnimalProductionUtility.MilkMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_MilkValuePerYearDesc".Translate(), 9830, null, null, false);
			}
			CompProperties_Shearable shearable = d.GetCompProperties<CompProperties_Shearable>();
			if (shearable != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolType".Translate(), shearable.woolDef.LabelCap, "Stat_Animal_WoolTypeDesc".Translate(), 9820, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(shearable.woolDef, -1)), false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolAmount".Translate(), shearable.woolAmount.ToString(), "Stat_Animal_WoolAmountDesc".Translate(), 9810, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolGrowthTime".Translate(), "PeriodDays".Translate(shearable.shearIntervalDays), "Stat_Animal_WoolGrowthTimeDesc".Translate(), 9800, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolPerYear".Translate(), AnimalProductionUtility.WoolPerYear(d).ToString("F0"), "Stat_Animal_WoolPerYearDesc".Translate(), 9790, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValue".Translate(), AnimalProductionUtility.WoolMarketValue(d).ToStringMoney(null), "Stat_Animal_WoolValueDesc".Translate(), 9780, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.AnimalProductivity, "Stat_Animal_WoolValuePerYear".Translate(), AnimalProductionUtility.WoolMarketValuePerYear(d).ToStringMoney(null), "Stat_Animal_WoolValuePerYearDesc".Translate(), 9770, null, null, false);
			}
			yield break;
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x001ADC12 File Offset: 0x001ABE12
		public static float AdultMeatAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null);
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x001ADC20 File Offset: 0x001ABE20
		public static float AdultLeatherAmount(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.LeatherAmount, null);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x001ADC2E File Offset: 0x001ABE2E
		public static bool Herbivore(ThingDef d)
		{
			return (d.race.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x001ADC44 File Offset: 0x001ABE44
		public static float NutritionToGestate(ThingDef d)
		{
			float num = 0f;
			LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
			return num + AnimalProductionUtility.GestationDaysEach(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001ADC98 File Offset: 0x001ABE98
		public static float NutritionToAdulthood(ThingDef d)
		{
			float num = 0f;
			num += AnimalProductionUtility.NutritionToGestate(d);
			for (int i = 1; i < d.race.lifeStageAges.Count; i++)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
				float num2 = (lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge) * 60f;
				num += num2 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			}
			return num;
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x001ADD24 File Offset: 0x001ABF24
		public static float GestationDaysEach(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			return d.race.gestationPeriodDays / ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f);
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x001ADD8D File Offset: 0x001ABF8D
		public static float GestationDaysLitter(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				return d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays;
			}
			return d.race.gestationPeriodDays;
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001ADDB8 File Offset: 0x001ABFB8
		public static IntRange OffspringRange(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties != null)
			{
				return compProperties.eggCountRange;
			}
			if (d.race.litterSizeCurve != null)
			{
				int min = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.First<CurvePoint>().x), 1);
				int max = Mathf.Max(Mathf.RoundToInt(d.race.litterSizeCurve.Last<CurvePoint>().x), 1);
				return new IntRange(min, max);
			}
			return new IntRange(1, 1);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001ADE38 File Offset: 0x001AC038
		public static float GrassNutritionPerDay()
		{
			ThingDef plant_Grass = ThingDefOf.Plant_Grass;
			return plant_Grass.GetStatValueAbstract(StatDefOf.Nutrition, null) / (plant_Grass.plant.growDays / 0.5f);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001ADE69 File Offset: 0x001AC069
		public static float GrassToMaintain(ThingDef d)
		{
			if (!AnimalProductionUtility.Herbivore(d))
			{
				return -1f;
			}
			return 2.6666667E-05f * d.race.baseHungerRate * 60000f / AnimalProductionUtility.GrassNutritionPerDay();
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001ADE98 File Offset: 0x001AC098
		public static float EggsPerYear(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / compProperties.eggLayIntervalDays * compProperties.eggCountRange.Average;
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x001ADECD File Offset: 0x001AC0CD
		public static float EggNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggNutrition(d);
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001ADEDC File Offset: 0x001AC0DC
		public static float EggNutrition(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001ADF14 File Offset: 0x001AC114
		public static float EggMarketValue(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return -1f;
			}
			return (compProperties.eggUnfertilizedDef ?? compProperties.eggFertilizedDef).BaseMarketValue;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001ADF46 File Offset: 0x001AC146
		public static float EggMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.EggsPerYear(d) * AnimalProductionUtility.EggMarketValue(d);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001ADF55 File Offset: 0x001AC155
		public static float YearsToAdulthood(ThingDef d)
		{
			return d.race.lifeStageAges.Last<LifeStageAge>().minAge;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x001ADF6C File Offset: 0x001AC16C
		public static float MeatPerDayDuringGrowth(ThingDef d)
		{
			float num = AnimalProductionUtility.AdultMeatAmount(d);
			float num2 = AnimalProductionUtility.YearsToAdulthood(d) * 60f;
			return num / num2;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001ADF90 File Offset: 0x001AC190
		public static float MilkPerYear(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.milkIntervalDays * (float)compProperties.milkAmount;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001ADFC4 File Offset: 0x001AC1C4
		public static float MilkMarketValue(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return -1f;
			}
			return compProperties.milkDef.BaseMarketValue;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001ADFEC File Offset: 0x001AC1EC
		public static float MilkMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkMarketValue(d);
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001ADFFC File Offset: 0x001AC1FC
		public static float MilkNutrition(ThingDef d)
		{
			CompProperties_Milkable compProperties = d.GetCompProperties<CompProperties_Milkable>();
			if (compProperties == null)
			{
				return 0f;
			}
			if (!compProperties.milkDef.IsIngestible)
			{
				return 0f;
			}
			return compProperties.milkDef.ingestible.CachedNutrition;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001AE03C File Offset: 0x001AC23C
		public static float MilkNutritionPerYear(ThingDef d)
		{
			return AnimalProductionUtility.MilkPerYear(d) * AnimalProductionUtility.MilkNutrition(d);
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001AE04C File Offset: 0x001AC24C
		public static float WoolPerYear(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return 60f / (float)compProperties.shearIntervalDays * (float)compProperties.woolAmount;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001AE080 File Offset: 0x001AC280
		public static float WoolMarketValue(ThingDef d)
		{
			CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
			if (compProperties == null)
			{
				return 0f;
			}
			return compProperties.woolDef.BaseMarketValue;
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001AE0A8 File Offset: 0x001AC2A8
		public static float WoolMarketValuePerYear(ThingDef d)
		{
			return AnimalProductionUtility.WoolPerYear(d) * AnimalProductionUtility.WoolMarketValue(d);
		}
	}
}
