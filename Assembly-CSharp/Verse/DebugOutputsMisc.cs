using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064D RID: 1613
	public static class DebugOutputsMisc
	{
		// Token: 0x0600296B RID: 10603 RVA: 0x00123F50 File Offset: 0x00122150
		[DebugOutput]
		public static void MiningResourceGeneration()
		{
			Func<ThingDef, ThingDef> mineable = delegate(ThingDef d)
			{
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			};
			Func<ThingDef, float> mineableCommonality = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterCommonality;
				}
				return 0f;
			};
			Func<ThingDef, IntRange> mineableLumpSizeRange = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return mineable(d).building.mineableScatterLumpSizeRange;
				}
				return IntRange.zero;
			};
			Func<ThingDef, float> mineableYield = delegate(ThingDef d)
			{
				if (mineable(d) != null)
				{
					return (float)mineable(d).building.EffectiveMineableYield;
				}
				return 0f;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.deepCommonality > 0f || mineableCommonality(d) > 0f
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("stackLimit", (ThingDef d) => d.stackLimit);
			array[3] = new TableDataGetter<ThingDef>("deep\ncommonality", (ThingDef d) => d.deepCommonality.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("deep\nlump size", (ThingDef d) => d.deepLumpSizeRange);
			array[5] = new TableDataGetter<ThingDef>("deep lump\nvalue min", (ThingDef d) => ((float)d.deepLumpSizeRange.min * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[6] = new TableDataGetter<ThingDef>("deep lump\nvalue avg", (ThingDef d) => (d.deepLumpSizeRange.Average * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("deep lump\nvalue max", (ThingDef d) => ((float)d.deepLumpSizeRange.max * d.BaseMarketValue * (float)d.deepCountPerCell).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("deep count\nper cell", (ThingDef d) => d.deepCountPerCell);
			array[9] = new TableDataGetter<ThingDef>("deep count\nper portion", (ThingDef d) => d.deepCountPerPortion);
			array[10] = new TableDataGetter<ThingDef>("deep portion\nvalue", (ThingDef d) => ((float)d.deepCountPerPortion * d.BaseMarketValue).ToStringMoney(null));
			array[11] = new TableDataGetter<ThingDef>("mineable\ncommonality", (ThingDef d) => mineableCommonality(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("mineable\nlump size", (ThingDef d) => mineableLumpSizeRange(d));
			array[13] = new TableDataGetter<ThingDef>("mineable yield\nper cell", (ThingDef d) => mineableYield(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x00124214 File Offset: 0x00122414
		[DebugOutput]
		public static void NaturalRocks()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && (d.building.isNaturalRock || d.building.isResourceRock) && !d.IsSmoothed
			select d into x
			orderby x.building.isNaturalRock descending, x.building.isResourceRock descending
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[9];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("isNaturalRock", (ThingDef d) => d.building.isNaturalRock.ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("isResourceRock", (ThingDef d) => d.building.isResourceRock.ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("smoothed", delegate(ThingDef d)
			{
				if (d.building.smoothedThing == null)
				{
					return "";
				}
				return d.building.smoothedThing.defName;
			});
			array[4] = new TableDataGetter<ThingDef>("mineableThing", delegate(ThingDef d)
			{
				if (d.building.mineableThing == null)
				{
					return "";
				}
				return d.building.mineableThing.defName;
			});
			array[5] = new TableDataGetter<ThingDef>("mineableYield", (ThingDef d) => d.building.EffectiveMineableYield);
			array[6] = new TableDataGetter<ThingDef>("mineableYieldWasteable", (ThingDef d) => d.building.mineableYieldWasteable);
			array[7] = new TableDataGetter<ThingDef>("NaturalRockType\never possible", (ThingDef d) => d.IsNonResourceNaturalRock.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("NaturalRockType\nin CurrentMap", delegate(ThingDef d)
			{
				if (Find.CurrentMap == null)
				{
					return "";
				}
				return Find.World.NaturalRockTypesIn(Find.CurrentMap.Tile).Contains(d).ToStringCheckBlank();
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x0012442C File Offset: 0x0012262C
		[DebugOutput]
		public static void MeditationFoci()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.StatBaseDefined(StatDefOf.MeditationFocusStrength)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[13];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null).ToStringPercent());
			array[2] = new TableDataGetter<ThingDef>("max\ntotal", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__TotalMax|2_3(d).ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("offset 0\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|2_1(d, 0));
			array[4] = new TableDataGetter<ThingDef>("offset 0\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, 0).ToStringPercentEmptyZero("F0"));
			array[5] = new TableDataGetter<ThingDef>("offset 1\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|2_1(d, 1));
			array[6] = new TableDataGetter<ThingDef>("offset 1\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, 1).ToStringPercentEmptyZero("F0"));
			array[7] = new TableDataGetter<ThingDef>("offset 2\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|2_1(d, 2));
			array[8] = new TableDataGetter<ThingDef>("offset 2\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, 2).ToStringPercentEmptyZero("F0"));
			array[9] = new TableDataGetter<ThingDef>("offset 3\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|2_1(d, 3));
			array[10] = new TableDataGetter<ThingDef>("offset 3\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, 3).ToStringPercentEmptyZero("F0"));
			array[11] = new TableDataGetter<ThingDef>("offset 4\nname", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetClassName|2_1(d, 4));
			array[12] = new TableDataGetter<ThingDef>("offset 4\nmax", (ThingDef d) => DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, 4).ToStringPercentEmptyZero("F0"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x001246B0 File Offset: 0x001228B0
		[DebugOutput]
		public static void DefaultStuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.MadeFromStuff && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("default stuff", (ThingDef d) => GenStuff.DefaultStuffFor(d).defName);
			array[3] = new TableDataGetter<ThingDef>("stuff categories", (ThingDef d) => (from c in d.stuffCategories
			select c.defName).ToCommaList(false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x001247A4 File Offset: 0x001229A4
		[DebugOutput]
		public static void Beauties()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>()).Where(delegate(BuildableDef d)
			{
				ThingDef thingDef = d as ThingDef;
				if (thingDef != null)
				{
					return BeautyUtility.BeautyRelevant(thingDef.category);
				}
				return d is TerrainDef;
			})
			orderby (int)d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[6];
			array[0] = new TableDataGetter<BuildableDef>("category", delegate(BuildableDef d)
			{
				if (!(d is ThingDef))
				{
					return "Terrain";
				}
				return ((ThingDef)d).category.ToString();
			});
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("beauty", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<BuildableDef>("market value", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<BuildableDef>("work to produce", (BuildableDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString());
			array[5] = new TableDataGetter<BuildableDef>("beauty per market value", delegate(BuildableDef d)
			{
				if (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f)
				{
					return "";
				}
				return (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x00124928 File Offset: 0x00122B28
		[DebugOutput]
		public static void StuffBeauty()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|5_0(d, StatDefOf.Beauty) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("beauty factor", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|5_0(d, StatDefOf.Beauty).ToString());
			array[2] = new TableDataGetter<ThingDef>("beauty offset", (ThingDef d) => DebugOutputsMisc.<StuffBeauty>g__getStatOffsetVal|5_1(d, StatDefOf.Beauty).ToString());
			array[3] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("beauty factor per market value", delegate(ThingDef d)
			{
				if (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|5_0(d, StatDefOf.Beauty) <= 0f)
				{
					return "";
				}
				return (DebugOutputsMisc.<StuffBeauty>g__getStatFactorVal|5_0(d, StatDefOf.Beauty) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00124A6C File Offset: 0x00122C6C
		[DebugOutput]
		public static void ThingsPowerAndHeat()
		{
			Func<ThingDef, CompProperties_HeatPusher> heatPusher = delegate(ThingDef d)
			{
				if (d.comps == null)
				{
					return null;
				}
				for (int i = 0; i < d.comps.Count; i++)
				{
					CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
					if (compProperties_HeatPusher != null)
					{
						return compProperties_HeatPusher;
					}
				}
				return null;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || heatPusher(d) != null) && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base\npower consumption", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() != null)
				{
					return d.GetCompProperties<CompProperties_Power>().basePowerConsumption.ToString();
				}
				return "";
			});
			array[2] = new TableDataGetter<ThingDef>("short circuit\nin rain", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain)
				{
					return "";
				}
				return "rainfire";
			});
			array[3] = new TableDataGetter<ThingDef>("transmits\npower", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_Power>() == null)
				{
					return "";
				}
				if (!d.GetCompProperties<CompProperties_Power>().transmitsPower)
				{
					return "";
				}
				return "transmit";
			});
			array[4] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue);
			array[5] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, false));
			array[6] = new TableDataGetter<ThingDef>("heat pusher\ncompClass", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).compClass.ToString();
				}
				return "";
			});
			array[7] = new TableDataGetter<ThingDef>("heat pusher\nheat per sec", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPerSecond.ToString();
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("heat pusher\nmin temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			array[9] = new TableDataGetter<ThingDef>("heat pusher\nmax temp", delegate(ThingDef d)
			{
				if (heatPusher(d) != null)
				{
					return heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1");
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00124C34 File Offset: 0x00122E34
		[DebugOutput]
		public static void FoodPoisonChances()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsIngestible
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("food poison chance", delegate(ThingDef d)
			{
				if (d.GetCompProperties<CompProperties_FoodPoisonable>() != null)
				{
					return "poisonable by cook";
				}
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
				if (statValueAbstract != 0f)
				{
					return statValueAbstract.ToStringPercent();
				}
				return "";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x00124CFC File Offset: 0x00122EFC
		[DebugOutput]
		public static void TechLevels()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building || d.category == ThingCategory.Item
			where !d.IsFrame && (d.building == null || !d.building.isNaturalRock)
			orderby (int)d.techLevel descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("tech level", (ThingDef d) => d.techLevel.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x00124E0C File Offset: 0x0012300C
		[DebugOutput]
		public static void Stuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[26];
			array[0] = new TableDataGetter<ThingDef>("fabric", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank());
			array[1] = new TableDataGetter<ThingDef>("leather", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("metal", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("stony", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("woody", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[6] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[7] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[8] = new TableDataGetter<ThingDef>("base\nmarket\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("melee\ncooldown\nmultiplier", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.MeleeWeapon_CooldownMultiplier));
			array[10] = new TableDataGetter<ThingDef>("melee\nsharp\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("melee\nsharp\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsSharpFactorOverall|9_1(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("melee\nblunt\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("melee\nblunt\ndps factor\noverall", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__meleeDpsBluntFactorOverall|9_2(d).ToString("F2"));
			array[14] = new TableDataGetter<ThingDef>("armor power\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("armor power\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("armor power\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("insulation\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("insulation\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("factor\nFlammability", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.Flammability));
			array[21] = new TableDataGetter<ThingDef>("factor\nWorkToMake", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.WorkToMake));
			array[22] = new TableDataGetter<ThingDef>("factor\nWorkToBuild", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.WorkToBuild));
			array[23] = new TableDataGetter<ThingDef>("factor\nMaxHp", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.MaxHitPoints));
			array[24] = new TableDataGetter<ThingDef>("factor\nBeauty", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.Beauty));
			array[25] = new TableDataGetter<ThingDef>("factor\nDoorspeed", (ThingDef d) => DebugOutputsMisc.<Stuffs>g__getStatFactorString|9_0(d, StatDefOf.DoorOpenSpeed));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x001252FC File Offset: 0x001234FC
		[DebugOutput]
		public static void BurningAndSmeltingThings()
		{
			List<RecipeDef> burnRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef.defName.Substring(0, 4).ToLower().Equals("burn"))
				{
					burnRecipes.Add(recipeDef);
				}
			}
			List<RecipeDef> smeltRecipes = new List<RecipeDef>();
			foreach (RecipeDef recipeDef2 in DefDatabase<RecipeDef>.AllDefsListForReading)
			{
				if (recipeDef2.defName.Substring(0, 5).ToLower().Equals("smelt"))
				{
					smeltRecipes.Add(recipeDef2);
				}
			}
			IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[8];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.BaseFlammability);
			array[2] = new TableDataGetter<ThingDef>("burn- or smeltable", (ThingDef d) => (smeltRecipes.Any((RecipeDef r) => r.IsIngredient(d)) || burnRecipes.Any((RecipeDef r) => r.IsIngredient(d))).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("burnable", (ThingDef d) => d.burnableByRecipe.ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("smeltable", (ThingDef d) => d.smeltable.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("burn recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in burnRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[6] = new TableDataGetter<ThingDef>("smelt recipe", delegate(ThingDef d)
			{
				string[] array2 = (from r in smeltRecipes
				where r.IsIngredient(d)
				select r.ToString()).ToArray<string>();
				if (array2.Length != 0)
				{
					return string.Join(",", array2);
				}
				return "NONE";
			});
			array[7] = new TableDataGetter<ThingDef>("category", delegate(ThingDef d)
			{
				if (d.thingCategories != null)
				{
					return string.Join(",", (from c in d.thingCategories
					select c.defName).ToArray<string>());
				}
				return "NULL";
			});
			DebugTables.MakeTablesDialog<ThingDef>(allDefs, array);
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x00125528 File Offset: 0x00123728
		[DebugOutput]
		public static void Medicines()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21];
			array[0] = new TableDataGetter<float>("potency", (float p) => p.ToStringPercent());
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog<float>(list, array);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x00125634 File Offset: 0x00123834
		[DebugOutput]
		public static void ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracyPawn;
			Func<StatModifier, bool> <>9__19;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					IEnumerable<StatModifier> statOffsets = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets;
					Func<StatModifier, bool> predicate;
					if ((predicate = <>9__19) == null)
					{
						predicate = (<>9__19 = ((StatModifier so) => so.stat == stat));
					}
					float value = statOffsets.First(predicate).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			IEnumerable<int> dataSources = list;
			TableDataGetter<int>[] array = new TableDataGetter<int>[18];
			array[0] = new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString());
			array[1] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"));
			array[2] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"));
			array[3] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"));
			array[4] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"));
			array[5] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"));
			array[6] = new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString());
			array[7] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"));
			array[8] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"));
			array[9] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"));
			array[10] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"));
			array[11] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"));
			array[12] = new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString());
			array[13] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"));
			array[14] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"));
			array[15] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"));
			array[16] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"));
			array[17] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"));
			DebugTables.MakeTablesDialog<int>(dataSources, array);
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000217C1 File Offset: 0x0001F9C1
		[DebugOutput(true)]
		public static void TemperatureData()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemps();
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x000217D2 File Offset: 0x0001F9D2
		[DebugOutput(true)]
		public static void WeatherChances()
		{
			Find.CurrentMap.weatherDecider.LogWeatherChances();
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000217E3 File Offset: 0x0001F9E3
		[DebugOutput(true)]
		public static void CelestialGlow()
		{
			GenCelestial.LogSunGlowForYear();
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000217EA File Offset: 0x0001F9EA
		[DebugOutput(true)]
		public static void SunAngle()
		{
			GenCelestial.LogSunAngleForYear();
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000217F1 File Offset: 0x0001F9F1
		[DebugOutput(true)]
		public static void FallColor()
		{
			PlantUtility.LogFallColorForYear();
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000217F8 File Offset: 0x0001F9F8
		[DebugOutput(true)]
		public static void PawnsListAllOnMap()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00021809 File Offset: 0x0001FA09
		[DebugOutput(true)]
		public static void WindSpeeds()
		{
			Find.CurrentMap.windManager.LogWindSpeeds();
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000217F8 File Offset: 0x0001F9F8
		[DebugOutput(true)]
		public static void MapPawnsList()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0002181A File Offset: 0x0001FA1A
		[DebugOutput]
		public static void Lords()
		{
			Find.CurrentMap.lordManager.LogLords();
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x00125890 File Offset: 0x00123A90
		[DebugOutput]
		public static void BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<BodyPartTagDef> enumerator2 = (from elem in localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags)
					orderby elem
					select elem).Distinct<BodyPartTagDef>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyPartTagDef tag = enumerator2.Current;
							stringBuilder.AppendLine(tag.defName);
							IEnumerable<BodyPartRecord> allParts = localBd.AllParts;
							Func<BodyPartRecord, bool> predicate;
							Func<BodyPartRecord, bool> <>9__3;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((BodyPartRecord part) => part.def.tags.Contains(tag)));
							}
							foreach (BodyPartRecord bodyPartRecord in from part in allParts.Where(predicate)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
							}
						}
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x00125928 File Offset: 0x00123B28
		[DebugOutput]
		public static void MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.Minifiable)
				{
					stringBuilder.Append(thingDef.defName);
					if (!thingDef.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(thingDef.tradeTags.ToCommaList(false));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x001259C8 File Offset: 0x00123BC8
		[DebugOutput]
		public static void ThingSetMakerTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass24_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass24_1();
					DebugOutputsMisc.<>c__DisplayClass24_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							StringBuilder stringBuilder = new StringBuilder();
							float num = 0f;
							float num2 = 0f;
							for (int i = 0; i < 50; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								if (stringBuilder.Length > 0)
								{
									stringBuilder.AppendLine();
								}
								if (list3.NullOrEmpty<Thing>())
								{
									stringBuilder.AppendLine("-(nothing generated)");
								}
								float num3 = 0f;
								float num4 = 0f;
								for (int j = 0; j < list3.Count; j++)
								{
									stringBuilder.AppendLine("-" + list3[j].LabelCap + " - $" + (list3[j].MarketValue * (float)list3[j].stackCount).ToString("F0"));
									num3 += list3[j].MarketValue * (float)list3[j].stackCount;
									if (!(list3[j] is Pawn))
									{
										num4 += list3[j].GetStatValue(StatDefOf.Mass, true) * (float)list3[j].stackCount;
									}
									list3[j].Destroy(DestroyMode.Vanish);
								}
								num += num3;
								num2 += num4;
								stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
								stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
							}
							StringBuilder stringBuilder2 = new StringBuilder();
							stringBuilder2.AppendLine("Default thing sets generated by: " + localDef.defName);
							string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localDef.root.fixedParams);
							stringBuilder2.AppendLine("root fixedParams: " + (nonNullFieldsDebugInfo.NullOrEmpty() ? "none" : nonNullFieldsDebugInfo));
							string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
							if (!nonNullFieldsDebugInfo2.NullOrEmpty())
							{
								stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
							}
							stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
							stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
							stringBuilder2.AppendLine();
							stringBuilder2.Append(stringBuilder.ToString());
							Log.Message(stringBuilder2.ToString(), false);
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x00125A58 File Offset: 0x00123C58
		[DebugOutput]
		public static void ThingSetMakerPossibleDefs()
		{
			Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
			foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				generatableThings[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
			}
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null)));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass()));
			list.Add(new TableDataGetter<ThingDef>("min\ncount", delegate(ThingDef d)
			{
				if (d.stackLimit == 1)
				{
					return "";
				}
				return d.minRewardCount.ToString();
			}));
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				list.Add(new TableDataGetter<ThingDef>(localDef.defName.Shorten(), (ThingDef d) => generatableThings[localDef].Contains(d).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.BaseMarketValue descending
			select d, list.ToArray());
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00125C68 File Offset: 0x00123E68
		[DebugOutput]
		public static void ThingSetMakerSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				Action<ThingSetMakerParams> <>9__1;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					DebugOutputsMisc.<>c__DisplayClass26_1 CS$<>8__locals2 = new DebugOutputsMisc.<>c__DisplayClass26_1();
					DebugOutputsMisc.<>c__DisplayClass26_1 CS$<>8__locals3 = CS$<>8__locals2;
					Action<ThingSetMakerParams> generate;
					if ((generate = <>9__1) == null)
					{
						generate = (<>9__1 = delegate(ThingSetMakerParams parms)
						{
							Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
							for (int i = 0; i < 500; i++)
							{
								List<Thing> list3 = localDef.root.Generate(parms);
								foreach (ThingDef thingDef in (from th in list3
								select th.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									if (!counts.ContainsKey(thingDef))
									{
										counts.Add(thingDef, 0);
									}
									Dictionary<ThingDef, int> counts2 = counts;
									ThingDef key = thingDef;
									int num = counts2[key];
									counts2[key] = num + 1;
								}
								for (int j = 0; j < list3.Count; j++)
								{
									list3[j].Destroy(DestroyMode.Vanish);
								}
							}
							IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
							where counts.ContainsKey(d)
							orderby counts[d] descending
							select d;
							TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
							array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
							array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
							array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
							array[3] = new TableDataGetter<ThingDef>("appearance rate in " + localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
							DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
						});
					}
					CS$<>8__locals3.generate = generate;
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.makingFaction = localF;
											obj.traderDef = localKind;
											CS$<>8__locals2.generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
						return;
					}
					CS$<>8__locals2.generate(localDef.debugParams);
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00125CF8 File Offset: 0x00123EF8
		[DebugOutput]
		public static void RewardsGeneration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction != Faction.OfPlayer)
				{
					Faction localF = faction;
					list.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
						{
							float localPoints = localPoints2;
							list2.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int i = 0; i < 30; i++)
								{
									RewardsGeneratorParams rewardsGeneratorParams = new RewardsGeneratorParams
									{
										allowGoodwill = true,
										allowRoyalFavor = true,
										populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest,
										giverFaction = localF,
										rewardValue = localPoints
									};
									float f;
									List<Reward> source = RewardsGenerator.Generate(rewardsGeneratorParams, out f);
									StringBuilder stringBuilder2 = stringBuilder;
									string[] array = new string[8];
									array[0] = "giver: ";
									array[1] = rewardsGeneratorParams.giverFaction.Name;
									array[2] = ", input value: ";
									array[3] = rewardsGeneratorParams.rewardValue.ToStringMoney(null);
									array[4] = ", output value: ";
									array[5] = f.ToStringMoney(null);
									array[6] = "\n";
									array[7] = (from x in source
									select "-" + x.ToString()).ToLineList(null, false).Indented("  ");
									stringBuilder2.AppendLine(string.Concat(array));
									stringBuilder.AppendLine();
								}
								Log.Message(stringBuilder.ToString(), false);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00125DB0 File Offset: 0x00123FB0
		[DebugOutput]
		public static void RewardsGenerationSampled()
		{
			RewardsGeneratorParams parms = default(RewardsGeneratorParams);
			parms.allowGoodwill = true;
			parms.allowRoyalFavor = true;
			parms.populationIntent = StorytellerUtilityPopulation.PopulationIntentForQuest;
			parms.giverFaction = (from x in Find.FactionManager.GetFactions_NewTemp(false, false, true, TechLevel.Undefined, false)
			where !x.Hidden && x.def.humanlikeFaction && !x.HostileTo(Faction.OfPlayer)
			select x).First<Faction>();
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
				{
					parms.rewardValue = localPoints;
					Dictionary<Type, int> countByType = new Dictionary<Type, int>();
					Dictionary<ThingDef, int> countByThingDef = new Dictionary<ThingDef, int>();
					for (int i = 0; i < 1000; i++)
					{
						foreach (Reward reward in RewardsGenerator.Generate(parms))
						{
							countByType.Increment(reward.GetType());
							Reward_Items reward_Items = reward as Reward_Items;
							if (reward_Items != null)
							{
								foreach (ThingDef key in (from x in reward_Items.items
								select x.GetInnerIfMinified().def).Distinct<ThingDef>())
								{
									countByThingDef.Increment(key);
								}
							}
						}
					}
					Dictionary<ThingSetMakerDef, List<ThingDef>> dictionary = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
					foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
					{
						ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
						dictionary[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
					}
					List<TableDataGetter<object>> list2 = new List<TableDataGetter<object>>();
					list2.Add(new TableDataGetter<object>("defName", delegate(object d)
					{
						if (!(d is Type))
						{
							return ((ThingDef)d).defName;
						}
						return "*" + ((Type)d).Name;
					}));
					list2.Add(new TableDataGetter<object>(string.Concat(new object[]
					{
						"times appeared\nin ",
						1000,
						" rewards\nof value ",
						localPoints
					}), delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}));
					DebugTables.MakeTablesDialog<object>(typeof(Reward).AllSubclassesNonAbstract().Cast<object>().Union(countByThingDef.Keys.Cast<object>()).OrderByDescending(delegate(object d)
					{
						if (!(d is Type))
						{
							return countByThingDef.TryGetValue((ThingDef)d, 0);
						}
						return countByType.TryGetValue((Type)d, 0);
					}), list2.ToArray());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00125EC8 File Offset: 0x001240C8
		[DebugOutput]
		public static void WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pkInner2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = pkInner2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionType);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int i = 0; i < num; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction);
						if (pawn.workSettings != null)
						{
							foreach (WorkTypeDef workTypeDef in pawn.GetDisabledWorkTypes(true))
							{
								DefMap<WorkTypeDef, int> defMap2 = defMap;
								WorkTypeDef def = workTypeDef;
								int num2 = defMap2[def];
								defMap2[def] = num2 + 1;
							}
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Generated ",
						num,
						" pawns of kind ",
						pkInner.defName,
						" on faction ",
						faction.ToStringSafe<Faction>()
					}));
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (workTypeDef2.workTags != WorkTags.None)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								workTypeDef2.defName,
								": ",
								defMap[workTypeDef2],
								"        ",
								((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
							}));
						}
					}
					IEnumerable<Backstory> enumerable = BackstoryDatabase.allBackstories.Select((KeyValuePair<string, Backstory> kvp) => kvp.Value);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							WorkTypeDef wt = enumerator3.Current;
							int num3 = 0;
							Func<WorkTypeDef, bool> <>9__3;
							foreach (Backstory backstory in enumerable)
							{
								IEnumerable<WorkTypeDef> disabledWorkTypes = backstory.DisabledWorkTypes;
								Func<WorkTypeDef, bool> predicate;
								if ((predicate = <>9__3) == null)
								{
									predicate = (<>9__3 = ((WorkTypeDef wd) => wt == wd));
								}
								if (disabledWorkTypes.Any(predicate))
								{
									num3++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								wt.defName,
								": ",
								num3,
								"     ",
								((float)num3 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
							}));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					foreach (object obj in Enum.GetValues(typeof(WorkTags)))
					{
						WorkTags workTags = (WorkTags)obj;
						int num4 = 0;
						foreach (Backstory backstory2 in enumerable)
						{
							if ((workTags & backstory2.workDisables) != WorkTags.None)
							{
								num4++;
							}
						}
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							workTags,
							": ",
							num4,
							"     ",
							((float)num4 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
						}));
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00125F9C File Offset: 0x0012419C
		[DebugOutput]
		public static void FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", thingDef.ingestible.preferability, thingDef.defName));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x0002182B File Offset: 0x0001FA2B
		[DebugOutput]
		public static void IngestibleMaxSatisfiedTitle()
		{
			RoyalTitleUtility.DoTable_IngestibleMaxSatisfiedTitle();
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00021832 File Offset: 0x0001FA32
		[DebugOutput]
		public static void AbilityCosts()
		{
			AbilityUtility.DoTable_AbilityCosts();
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00126070 File Offset: 0x00124270
		[DebugOutput]
		public static void MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x001260FC File Offset: 0x001242FC
		[DebugOutput]
		public static void GenSteps()
		{
			IEnumerable<GenStepDef> dataSources = from x in DefDatabase<GenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<GenStepDef>[] array = new TableDataGetter<GenStepDef>[4];
			array[0] = new TableDataGetter<GenStepDef>("defName", (GenStepDef x) => x.defName);
			array[1] = new TableDataGetter<GenStepDef>("order", (GenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<GenStepDef>("class", (GenStepDef x) => x.genStep.GetType().Name);
			array[3] = new TableDataGetter<GenStepDef>("site", delegate(GenStepDef x)
			{
				if (x.linkWithSite == null)
				{
					return "";
				}
				return x.linkWithSite.defName;
			});
			DebugTables.MakeTablesDialog<GenStepDef>(dataSources, array);
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00126214 File Offset: 0x00124414
		[DebugOutput]
		public static void WorldGenSteps()
		{
			IEnumerable<WorldGenStepDef> dataSources = from x in DefDatabase<WorldGenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<WorldGenStepDef>[] array = new TableDataGetter<WorldGenStepDef>[3];
			array[0] = new TableDataGetter<WorldGenStepDef>("defName", (WorldGenStepDef x) => x.defName);
			array[1] = new TableDataGetter<WorldGenStepDef>("order", (WorldGenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<WorldGenStepDef>("class", (WorldGenStepDef x) => x.worldGenStep.GetType().Name);
			DebugTables.MakeTablesDialog<WorldGenStepDef>(dataSources, array);
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x00126300 File Offset: 0x00124500
		[DebugOutput]
		public static void ShuttleDefsToAvoid()
		{
			IEnumerable<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[2];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef x) => x.defName);
			array[1] = new TableDataGetter<ThingDef>("avoid", (ThingDef x) => (GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, x) || (x.plant != null && x.plant.IsTree) || x.category == ThingCategory.Item || x.category == ThingCategory.Building).ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThingDef>(allDefsListForReading, array);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x00126378 File Offset: 0x00124578
		[CompilerGenerated]
		internal static bool <MeditationFoci>g__TryGetOffset|2_0(ThingDef d, int index, out FocusStrengthOffset result)
		{
			result = null;
			CompProperties_MeditationFocus compProperties = d.GetCompProperties<CompProperties_MeditationFocus>();
			if (compProperties == null)
			{
				return false;
			}
			if (compProperties.offsets.Count <= index)
			{
				return false;
			}
			result = compProperties.offsets[index];
			return true;
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x001263B4 File Offset: 0x001245B4
		[CompilerGenerated]
		internal static string <MeditationFoci>g__GetOffsetClassName|2_1(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|2_0(d, index, out focusStrengthOffset))
			{
				return "";
			}
			return focusStrengthOffset.GetType().Name;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x001263E0 File Offset: 0x001245E0
		[CompilerGenerated]
		internal static float <MeditationFoci>g__GetOffsetMax|2_2(ThingDef d, int index)
		{
			FocusStrengthOffset focusStrengthOffset;
			if (!DebugOutputsMisc.<MeditationFoci>g__TryGetOffset|2_0(d, index, out focusStrengthOffset))
			{
				return 0f;
			}
			return Mathf.Max(focusStrengthOffset.MaxOffset(null), 0f);
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x00126410 File Offset: 0x00124610
		[CompilerGenerated]
		internal static float <MeditationFoci>g__TotalMax|2_3(ThingDef d)
		{
			float num = d.GetStatValueAbstract(StatDefOf.MeditationFocusStrength, null);
			for (int i = 0; i < 5; i++)
			{
				num += DebugOutputsMisc.<MeditationFoci>g__GetOffsetMax|2_2(d, i);
			}
			return num;
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x00126444 File Offset: 0x00124644
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatFactorVal|5_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x001264A0 File Offset: 0x001246A0
		[CompilerGenerated]
		internal static float <StuffBeauty>g__getStatOffsetVal|5_1(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statOffsets == null)
			{
				return 0f;
			}
			StatModifier statModifier = d.stuffProps.statOffsets.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return 0f;
			}
			return statModifier.value;
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x001264FC File Offset: 0x001246FC
		[CompilerGenerated]
		internal static string <Stuffs>g__getStatFactorString|9_0(ThingDef d, StatDef stat)
		{
			if (d.stuffProps.statFactors == null)
			{
				return "";
			}
			StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
			if (statModifier == null)
			{
				return "";
			}
			return stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute, true);
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x00126564 File Offset: 0x00124764
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsSharpFactorOverall|9_1(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x00126598 File Offset: 0x00124798
		[CompilerGenerated]
		internal static float <Stuffs>g__meleeDpsBluntFactorOverall|9_2(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}
	}
}
