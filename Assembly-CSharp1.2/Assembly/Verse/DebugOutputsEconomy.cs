using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200060B RID: 1547
	public static class DebugOutputsEconomy
	{
		// Token: 0x06002640 RID: 9792 RVA: 0x001190D4 File Offset: 0x001172D4
		[DebugOutput("Economy", false)]
		public static void ApparelByStuff()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate()
			{
				DebugOutputsEconomy.DoTableInternalApparel(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (ThingDef localStuff2 in from td in DefDatabase<ThingDef>.AllDefs
			where td.IsStuff
			select td)
			{
				ThingDef localStuff = localStuff2;
				list.Add(new FloatMenuOption(localStuff.defName, delegate()
				{
					DebugOutputsEconomy.DoTableInternalApparel(localStuff);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x001191C8 File Offset: 0x001173C8
		[DebugOutput("Economy", false)]
		public static void ApparelArmor()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("stuff", (ThingDef x) => x.MadeFromStuff.ToStringCheckBlank()));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef x) => x.BaseMass));
			list.Add(new TableDataGetter<ThingDef>("mrkt\nvalue", (ThingDef x) => x.BaseMarketValue.ToString("F0")));
			list.Add(new TableDataGetter<ThingDef>("hp", (ThingDef x) => x.BaseMaxHitPoints));
			list.Add(new TableDataGetter<ThingDef>("flama\nbility", (ThingDef x) => x.BaseFlammability));
			list.Add(new TableDataGetter<ThingDef>("recipe\nmin\nskill", delegate(ThingDef x)
			{
				if (x.recipeMaker == null || x.recipeMaker.skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return "";
				}
				return x.recipeMaker.skillRequirements[0].skill.defName + " " + x.recipeMaker.skillRequirements[0].minLevel;
			}));
			list.Add(new TableDataGetter<ThingDef>("equip\ndelay", (ThingDef x) => x.GetStatValueAbstract(StatDefOf.EquipDelay, null)));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return string.Concat(new string[]
				{
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null).ToStringPercent(),
					" / ",
					x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, null).ToStringPercent()
				});
			}));
			list.Add(new TableDataGetter<ThingDef>("verbs", (ThingDef x) => string.Join(",", from v in x.Verbs
			select v.label)));
			foreach (ThingDef stuffLocal2 in new List<ThingDef>
			{
				ThingDefOf.Steel,
				ThingDefOf.Plasteel,
				ThingDefOf.Cloth,
				ThingDef.Named("Leather_Patch"),
				ThingDefOf.Leather_Plain,
				ThingDef.Named("Leather_Heavy"),
				ThingDef.Named("Leather_Thrumbo"),
				ThingDef.Named("Synthread"),
				ThingDef.Named("Hyperweave"),
				ThingDef.Named("DevilstrandCloth"),
				ThingDef.Named("WoolSheep"),
				ThingDef.Named("WoolMegasloth"),
				ThingDefOf.BlocksGranite,
				ThingDefOf.Silver,
				ThingDefOf.Gold
			})
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return string.Concat(new string[]
						{
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuffLocal).ToStringPercent(),
							" / ",
							x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, stuffLocal).ToStringPercent()
						});
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00119560 File Offset: 0x00117760
		[DebugOutput("Economy", false)]
		public static void ApparelInsulation()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				if (x.MadeFromStuff)
				{
					return "";
				}
				return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, null).ToStringTemperature("F1") + " / " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, null).ToStringTemperature("F1");
			}));
			foreach (ThingDef stuffLocal2 in from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			orderby x.BaseMarketValue
			select x)
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							return "";
						}
						return x.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuffLocal).ToString("F1") + ", " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuffLocal).ToString("F1");
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x001196F8 File Offset: 0x001178F8
		private static void DoTableInternalApparel(ThingDef stuff)
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel && (stuff == null || (d.MadeFromStuff && stuff.stuffProps.CanMake(d)))
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[15];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("body\nparts", (ThingDef d) => GenText.ToSpaceList(from bp in d.apparel.bodyPartGroups
			select bp.defName));
			array[2] = new TableDataGetter<ThingDef>("layers", (ThingDef d) => GenText.ToSpaceList(from l in d.apparel.layers
			select l.ToString()));
			array[3] = new TableDataGetter<ThingDef>("tags", (ThingDef d) => GenText.ToSpaceList(from t in d.apparel.tags
			select t.ToString()));
			array[4] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("insul.\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("insul.\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("armor\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("armor\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("StuffEffectMult.\narmor", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor, stuff).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("StuffEffectMult.\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Cold, stuff).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("StuffEffectMult.\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Heat, stuff).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("equip\ntime", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.EquipDelay, stuff).ToString("F1"));
			array[14] = new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00119910 File Offset: 0x00117B10
		[DebugOutput("Economy", false)]
		public static void RecipeSkills()
		{
			IEnumerable<RecipeDef> allDefs = DefDatabase<RecipeDef>.AllDefs;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[5];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("workSkill", delegate(RecipeDef d)
			{
				if (d.workSkill != null)
				{
					return d.workSkill.defName;
				}
				return "";
			});
			array[2] = new TableDataGetter<RecipeDef>("workSpeedStat", delegate(RecipeDef d)
			{
				if (d.workSpeedStat != null)
				{
					return d.workSpeedStat.defName;
				}
				return "";
			});
			array[3] = new TableDataGetter<RecipeDef>("workSpeedStat's skillNeedFactors", delegate(RecipeDef d)
			{
				if (d.workSpeedStat == null)
				{
					return "";
				}
				if (!d.workSpeedStat.skillNeedFactors.NullOrEmpty<SkillNeed>())
				{
					return (from fac in d.workSpeedStat.skillNeedFactors
					select fac.skill.defName).ToCommaList(false);
				}
				return "";
			});
			array[4] = new TableDataGetter<RecipeDef>("workSkillLearnFactor", (RecipeDef d) => d.workSkillLearnFactor);
			DebugTables.MakeTablesDialog<RecipeDef>(allDefs, array);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x00119A0C File Offset: 0x00117C0C
		[DebugOutput("Economy", false)]
		public static void Drugs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWithinCategory(ThingCategoryDefOf.Medicine) || d.IsWithinCategory(ThingCategoryDefOf.Drugs)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[30];
			array[0] = new TableDataGetter<ThingDef>("name", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[2] = new TableDataGetter<ThingDef>("ingredients", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, true));
			array[3] = new TableDataGetter<ThingDef>("work\namount", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f)
				{
					return "-";
				}
				return DebugOutputsEconomy.WorkToProduceBest(d).ToString("F0");
			});
			array[4] = new TableDataGetter<ThingDef>("real\ningredient cost", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealIngredientCost|5_0(d).ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("real\nsell price", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealSellPrice|5_1(d).ToStringMoney(null));
			array[6] = new TableDataGetter<ThingDef>("real\nprofit\nper item", (ThingDef d) => (DebugOutputsEconomy.<Drugs>g__RealSellPrice|5_1(d) - DebugOutputsEconomy.<Drugs>g__RealIngredientCost|5_0(d)).ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("real\nprofit\nper day's work", (ThingDef d) => ((DebugOutputsEconomy.<Drugs>g__RealSellPrice|5_1(d) - DebugOutputsEconomy.<Drugs>g__RealIngredientCost|5_0(d)) / DebugOutputsEconomy.WorkToProduceBest(d) * 30000f).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("real\nbuy price", (ThingDef d) => DebugOutputsEconomy.<Drugs>g__RealBuyPrice|5_2(d).ToStringMoney(null));
			array[9] = new TableDataGetter<ThingDef>("for\npleasure", (ThingDef d) => d.IsPleasureDrug.ToStringCheckBlank());
			array[10] = new TableDataGetter<ThingDef>("non\nmedical", (ThingDef d) => d.IsNonMedicalDrug.ToStringCheckBlank());
			array[11] = new TableDataGetter<ThingDef>("joy", delegate(ThingDef d)
			{
				if (!d.IsPleasureDrug)
				{
					return "-";
				}
				return d.ingestible.joy.ToString();
			});
			array[12] = new TableDataGetter<ThingDef>("high\ngain", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetDrugHighGiver(d) == null)
				{
					return "-";
				}
				if (DrugStatsUtility.GetDrugHighGiver(d).severity <= 0f)
				{
					return "-";
				}
				return DrugStatsUtility.GetDrugHighGiver(d).severity.ToString();
			});
			array[13] = new TableDataGetter<ThingDef>("high\noffset\nper day", delegate(ThingDef d)
			{
				IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
				if (((drugHighGiver != null) ? drugHighGiver.hediffDef : null) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetHighOffsetPerDay(d).ToString();
			});
			array[14] = new TableDataGetter<ThingDef>("high\ndays\nper dose", delegate(ThingDef d)
			{
				IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
				if (((drugHighGiver != null) ? drugHighGiver.hediffDef : null) == null)
				{
					return "-";
				}
				return (DrugStatsUtility.GetDrugHighGiver(d).severity / -DrugStatsUtility.GetHighOffsetPerDay(d)).ToString("F2");
			});
			array[15] = new TableDataGetter<ThingDef>("tolerance\ngain", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetToleranceGain(d) <= 0f)
				{
					return "-";
				}
				return DrugStatsUtility.GetToleranceGain(d).ToStringPercent();
			});
			array[16] = new TableDataGetter<ThingDef>("tolerance\noffset\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetTolerance(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetToleranceOffsetPerDay(d).ToStringPercent();
			});
			array[17] = new TableDataGetter<ThingDef>("tolerance\ndays\nper dose", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetTolerance(d) == null)
				{
					return "-";
				}
				return (DrugStatsUtility.GetToleranceGain(d) / -DrugStatsUtility.GetToleranceOffsetPerDay(d)).ToString("F2");
			});
			array[18] = new TableDataGetter<ThingDef>("addiction\nmin tolerance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|5_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__MinToleranceToAddict|5_11(d).ToString();
			});
			array[19] = new TableDataGetter<ThingDef>("addiction\nnew chance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|5_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__NewAddictionChance|5_6(d).ToStringPercent();
			});
			array[20] = new TableDataGetter<ThingDef>("addiction\nnew severity", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|5_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__NewAddictionSeverity|5_7(d).ToString();
			});
			array[21] = new TableDataGetter<ThingDef>("addiction\nold severity gain", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__Addictive|5_4(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__OldAddictionSeverityOffset|5_8(d).ToString();
			});
			array[22] = new TableDataGetter<ThingDef>("addiction\noffset\nper day", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<Drugs>g__Addiction|5_5(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetAddictionOffsetPerDay(d).ToString();
			});
			array[23] = new TableDataGetter<ThingDef>("addiction\nrecover\nmin days", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<Drugs>g__Addiction|5_5(d) == null)
				{
					return "-";
				}
				return (DebugOutputsEconomy.<Drugs>g__NewAddictionSeverity|5_7(d) / -DrugStatsUtility.GetAddictionOffsetPerDay(d)).ToString("F2");
			});
			array[24] = new TableDataGetter<ThingDef>("need fall\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetNeed(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetNeed(d).fallPerDay.ToString("F2");
			});
			array[25] = new TableDataGetter<ThingDef>("need cost\nper day", delegate(ThingDef d)
			{
				if (DrugStatsUtility.GetNeed(d) == null)
				{
					return "-";
				}
				return DrugStatsUtility.GetAddictionNeedCostPerDay(d).ToStringMoney(null);
			});
			array[26] = new TableDataGetter<ThingDef>("overdose\nseverity gain", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__OverdoseSeverity|5_9(d).ToString();
			});
			array[27] = new TableDataGetter<ThingDef>("overdose\nrandom-emerg\nchance", delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
				{
					return "-";
				}
				return DebugOutputsEconomy.<Drugs>g__LargeOverdoseChance|5_10(d).ToStringPercent();
			});
			array[28] = new TableDataGetter<ThingDef>("combat\ndrug", (ThingDef d) => (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d) && d.GetCompProperties<CompProperties_Drug>().isCombatEnhancingDrug).ToStringCheckBlank());
			array[29] = new TableDataGetter<ThingDef>("safe dose\ninterval", (ThingDef d) => DrugStatsUtility.GetSafeDoseIntervalReadout(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x00119F8C File Offset: 0x0011818C
		[DebugOutput("Economy", false)]
		public static void Wool()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh && d.GetCompProperties<CompProperties_Shearable>() != null
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("animal", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("woolDef", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.defName);
			array[2] = new TableDataGetter<ThingDef>("woolAmount", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolAmount.ToString());
			array[3] = new TableDataGetter<ThingDef>("woolValue", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().woolDef.BaseMarketValue.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("shear interval", (ThingDef d) => d.GetCompProperties<CompProperties_Shearable>().shearIntervalDays.ToString("F1"));
			array[5] = new TableDataGetter<ThingDef>("value per year", delegate(ThingDef d)
			{
				CompProperties_Shearable compProperties = d.GetCompProperties<CompProperties_Shearable>();
				return (compProperties.woolDef.BaseMarketValue * (float)compProperties.woolAmount * (60f / (float)compProperties.shearIntervalDays)).ToString("F0");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x0011A0D8 File Offset: 0x001182D8
		[DebugOutput("Economy", false)]
		public static void AnimalGrowth()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby DebugOutputsEconomy.<AnimalGrowth>g__bestMeatPerInput|7_6(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[17];
			array[0] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("hungerRate", (ThingDef d) => d.race.baseHungerRate.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("gestDaysEach", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__gestDaysEach|7_0(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("herbiv", delegate(ThingDef d)
			{
				if ((d.race.foodType & FoodTypeFlags.Plant) == FoodTypeFlags.None)
				{
					return "";
				}
				return "Y";
			});
			array[4] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[5] = new TableDataGetter<ThingDef>("bodySize", (ThingDef d) => d.race.baseBodySize.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("age Adult", (ThingDef d) => d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge.ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition to adulthood", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__nutritionToAdulthood|7_4(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("adult meat-nut", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("adult meat-nut / input-nut", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__adultMeatNutPerInput|7_5(d).ToString("F3"));
			array[10] = new TableDataGetter<ThingDef>("|", (ThingDef d) => "|");
			array[11] = new TableDataGetter<ThingDef>("baby size", (ThingDef d) => (d.race.lifeStageAges[0].def.bodySizeFactor * d.race.baseBodySize).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("nutrition to gestate", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__nutritionToGestate|7_1(d).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("egg nut", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__eggNut|7_7(d));
			array[14] = new TableDataGetter<ThingDef>("baby meat-nut", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__babyMeatNut|7_2(d).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("baby meat-nut / input-nut", (ThingDef d) => DebugOutputsEconomy.<AnimalGrowth>g__babyMeatNutPerInput|7_3(d).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("baby wins", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.<AnimalGrowth>g__babyMeatNutPerInput|7_3(d) <= DebugOutputsEconomy.<AnimalGrowth>g__adultMeatNutPerInput|7_5(d))
				{
					return "";
				}
				return "B";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x0011A434 File Offset: 0x00118634
		[DebugOutput("Economy", false)]
		public static void AnimalBreeding()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Pawn && d.race.IsFlesh
			orderby DebugOutputsEconomy.GestationDaysEach(d) descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("gestDaysEach", (ThingDef d) => DebugOutputsEconomy.GestationDaysEach(d).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("avgOffspring", delegate(ThingDef d)
			{
				if (!d.HasComp(typeof(CompEggLayer)))
				{
					return ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f).ToString("F2");
				}
				return d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average.ToString("F2");
			});
			array[3] = new TableDataGetter<ThingDef>("gestDaysRaw", delegate(ThingDef d)
			{
				if (!d.HasComp(typeof(CompEggLayer)))
				{
					return d.race.gestationPeriodDays.ToString("F1");
				}
				return d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays.ToString("F1");
			});
			array[4] = new TableDataGetter<ThingDef>("growth per 30d", delegate(ThingDef d)
			{
				float f = 1f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average : ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f));
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays : d.race.gestationPeriodDays);
				float p = 30f / num;
				return Mathf.Pow(f, p).ToString("F2");
			});
			array[5] = new TableDataGetter<ThingDef>("growth per 60d", delegate(ThingDef d)
			{
				float f = 1f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggCountRange.Average : ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f));
				float num = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1].minAge * 60f + (d.HasComp(typeof(CompEggLayer)) ? d.GetCompProperties<CompProperties_EggLayer>().eggLayIntervalDays : d.race.gestationPeriodDays);
				float p = 60f / num;
				return Mathf.Pow(f, p).ToString("F2");
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0011A5A4 File Offset: 0x001187A4
		private static float GestationDaysEach(ThingDef d)
		{
			if (d.HasComp(typeof(CompEggLayer)))
			{
				CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
				return compProperties.eggLayIntervalDays / compProperties.eggCountRange.Average;
			}
			return d.race.gestationPeriodDays / ((d.race.litterSizeCurve != null) ? Rand.ByCurveAverage(d.race.litterSizeCurve) : 1f);
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0011A610 File Offset: 0x00118810
		[DebugOutput("Economy", false)]
		public static void BuildingSkills()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>())
			where d.BuildableByPlayer
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[3];
			array[0] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[1] = new TableDataGetter<BuildableDef>("construction skill prerequisite", (BuildableDef d) => d.constructionSkillPrerequisite);
			array[2] = new TableDataGetter<BuildableDef>("artistic skill prerequisite", (BuildableDef d) => d.artisticSkillPrerequisite);
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0011A6EC File Offset: 0x001188EC
		[DebugOutput("Economy", false)]
		public static void Crops()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant && d.plant.Harvestable && d.plant.Sowable
			orderby d.plant.IsTree
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[17];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("product", (ThingDef d) => d.plant.harvestedThingDef.defName);
			array[2] = new TableDataGetter<ThingDef>("grow\ntime", (ThingDef d) => d.plant.growDays.ToString("F1"));
			array[3] = new TableDataGetter<ThingDef>("work\nsow", (ThingDef d) => d.plant.sowWork.ToString("F0"));
			array[4] = new TableDataGetter<ThingDef>("work\nharvest", (ThingDef d) => d.plant.harvestWork.ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("work\ntotal", (ThingDef d) => (d.plant.sowWork + d.plant.harvestWork).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("harvest\nyield", (ThingDef d) => d.plant.harvestYield.ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("work-cost\nper cycle", (ThingDef d) => DebugOutputsEconomy.<Crops>g__workCost|11_0(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("work-cost\nper harvestCount", (ThingDef d) => (DebugOutputsEconomy.<Crops>g__workCost|11_0(d) / d.plant.harvestYield).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("value\neach", (ThingDef d) => d.plant.harvestedThingDef.BaseMarketValue.ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("harvest Value\nTotal", (ThingDef d) => (d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("profit\nper growDay", (ThingDef d) => ((d.plant.harvestYield * d.plant.harvestedThingDef.BaseMarketValue - DebugOutputsEconomy.<Crops>g__workCost|11_0(d)) / d.plant.growDays).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("nutrition\nper growDay", delegate(ThingDef d)
			{
				if (d.plant.harvestedThingDef.ingestible == null)
				{
					return "";
				}
				return (d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F2");
			});
			array[13] = new TableDataGetter<ThingDef>("nutrition", delegate(ThingDef d)
			{
				if (d.plant.harvestedThingDef.ingestible == null)
				{
					return "";
				}
				return d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
			});
			array[14] = new TableDataGetter<ThingDef>("fert\nmin", (ThingDef d) => d.plant.fertilityMin.ToStringPercent());
			array[15] = new TableDataGetter<ThingDef>("fert\nsensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToStringPercent());
			array[16] = new TableDataGetter<ThingDef>("yield per\nharvest work", (ThingDef d) => (d.plant.harvestYield / d.plant.harvestWork).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x0011AA48 File Offset: 0x00118C48
		[DebugOutput("Economy", false)]
		public static void ItemAndBuildingAcquisition()
		{
			Func<ThingDef, string> calculatedMarketValue = delegate(ThingDef d)
			{
				if (!DebugOutputsEconomy.Producible(d))
				{
					return "not producible";
				}
				if (!d.StatBaseDefined(StatDefOf.MarketValue))
				{
					return "used";
				}
				string text = StatWorker_MarketValue.CalculatedBaseMarketValue(d, null).ToString("F1");
				if (StatWorker_MarketValue.CalculableRecipe(d) != null)
				{
					return text + " (recipe)";
				}
				return text;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && (d.BuildableByPlayer || d.Minifiable))
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[16];
			array[0] = new TableDataGetter<ThingDef>("cat.", (ThingDef d) => d.category.ToString().Substring(0, 1).CapitalizeFirst());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("mobile", (ThingDef d) => (d.category == ThingCategory.Item || d.Minifiable).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("base\nmarket value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("calculated\nmarket value", (ThingDef d) => calculatedMarketValue(d));
			array[5] = new TableDataGetter<ThingDef>("cost to make", (ThingDef d) => DebugOutputsEconomy.CostToMakeString(d, false));
			array[6] = new TableDataGetter<ThingDef>("work to produce", delegate(ThingDef d)
			{
				if (DebugOutputsEconomy.WorkToProduceBest(d) <= 0f)
				{
					return "-";
				}
				return DebugOutputsEconomy.WorkToProduceBest(d).ToString("F1");
			});
			array[7] = new TableDataGetter<ThingDef>("profit", (ThingDef d) => (d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("profit\nrate", delegate(ThingDef d)
			{
				if (d.recipeMaker == null)
				{
					return "-";
				}
				return ((d.BaseMarketValue - DebugOutputsEconomy.CostToMake(d, false)) / DebugOutputsEconomy.WorkToProduceBest(d) * 10000f).ToString("F0");
			});
			array[9] = new TableDataGetter<ThingDef>("market value\ndefined", (ThingDef d) => d.statBases.Any((StatModifier st) => st.stat == StatDefOf.MarketValue).ToStringCheckBlank());
			array[10] = new TableDataGetter<ThingDef>("producible", (ThingDef d) => DebugOutputsEconomy.Producible(d).ToStringCheckBlank());
			array[11] = new TableDataGetter<ThingDef>("thing set\nmaker tags", delegate(ThingDef d)
			{
				if (!d.thingSetMakerTags.NullOrEmpty<string>())
				{
					return d.thingSetMakerTags.ToCommaList(false);
				}
				return "";
			});
			array[12] = new TableDataGetter<ThingDef>("made\nfrom\nstuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[13] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<ThingDef>("recipes", (ThingDef d) => DebugOutputsEconomy.<ItemAndBuildingAcquisition>g__recipes|12_0(d));
			array[15] = new TableDataGetter<ThingDef>("work amount\nsources", (ThingDef d) => DebugOutputsEconomy.<ItemAndBuildingAcquisition>g__workAmountSources|12_1(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0011AD90 File Offset: 0x00118F90
		[DebugOutput("Economy", false)]
		public static void ItemAccessibility()
		{
			IEnumerable<ThingDef> dataSources = from x in ThingSetMakerUtility.allGeneratableItems
			orderby x.defName
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("1", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[2] = new TableDataGetter<ThingDef>("10", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[3] = new TableDataGetter<ThingDef>("100", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 100, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[4] = new TableDataGetter<ThingDef>("1000", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 1000, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			array[5] = new TableDataGetter<ThingDef>("10000", delegate(ThingDef d)
			{
				if (!PlayerItemAccessibilityUtility.PossiblyAccessible(d, 10000, Find.CurrentMap))
				{
					return "";
				}
				return "✓";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x0011AEDC File Offset: 0x001190DC
		[DebugOutput("Economy", false)]
		public static void ThingSetMakerTags()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToString("F1")));
			List<TableDataGetter<ThingDef>> list2 = list;
			using (IEnumerator<string> enumerator = (from d in DefDatabase<ThingDef>.AllDefs
			where d.thingSetMakerTags != null
			select d).SelectMany((ThingDef d) => d.thingSetMakerTags).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string uniqueTag = enumerator.Current;
					list2.Add(new TableDataGetter<ThingDef>(uniqueTag, (ThingDef d) => (d.thingSetMakerTags != null && d.thingSetMakerTags.Contains(uniqueTag)).ToStringCheckBlank()));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.01f) || (d.category == ThingCategory.Building && d.Minifiable)
			orderby d.BaseMarketValue
			select d, list2.ToArray());
			string text = "";
			string[] array = new string[]
			{
				"RewardStandardHighFreq",
				"RewardStandardMidFreq",
				"RewardStandardLowFreq"
			};
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingSetMakerTags != null)
				{
					int num = 0;
					for (int i = 0; i < array.Length; i++)
					{
						if (thingDef.thingSetMakerTags.Contains(array[i]))
						{
							num++;
						}
					}
					if (num > 1)
					{
						text = string.Concat(new object[]
						{
							text,
							thingDef.defName,
							": ",
							num,
							" reward tags\n"
						});
					}
				}
			}
			if (text.Length > 0)
			{
				Log.Warning(text, false);
			}
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0011B138 File Offset: 0x00119338
		[DebugOutput("Economy", false)]
		public static void ThingSmeltProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, GenStuff.DefaultStuffFor(thingDef));
				if (thing.SmeltProducts(1f).Any<Thing>())
				{
					stringBuilder.Append(thing.LabelCap + ": ");
					foreach (Thing thing2 in thing.SmeltProducts(1f))
					{
						stringBuilder.Append(" " + thing2.Label);
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0011B224 File Offset: 0x00119424
		[DebugOutput("Economy", false)]
		public static void Recipes()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where !d.products.NullOrEmpty<ThingDefCountClass>() && !d.ingredients.NullOrEmpty<IngredientCount>()
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[12];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work /w carry", (RecipeDef d) => DebugOutputsEconomy.TrueWorkWithCarryTime(d).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("work seconds", (RecipeDef d) => (DebugOutputsEconomy.TrueWorkWithCarryTime(d) / 60f).ToString("F0"));
			array[3] = new TableDataGetter<RecipeDef>("cheapest products value", (RecipeDef d) => DebugOutputsEconomy.CheapestProductsValue(d).ToString("F1"));
			array[4] = new TableDataGetter<RecipeDef>("cheapest ingredients value", (RecipeDef d) => DebugOutputsEconomy.CheapestIngredientValue(d).ToString("F1"));
			array[5] = new TableDataGetter<RecipeDef>("work value", (RecipeDef d) => DebugOutputsEconomy.WorkValueEstimate(d).ToString("F1"));
			array[6] = new TableDataGetter<RecipeDef>("profit raw", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[7] = new TableDataGetter<RecipeDef>("profit with work", (RecipeDef d) => (DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.WorkValueEstimate(d) - DebugOutputsEconomy.CheapestIngredientValue(d)).ToString("F1"));
			array[8] = new TableDataGetter<RecipeDef>("profit per work day", (RecipeDef d) => ((DebugOutputsEconomy.CheapestProductsValue(d) - DebugOutputsEconomy.CheapestIngredientValue(d)) * 60000f / DebugOutputsEconomy.TrueWorkWithCarryTime(d)).ToString("F0"));
			array[9] = new TableDataGetter<RecipeDef>("min skill", delegate(RecipeDef d)
			{
				if (!d.skillRequirements.NullOrEmpty<SkillRequirement>())
				{
					return d.skillRequirements[0].Summary;
				}
				return "";
			});
			array[10] = new TableDataGetter<RecipeDef>("cheapest stuff", delegate(RecipeDef d)
			{
				if (DebugOutputsEconomy.CheapestNonDerpStuff(d) == null)
				{
					return "";
				}
				return DebugOutputsEconomy.CheapestNonDerpStuff(d).defName;
			});
			array[11] = new TableDataGetter<RecipeDef>("cheapest ingredients", (RecipeDef d) => (from pa in DebugOutputsEconomy.CheapestIngredients(d)
			select pa.First.defName + " x" + pa.Second).ToCommaList(false));
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0011B47C File Offset: 0x0011967C
		[DebugOutput("Economy", false)]
		public static void Floors()
		{
			IEnumerable<TerrainDef> dataSources = (from d in DefDatabase<TerrainDef>.AllDefs
			where d.designationCategory == DesignationCategoryDefOf.Floors || d == TerrainDefOf.Soil
			select d).Concat(TerrainDefGenerator_Stone.ImpliedTerrainDefs());
			TableDataGetter<TerrainDef>[] array = new TableDataGetter<TerrainDef>[5];
			array[0] = new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName);
			array[1] = new TableDataGetter<TerrainDef>("stuff cost", delegate(TerrainDef d)
			{
				if (d.costList.NullOrEmpty<ThingDefCountClass>())
				{
					return "";
				}
				return d.costList.First<ThingDefCountClass>().Label;
			});
			array[2] = new TableDataGetter<TerrainDef>("work to build", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null));
			array[3] = new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null));
			array[4] = new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null));
			DebugTables.MakeTablesDialog<TerrainDef>(dataSources, array);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0011B5A4 File Offset: 0x001197A4
		private static bool Producible(BuildableDef b)
		{
			ThingDef d = b as ThingDef;
			TerrainDef terrainDef = b as TerrainDef;
			if (d != null)
			{
				Predicate<ThingDefCountClass> <>9__1;
				if (DefDatabase<RecipeDef>.AllDefs.Any(delegate(RecipeDef r)
				{
					List<ThingDefCountClass> products = r.products;
					Predicate<ThingDefCountClass> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((ThingDefCountClass pr) => pr.thingDef == d));
					}
					return products.Any(predicate);
				}))
				{
					return true;
				}
				if (d.category == ThingCategory.Building && d.BuildableByPlayer)
				{
					return true;
				}
			}
			else if (terrainDef != null)
			{
				return terrainDef.BuildableByPlayer;
			}
			return false;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0011B614 File Offset: 0x00119814
		public static string CostListString(BuildableDef d, bool divideByVolume, bool starIfOnlyBuyable)
		{
			if (!DebugOutputsEconomy.Producible(d))
			{
				return "";
			}
			List<string> list = new List<string>();
			if (d.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in d.costList)
				{
					float num = (float)thingDefCountClass.count;
					if (divideByVolume)
					{
						num /= thingDefCountClass.thingDef.VolumePerUnit;
					}
					string text = thingDefCountClass.thingDef + " x" + num;
					if (starIfOnlyBuyable && DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef))
					{
						text += "*";
					}
					list.Add(text);
				}
			}
			if (d.MadeFromStuff)
			{
				list.Add("stuff x" + d.costStuffCount);
			}
			return list.ToCommaList(false);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0011B700 File Offset: 0x00119900
		private static float TrueWorkWithCarryTime(RecipeDef d)
		{
			ThingDef stuffDef = DebugOutputsEconomy.CheapestNonDerpStuff(d);
			return (float)d.ingredients.Count * 90f + d.WorkAmountTotal(stuffDef) + 90f;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0011B734 File Offset: 0x00119934
		private static float CheapestIngredientValue(RecipeDef d)
		{
			float num = 0f;
			foreach (Pair<ThingDef, float> pair in DebugOutputsEconomy.CheapestIngredients(d))
			{
				num += pair.First.BaseMarketValue * pair.Second;
			}
			return num;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0001F4A3 File Offset: 0x0001D6A3
		private static IEnumerable<Pair<ThingDef, float>> CheapestIngredients(RecipeDef d)
		{
			foreach (IngredientCount ingredientCount in d.ingredients)
			{
				ThingDef thingDef = (from td in ingredientCount.filter.AllowedThingDefs
				where td != ThingDefOf.Meat_Human
				select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
				yield return new Pair<ThingDef, float>(thingDef, ingredientCount.GetBaseCount() / d.IngredientValueGetter.ValuePerUnitOf(thingDef));
			}
			List<IngredientCount>.Enumerator enumerator = default(List<IngredientCount>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0001F4B3 File Offset: 0x0001D6B3
		private static float WorkValueEstimate(RecipeDef d)
		{
			return DebugOutputsEconomy.TrueWorkWithCarryTime(d) * 0.01f;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x0011B798 File Offset: 0x00119998
		private static ThingDef CheapestNonDerpStuff(RecipeDef d)
		{
			ThingDef productDef = d.products[0].thingDef;
			if (!productDef.MadeFromStuff)
			{
				return null;
			}
			return (from td in d.ingredients.First<IngredientCount>().filter.AllowedThingDefs
			where !productDef.IsWeapon || !PawnWeaponGenerator.IsDerpWeapon(productDef, td)
			select td).MinBy((ThingDef td) => td.BaseMarketValue / td.VolumePerUnit);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x0011B81C File Offset: 0x00119A1C
		private static float CheapestProductsValue(RecipeDef d)
		{
			float num = 0f;
			foreach (ThingDefCountClass thingDefCountClass in d.products)
			{
				num += thingDefCountClass.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, DebugOutputsEconomy.CheapestNonDerpStuff(d)) * (float)thingDefCountClass.count;
			}
			return num;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0011B890 File Offset: 0x00119A90
		private static string CostToMakeString(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return "-";
			}
			return DebugOutputsEconomy.CostToMake(d, real).ToString("F1");
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0011B8C0 File Offset: 0x00119AC0
		private static float CostToMake(ThingDef d, bool real = false)
		{
			if (d.recipeMaker == null)
			{
				return d.BaseMarketValue;
			}
			float num = 0f;
			if (d.costList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in d.costList)
				{
					float num2 = 1f;
					if (real)
					{
						num2 = (DebugOutputsEconomy.RequiresBuying(thingDefCountClass.thingDef) ? 1.4f : 0.6f);
					}
					num += (float)thingDefCountClass.count * DebugOutputsEconomy.CostToMake(thingDefCountClass.thingDef, true) * num2;
				}
			}
			if (d.costStuffCount > 0)
			{
				ThingDef thingDef = GenStuff.DefaultStuffFor(d);
				num += (float)d.costStuffCount * thingDef.BaseMarketValue;
			}
			return num;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x0011B98C File Offset: 0x00119B8C
		private static bool RequiresBuying(ThingDef def)
		{
			if (def.costList != null)
			{
				using (List<ThingDefCountClass>.Enumerator enumerator = def.costList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (DebugOutputsEconomy.RequiresBuying(enumerator.Current.thingDef))
						{
							return true;
						}
					}
				}
				return false;
			}
			return !DefDatabase<ThingDef>.AllDefs.Any((ThingDef d) => d.plant != null && d.plant.harvestedThingDef == def && d.plant.Sowable);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0011BA24 File Offset: 0x00119C24
		public static float WorkToProduceBest(BuildableDef d)
		{
			float num = float.MaxValue;
			if (d.StatBaseDefined(StatDefOf.WorkToMake))
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToMake, null);
			}
			if (d.StatBaseDefined(StatDefOf.WorkToBuild) && d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) < num)
			{
				num = d.GetStatValueAbstract(StatDefOf.WorkToBuild, null);
			}
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d && recipeDef.workAmount < num)
						{
							num = recipeDef.workAmount;
						}
					}
				}
			}
			if (num != 3.4028235E+38f)
			{
				return num;
			}
			return -1f;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0011BB18 File Offset: 0x00119D18
		[DebugOutput("Economy", false)]
		public static void HediffsPriceImpact()
		{
			IEnumerable<HediffDef> allDefs = DefDatabase<HediffDef>.AllDefs;
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef h) => h.defName));
			list.Add(new TableDataGetter<HediffDef>("price impact", (HediffDef h) => h.priceImpact.ToStringCheckBlank()));
			list.Add(new TableDataGetter<HediffDef>("price offset", delegate(HediffDef h)
			{
				if (h.priceOffset != 0f)
				{
					return h.priceOffset.ToStringMoney(null);
				}
				if (h.spawnThingOnRemoved != null)
				{
					return h.spawnThingOnRemoved.BaseMarketValue.ToStringMoney(null);
				}
				return "";
			}));
			DebugTables.MakeTablesDialog<HediffDef>(allDefs, list.ToArray());
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0001F4C1 File Offset: 0x0001D6C1
		[CompilerGenerated]
		internal static float <Drugs>g__RealIngredientCost|5_0(ThingDef d)
		{
			return DebugOutputsEconomy.CostToMake(d, true);
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0001F4CA File Offset: 0x0001D6CA
		[CompilerGenerated]
		internal static float <Drugs>g__RealSellPrice|5_1(ThingDef d)
		{
			return d.BaseMarketValue * 0.6f;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
		[CompilerGenerated]
		internal static float <Drugs>g__RealBuyPrice|5_2(ThingDef d)
		{
			return d.BaseMarketValue * 1.4f;
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0001F4E6 File Offset: 0x0001D6E6
		[CompilerGenerated]
		internal static bool <Drugs>g__IsDrug|5_3(ThingDef d)
		{
			return d.HasComp(typeof(CompDrug));
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0001F4F8 File Offset: 0x0001D6F8
		[CompilerGenerated]
		internal static bool <Drugs>g__Addictive|5_4(ThingDef d)
		{
			return DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d) && DrugStatsUtility.GetDrugComp(d).Addictive;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0001F50F File Offset: 0x0001D70F
		[CompilerGenerated]
		internal static HediffDef <Drugs>g__Addiction|5_5(ThingDef d)
		{
			if (!DebugOutputsEconomy.<Drugs>g__Addictive|5_4(d))
			{
				return null;
			}
			return DrugStatsUtility.GetChemical(d).addictionHediff;
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0001F526 File Offset: 0x0001D726
		[CompilerGenerated]
		internal static float <Drugs>g__NewAddictionChance|5_6(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).addictiveness;
			}
			return -1f;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0001F541 File Offset: 0x0001D741
		[CompilerGenerated]
		internal static float <Drugs>g__NewAddictionSeverity|5_7(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetChemical(d).addictionHediff.initialSeverity;
			}
			return -1f;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0001F561 File Offset: 0x0001D761
		[CompilerGenerated]
		internal static float <Drugs>g__OldAddictionSeverityOffset|5_8(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).existingAddictionSeverityOffset;
			}
			return -1f;
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0001F57C File Offset: 0x0001D77C
		[CompilerGenerated]
		internal static FloatRange <Drugs>g__OverdoseSeverity|5_9(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).overdoseSeverityOffset;
			}
			return FloatRange.Zero;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0001F597 File Offset: 0x0001D797
		[CompilerGenerated]
		internal static float <Drugs>g__LargeOverdoseChance|5_10(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).largeOverdoseChance;
			}
			return -1f;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0001F5B2 File Offset: 0x0001D7B2
		[CompilerGenerated]
		internal static float <Drugs>g__MinToleranceToAddict|5_11(ThingDef d)
		{
			if (DebugOutputsEconomy.<Drugs>g__IsDrug|5_3(d))
			{
				return DrugStatsUtility.GetDrugComp(d).minToleranceToAddict;
			}
			return -1f;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0001F5CD File Offset: 0x0001D7CD
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__gestDaysEach|7_0(ThingDef d)
		{
			return DebugOutputsEconomy.GestationDaysEach(d);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0011BBC8 File Offset: 0x00119DC8
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__nutritionToGestate|7_1(ThingDef d)
		{
			float num = 0f;
			LifeStageAge lifeStageAge = d.race.lifeStageAges[d.race.lifeStageAges.Count - 1];
			return num + DebugOutputsEconomy.<AnimalGrowth>g__gestDaysEach|7_0(d) * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0011BC1C File Offset: 0x00119E1C
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__babyMeatNut|7_2(ThingDef d)
		{
			LifeStageAge lifeStageAge = d.race.lifeStageAges[0];
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f * lifeStageAge.def.bodySizeFactor;
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0001F5D5 File Offset: 0x0001D7D5
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__babyMeatNutPerInput|7_3(ThingDef d)
		{
			return DebugOutputsEconomy.<AnimalGrowth>g__babyMeatNut|7_2(d) / DebugOutputsEconomy.<AnimalGrowth>g__nutritionToGestate|7_1(d);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0011BC5C File Offset: 0x00119E5C
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__nutritionToAdulthood|7_4(ThingDef d)
		{
			float num = 0f;
			num += DebugOutputsEconomy.<AnimalGrowth>g__nutritionToGestate|7_1(d);
			for (int i = 1; i < d.race.lifeStageAges.Count; i++)
			{
				LifeStageAge lifeStageAge = d.race.lifeStageAges[i];
				float num2 = (lifeStageAge.minAge - d.race.lifeStageAges[i - 1].minAge) * 60f;
				num += num2 * lifeStageAge.def.hungerRateFactor * d.race.baseHungerRate;
			}
			return num;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0001F5E4 File Offset: 0x0001D7E4
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__adultMeatNutPerInput|7_5(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MeatAmount, null) * 0.05f / DebugOutputsEconomy.<AnimalGrowth>g__nutritionToAdulthood|7_4(d);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0011BCE8 File Offset: 0x00119EE8
		[CompilerGenerated]
		internal static float <AnimalGrowth>g__bestMeatPerInput|7_6(ThingDef d)
		{
			float a = DebugOutputsEconomy.<AnimalGrowth>g__babyMeatNutPerInput|7_3(d);
			float b = DebugOutputsEconomy.<AnimalGrowth>g__adultMeatNutPerInput|7_5(d);
			return Mathf.Max(a, b);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0011BD08 File Offset: 0x00119F08
		[CompilerGenerated]
		internal static string <AnimalGrowth>g__eggNut|7_7(ThingDef d)
		{
			CompProperties_EggLayer compProperties = d.GetCompProperties<CompProperties_EggLayer>();
			if (compProperties == null)
			{
				return "";
			}
			return compProperties.eggFertilizedDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0001F5FF File Offset: 0x0001D7FF
		[CompilerGenerated]
		internal static float <Crops>g__workCost|11_0(ThingDef d)
		{
			return 1.1f + d.plant.growDays * 1f + (d.plant.sowWork + d.plant.harvestWork) * 0.00612f;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0011BD44 File Offset: 0x00119F44
		[CompilerGenerated]
		internal static string <ItemAndBuildingAcquisition>g__recipes|12_0(ThingDef d)
		{
			List<string> list = new List<string>();
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d)
						{
							list.Add(recipeDef.defName);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return "";
			}
			return list.ToCommaList(false);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0011BDE8 File Offset: 0x00119FE8
		[CompilerGenerated]
		internal static string <ItemAndBuildingAcquisition>g__workAmountSources|12_1(ThingDef d)
		{
			List<string> list = new List<string>();
			if (d.StatBaseDefined(StatDefOf.WorkToMake))
			{
				list.Add("WorkToMake(" + d.GetStatValueAbstract(StatDefOf.WorkToMake, null) + ")");
			}
			if (d.StatBaseDefined(StatDefOf.WorkToBuild))
			{
				list.Add("WorkToBuild(" + d.GetStatValueAbstract(StatDefOf.WorkToBuild, null) + ")");
			}
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				if (recipeDef.workAmount > 0f && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int i = 0; i < recipeDef.products.Count; i++)
					{
						if (recipeDef.products[i].thingDef == d)
						{
							list.Add(string.Concat(new object[]
							{
								"RecipeDef-",
								recipeDef.defName,
								"(",
								recipeDef.workAmount,
								")"
							}));
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return "";
			}
			return list.ToCommaList(false);
		}
	}
}
