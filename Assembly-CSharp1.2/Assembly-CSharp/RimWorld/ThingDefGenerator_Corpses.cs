using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF1 RID: 3825
	public static class ThingDefGenerator_Corpses
	{
		// Token: 0x060054AA RID: 21674 RVA: 0x0003ABAF File Offset: 0x00038DAF
		public static IEnumerable<ThingDef> ImpliedCorpseDefs()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>())
			{
				if (thingDef.category == ThingCategory.Pawn)
				{
					ThingDef thingDef2 = new ThingDef();
					thingDef2.category = ThingCategory.Item;
					thingDef2.thingClass = typeof(Corpse);
					thingDef2.selectable = true;
					thingDef2.tickerType = TickerType.Rare;
					thingDef2.altitudeLayer = AltitudeLayer.ItemImportant;
					thingDef2.scatterableOnMapGen = false;
					thingDef2.SetStatBaseValue(StatDefOf.Beauty, -50f);
					thingDef2.SetStatBaseValue(StatDefOf.DeteriorationRate, 1f);
					thingDef2.SetStatBaseValue(StatDefOf.FoodPoisonChanceFixedHuman, 0.05f);
					thingDef2.alwaysHaulable = true;
					thingDef2.soundDrop = SoundDefOf.Corpse_Drop;
					thingDef2.pathCost = DefGenerator.StandardItemPathCost;
					thingDef2.socialPropernessMatters = false;
					thingDef2.tradeability = Tradeability.None;
					thingDef2.messageOnDeteriorateInStorage = false;
					thingDef2.inspectorTabs = new List<Type>();
					thingDef2.inspectorTabs.Add(typeof(ITab_Pawn_Health));
					thingDef2.inspectorTabs.Add(typeof(ITab_Pawn_Character));
					thingDef2.inspectorTabs.Add(typeof(ITab_Pawn_Gear));
					thingDef2.inspectorTabs.Add(typeof(ITab_Pawn_Social));
					thingDef2.inspectorTabs.Add(typeof(ITab_Pawn_Log));
					thingDef2.comps.Add(new CompProperties_Forbiddable());
					thingDef2.recipes = new List<RecipeDef>();
					if (!thingDef.race.IsMechanoid)
					{
						thingDef2.recipes.Add(RecipeDefOf.RemoveBodyPart);
					}
					thingDef2.defName = "Corpse_" + thingDef.defName;
					thingDef2.label = "CorpseLabel".Translate(thingDef.label);
					thingDef2.description = "CorpseDesc".Translate(thingDef.label);
					thingDef2.soundImpactDefault = thingDef.soundImpactDefault;
					thingDef2.SetStatBaseValue(StatDefOf.MarketValue, ThingDefGenerator_Corpses.CalculateMarketValue(thingDef));
					thingDef2.SetStatBaseValue(StatDefOf.Flammability, thingDef.GetStatValueAbstract(StatDefOf.Flammability, null));
					thingDef2.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)thingDef.BaseMaxHitPoints);
					thingDef2.SetStatBaseValue(StatDefOf.Mass, thingDef.statBases.GetStatOffsetFromList(StatDefOf.Mass));
					thingDef2.SetStatBaseValue(StatDefOf.Nutrition, 5.2f);
					thingDef2.modContentPack = thingDef.modContentPack;
					thingDef2.ingestible = new IngestibleProperties();
					thingDef2.ingestible.parent = thingDef2;
					IngestibleProperties ingestible = thingDef2.ingestible;
					ingestible.foodType = FoodTypeFlags.Corpse;
					ingestible.sourceDef = thingDef;
					ingestible.preferability = (thingDef.race.IsFlesh ? FoodPreferability.DesperateOnly : FoodPreferability.NeverForNutrition);
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(ingestible, "tasteThought", ThoughtDefOf.AteCorpse.defName, null, null);
					ingestible.maxNumToIngestAtOnce = 1;
					ingestible.ingestEffect = EffecterDefOf.EatMeat;
					ingestible.ingestSound = SoundDefOf.RawMeat_Eat;
					ingestible.specialThoughtDirect = thingDef.race.FleshType.ateDirect;
					if (thingDef.race.IsFlesh)
					{
						CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
						compProperties_Rottable.daysToRotStart = 2.5f;
						compProperties_Rottable.daysToDessicated = 5f;
						compProperties_Rottable.rotDamagePerDay = 2f;
						compProperties_Rottable.dessicatedDamagePerDay = 0.7f;
						thingDef2.comps.Add(compProperties_Rottable);
						CompProperties_SpawnerFilth compProperties_SpawnerFilth = new CompProperties_SpawnerFilth();
						compProperties_SpawnerFilth.filthDef = ThingDefOf.Filth_CorpseBile;
						compProperties_SpawnerFilth.spawnCountOnSpawn = 0;
						compProperties_SpawnerFilth.spawnMtbHours = 0f;
						compProperties_SpawnerFilth.spawnRadius = 0.1f;
						compProperties_SpawnerFilth.spawnEveryDays = 1f;
						compProperties_SpawnerFilth.requiredRotStage = new RotStage?(RotStage.Rotting);
						thingDef2.comps.Add(compProperties_SpawnerFilth);
					}
					if (thingDef2.thingCategories == null)
					{
						thingDef2.thingCategories = new List<ThingCategoryDef>();
					}
					if (thingDef.race.Humanlike)
					{
						DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(thingDef2.thingCategories, ThingCategoryDefOf.CorpsesHumanlike.defName, thingDef2, null);
					}
					else
					{
						DirectXmlCrossRefLoader.RegisterListWantsCrossRef<ThingCategoryDef>(thingDef2.thingCategories, thingDef.race.FleshType.corpseCategory.defName, thingDef2, null);
					}
					thingDef.race.corpseDef = thingDef2;
					yield return thingDef2;
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001C5510 File Offset: 0x001C3710
		private static float CalculateMarketValue(ThingDef raceDef)
		{
			float num = 0f;
			if (raceDef.race.meatDef != null)
			{
				int num2 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.MeatAmount, null));
				num += (float)num2 * raceDef.race.meatDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			if (raceDef.race.leatherDef != null)
			{
				int num3 = Mathf.RoundToInt(raceDef.GetStatValueAbstract(StatDefOf.LeatherAmount, null));
				num += (float)num3 * raceDef.race.leatherDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			if (raceDef.butcherProducts != null)
			{
				for (int i = 0; i < raceDef.butcherProducts.Count; i++)
				{
					num += raceDef.butcherProducts[i].thingDef.BaseMarketValue * (float)raceDef.butcherProducts[i].count;
				}
			}
			return num * 0.6f;
		}

		// Token: 0x04003566 RID: 13670
		private const float DaysToStartRot = 2.5f;

		// Token: 0x04003567 RID: 13671
		private const float DaysToDessicate = 5f;

		// Token: 0x04003568 RID: 13672
		private const float RotDamagePerDay = 2f;

		// Token: 0x04003569 RID: 13673
		private const float DessicatedDamagePerDay = 0.7f;

		// Token: 0x0400356A RID: 13674
		private const float ButcherProductsMarketValueFactor = 0.6f;
	}
}
