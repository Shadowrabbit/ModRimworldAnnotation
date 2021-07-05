using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006BA RID: 1722
	public static class FoodUtility
	{
		// Token: 0x06002FCD RID: 12237 RVA: 0x0011A7A8 File Offset: 0x001189A8
		public static bool WillEat(this Pawn p, Thing food, Pawn getter = null, bool careIfNotAcceptableForTitle = true)
		{
			if (!p.RaceProps.CanEverEat(food))
			{
				return false;
			}
			if (p.foodRestriction != null)
			{
				FoodRestriction currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
				if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food) && (food.def.IsWithinCategory(ThingCategoryDefOf.Foods) || food.def.IsWithinCategory(ThingCategoryDefOf.Corpses)))
				{
					return false;
				}
			}
			return !FoodUtility.IsVeneratedAnimalMeatOrCorpseOrHasIngredients(food, p) && (!careIfNotAcceptableForTitle || !FoodUtility.InappropriateForTitle(food.def, p, true));
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x0011A82C File Offset: 0x00118A2C
		public static bool WillEat(this Pawn p, ThingDef food, Pawn getter = null, bool careIfNotAcceptableForTitle = true)
		{
			if (!p.RaceProps.CanEverEat(food))
			{
				return false;
			}
			if (p.foodRestriction != null)
			{
				FoodRestriction currentRespectedRestriction = p.foodRestriction.GetCurrentRespectedRestriction(getter);
				if (currentRespectedRestriction != null && !currentRespectedRestriction.Allows(food) && food.IsWithinCategory(currentRespectedRestriction.filter.DisplayRootCategory.catDef))
				{
					return false;
				}
			}
			return !FoodUtility.IsVeneratedAnimalMeatOrCorpse(food, p, null) && (!careIfNotAcceptableForTitle || !FoodUtility.InappropriateForTitle(food, p, true));
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x0011A8A0 File Offset: 0x00118AA0
		public static bool IsVeneratedAnimalMeatOrCorpseOrHasIngredients(Thing food, Pawn ingester)
		{
			if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(food.def, ingester, food))
			{
				return true;
			}
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				for (int i = 0; i < compIngredients.ingredients.Count; i++)
				{
					if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(compIngredients.ingredients[i], ingester, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x0011A8F8 File Offset: 0x00118AF8
		public static bool InappropriateForTitle(ThingDef food, Pawn p, bool allowIfStarving)
		{
			if ((allowIfStarving && p.needs.food.Starving) || (p.story != null && p.story.traits.HasTrait(TraitDefOf.Ascetic)) || p.IsPrisoner || (food.ingestible.joyKind != null && food.ingestible.joy > 0f))
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = p.royalty;
			RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
			return royalTitle != null && royalTitle.conceited && royalTitle.def.foodRequirement.Defined && !royalTitle.def.foodRequirement.Acceptable(food);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x0011A9A8 File Offset: 0x00118BA8
		public static bool TryFindBestFoodSourceFor(Pawn getter, Pawn eater, bool desperate, out Thing foodSource, out ThingDef foodDef, bool canRefillDispenser = true, bool canUseInventory = true, bool allowForbidden = false, bool allowCorpse = true, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined)
		{
			bool flag = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			bool allowDrug = !eater.IsTeetotaler();
			Thing thing = null;
			if (canUseInventory)
			{
				if (flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, eater, (minPrefOverride == FoodPreferability.Undefined) ? FoodPreferability.MealAwful : minPrefOverride, FoodPreferability.MealLavish, 0f, false);
				}
				if (thing != null)
				{
					if (getter.Faction != Faction.OfPlayer)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
			}
			ThingDef thingDef;
			Thing thing2 = FoodUtility.BestFoodSourceOnMap(getter, eater, desperate, out thingDef, FoodPreferability.MealLavish, getter == eater, allowDrug, allowCorpse, true, canRefillDispenser, allowForbidden, allowSociallyImproper, allowHarvest, forceScanWholeMap, ignoreReservations, calculateWantedStackCount, minPrefOverride, null);
			if (thing == null && thing2 == null)
			{
				if (canUseInventory && flag)
				{
					thing = FoodUtility.BestFoodInInventory(getter, eater, FoodPreferability.DesperateOnly, FoodPreferability.MealLavish, 0f, allowDrug);
					if (thing != null)
					{
						foodSource = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				if (thing2 == null && getter == eater && (getter.RaceProps.predator || (getter.IsWildMan() && !getter.IsPrisoner && !getter.WorkTypeIsDisabled(WorkTypeDefOf.Hunting))))
				{
					Pawn pawn = FoodUtility.BestPawnToHuntForPredator(getter, forceScanWholeMap);
					if (pawn != null)
					{
						foodSource = pawn;
						foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
						return true;
					}
				}
				foodSource = null;
				foodDef = null;
				return false;
			}
			if (thing == null && thing2 != null)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			if (thing2 == null)
			{
				foodSource = thing;
				foodDef = finalIngestibleDef;
				return true;
			}
			float num = FoodUtility.FoodOptimality(eater, thing2, thingDef, (float)(getter.Position - thing2.Position).LengthManhattan, false);
			float num2 = FoodUtility.FoodOptimality(eater, thing, finalIngestibleDef, 0f, false);
			num2 -= 32f;
			if (num > num2)
			{
				foodSource = thing2;
				foodDef = thingDef;
				return true;
			}
			foodSource = thing;
			foodDef = FoodUtility.GetFinalIngestibleDef(foodSource, false);
			return true;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0011ABA4 File Offset: 0x00118DA4
		public static ThingDef GetFinalIngestibleDef(Thing foodSource, bool harvest = false)
		{
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			if (building_NutrientPasteDispenser != null)
			{
				return building_NutrientPasteDispenser.DispensableDef;
			}
			Pawn pawn = foodSource as Pawn;
			if (pawn != null)
			{
				return pawn.RaceProps.corpseDef;
			}
			if (harvest)
			{
				Plant plant = foodSource as Plant;
				if (plant != null && plant.HarvestableNow && plant.def.plant.harvestedThingDef.IsIngestible)
				{
					return plant.def.plant.harvestedThingDef;
				}
			}
			return foodSource.def;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x0011AC1C File Offset: 0x00118E1C
		public static Thing BestFoodInInventory(Pawn holder, Pawn eater = null, FoodPreferability minFoodPref = FoodPreferability.NeverForNutrition, FoodPreferability maxFoodPref = FoodPreferability.MealLavish, float minStackNutrition = 0f, bool allowDrug = false)
		{
			if (holder.inventory == null)
			{
				return null;
			}
			if (eater == null)
			{
				eater = holder;
			}
			ThingOwner<Thing> innerContainer = holder.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				if (thing.def.IsNutritionGivingIngestible && thing.IngestibleNow && eater.WillEat(thing, holder, true) && thing.def.ingestible.preferability >= minFoodPref && thing.def.ingestible.preferability <= maxFoodPref && (allowDrug || !thing.def.IsDrug) && thing.GetStatValue(StatDefOf.Nutrition, true) * (float)thing.stackCount >= minStackNutrition)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x0011ACD4 File Offset: 0x00118ED4
		public static int GetMaxAmountToPickup(Thing food, Pawn pawn, int wantedCount)
		{
			if (food is Building_NutrientPasteDispenser)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return -1;
			}
			else if (food is Corpse)
			{
				if (!pawn.CanReserve(food, 1, -1, null, false))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				int num = Math.Min(wantedCount, food.stackCount);
				if (food.Spawned && food.Map != null)
				{
					return Math.Min(num, food.Map.reservationManager.CanReserveStack(pawn, food, 10, null, false));
				}
				return num;
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x0011AD60 File Offset: 0x00118F60
		public static Thing BestFoodSourceOnMap(Pawn getter, Pawn eater, bool desperate, out ThingDef foodDef, FoodPreferability maxPref = FoodPreferability.MealLavish, bool allowPlant = true, bool allowDrug = true, bool allowCorpse = true, bool allowDispenserFull = true, bool allowDispenserEmpty = true, bool allowForbidden = false, bool allowSociallyImproper = false, bool allowHarvest = false, bool forceScanWholeMap = false, bool ignoreReservations = false, bool calculateWantedStackCount = false, FoodPreferability minPrefOverride = FoodPreferability.Undefined, float? minNutrition = null)
		{
			foodDef = null;
			bool getterCanManipulate = getter.RaceProps.ToolUser && getter.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			if (!getterCanManipulate && getter != eater)
			{
				Log.Error(string.Concat(new object[]
				{
					getter,
					" tried to find food to bring to ",
					eater,
					" but ",
					getter,
					" is incapable of Manipulation."
				}));
				return null;
			}
			FoodPreferability minPref;
			if (minPrefOverride == FoodPreferability.Undefined)
			{
				if (eater.NonHumanlikeOrWildMan())
				{
					minPref = FoodPreferability.NeverForNutrition;
				}
				else if (desperate)
				{
					minPref = FoodPreferability.DesperateOnly;
				}
				else
				{
					minPref = ((eater.needs.food.CurCategory >= HungerCategory.UrgentlyHungry) ? FoodPreferability.RawBad : FoodPreferability.MealAwful);
				}
			}
			else
			{
				minPref = minPrefOverride;
			}
			Predicate<Thing> foodValidator = delegate(Thing t)
			{
				Building_NutrientPasteDispenser building_NutrientPasteDispenser = t as Building_NutrientPasteDispenser;
				if (building_NutrientPasteDispenser != null)
				{
					if (!allowDispenserFull || !getterCanManipulate || ThingDefOf.MealNutrientPaste.ingestible.preferability < minPref || ThingDefOf.MealNutrientPaste.ingestible.preferability > maxPref || !eater.WillEat(ThingDefOf.MealNutrientPaste, getter, true) || (t.Faction != getter.Faction && t.Faction != getter.HostFaction) || (!allowForbidden && t.IsForbidden(getter)) || (!building_NutrientPasteDispenser.powerComp.PowerOn || (!allowDispenserEmpty && !building_NutrientPasteDispenser.HasEnoughFeedstockInHoppers())) || !t.InteractionCell.Standable(t.Map) || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || !getter.Map.reachability.CanReachNonLocal(getter.Position, new TargetInfo(t.InteractionCell, t.Map, false), PathEndMode.OnCell, TraverseParms.For(getter, Danger.Some, TraverseMode.ByPawn, false, false, false)))
					{
						return false;
					}
				}
				else
				{
					int stackCount = 1;
					float statValue = t.GetStatValue(StatDefOf.Nutrition, true);
					if (minNutrition != null)
					{
						stackCount = FoodUtility.StackCountForNutrition(minNutrition.Value, statValue);
					}
					else if (calculateWantedStackCount)
					{
						stackCount = FoodUtility.WillIngestStackCountOf(eater, t.def, statValue);
					}
					if (t.def.ingestible.preferability < minPref || t.def.ingestible.preferability > maxPref || !eater.WillEat(t, getter, true) || !t.def.IsNutritionGivingIngestible || !t.IngestibleNow || (!allowCorpse && t is Corpse) || (!allowDrug && t.def.IsDrug) || (!allowForbidden && t.IsForbidden(getter)) || (!desperate && t.IsNotFresh()) || (t.IsDessicated() || !FoodUtility.IsFoodSourceOnMapSociallyProper(t, getter, eater, allowSociallyImproper) || (!getter.AnimalAwareOf(t) && !forceScanWholeMap)) || (!ignoreReservations && !getter.CanReserve(t, 10, stackCount, null, false)))
					{
						return false;
					}
				}
				return true;
			};
			ThingRequest thingRequest;
			if ((eater.RaceProps.foodType & (FoodTypeFlags.Plant | FoodTypeFlags.Tree)) > FoodTypeFlags.None && allowPlant)
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSource);
			}
			else
			{
				thingRequest = ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree);
			}
			Thing bestThing;
			if (getter.RaceProps.Humanlike)
			{
				bestThing = FoodUtility.SpawnedFoodSearchInnerScan(eater, getter.Position, getter.Map.listerThings.ThingsMatching(thingRequest), PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, foodValidator);
				if (allowHarvest & getterCanManipulate)
				{
					int searchRegionsMax;
					if (forceScanWholeMap && bestThing == null)
					{
						searchRegionsMax = -1;
					}
					else
					{
						searchRegionsMax = 30;
					}
					Thing thing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, ThingRequest.ForGroup(ThingRequestGroup.HarvestablePlant), PathEndMode.Touch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, delegate(Thing x)
					{
						Plant plant = (Plant)x;
						if (!plant.HarvestableNow)
						{
							return false;
						}
						ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
						return harvestedThingDef.IsNutritionGivingIngestible && eater.WillEat(harvestedThingDef, getter, true) && getter.CanReserve(plant, 1, -1, null, false) && (allowForbidden || !plant.IsForbidden(getter)) && (bestThing == null || FoodUtility.GetFinalIngestibleDef(bestThing, false).ingestible.preferability < harvestedThingDef.ingestible.preferability);
					}, null, 0, searchRegionsMax, false, RegionType.Set_Passable, false);
					if (thing != null)
					{
						bestThing = thing;
						foodDef = FoodUtility.GetFinalIngestibleDef(thing, true);
					}
				}
				if (foodDef == null && bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			else
			{
				int maxRegionsToScan = FoodUtility.GetMaxRegionsToScan(getter, forceScanWholeMap);
				FoodUtility.filtered.Clear();
				foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(getter.Position, getter.Map, 2f, true))
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && pawn != getter && pawn.RaceProps.Animal && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest && pawn.CurJob.GetTarget(TargetIndex.A).HasThing)
					{
						FoodUtility.filtered.Add(pawn.CurJob.GetTarget(TargetIndex.A).Thing);
					}
				}
				bool ignoreEntirelyForbiddenRegions = !allowForbidden && ForbidUtility.CaresAboutForbidden(getter, true) && getter.playerSettings != null && getter.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap != null;
				Predicate<Thing> validator = (Thing t) => foodValidator(t) && !FoodUtility.filtered.Contains(t) && (t is Building_NutrientPasteDispenser || t.def.ingestible.preferability > FoodPreferability.DesperateOnly) && !t.IsNotFresh();
				bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, validator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				FoodUtility.filtered.Clear();
				if (bestThing == null)
				{
					desperate = true;
					bestThing = GenClosest.ClosestThingReachable(getter.Position, getter.Map, thingRequest, PathEndMode.ClosestTouch, TraverseParms.For(getter, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, foodValidator, null, 0, maxRegionsToScan, false, RegionType.Set_Passable, ignoreEntirelyForbiddenRegions);
				}
				if (bestThing != null)
				{
					foodDef = FoodUtility.GetFinalIngestibleDef(bestThing, false);
				}
			}
			return bestThing;
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x0011B20C File Offset: 0x0011940C
		private static int GetMaxRegionsToScan(Pawn getter, bool forceScanWholeMap)
		{
			if (getter.RaceProps.Humanlike)
			{
				return -1;
			}
			if (forceScanWholeMap)
			{
				return -1;
			}
			if (getter.Faction == Faction.OfPlayer)
			{
				return 100;
			}
			return 30;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x0011B234 File Offset: 0x00119434
		private static bool IsFoodSourceOnMapSociallyProper(Thing t, Pawn getter, Pawn eater, bool allowSociallyImproper)
		{
			if (!allowSociallyImproper)
			{
				bool animalsCare = !getter.RaceProps.Animal;
				if (!t.IsSociallyProper(getter) && !t.IsSociallyProper(eater, eater.IsPrisonerOfColony, animalsCare))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x0011B270 File Offset: 0x00119470
		public static float FoodOptimality(Pawn eater, Thing foodSource, ThingDef foodDef, float dist, bool takingToInventory = false)
		{
			float num = 300f;
			num -= dist;
			switch (foodDef.ingestible.preferability)
			{
			case FoodPreferability.NeverForNutrition:
				return -9999999f;
			case FoodPreferability.DesperateOnly:
				num -= 150f;
				break;
			case FoodPreferability.DesperateOnlyForHumanlikes:
				if (eater.RaceProps.Humanlike)
				{
					num -= 150f;
				}
				break;
			}
			CompRottable compRottable = foodSource.TryGetComp<CompRottable>();
			if (compRottable != null)
			{
				if (compRottable.Stage == RotStage.Dessicated)
				{
					return -9999999f;
				}
				if (!takingToInventory && compRottable.Stage == RotStage.Fresh && compRottable.TicksUntilRotAtCurrentTemp < 30000)
				{
					num += 12f;
				}
			}
			if (eater.needs != null && eater.needs.mood != null)
			{
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(eater, foodSource, foodDef);
				for (int i = 0; i < list.Count; i++)
				{
					num += FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].thought.stages[0].baseMoodEffect);
				}
			}
			if (foodDef.ingestible != null)
			{
				if (eater.RaceProps.Humanlike)
				{
					num += foodDef.ingestible.optimalityOffsetHumanlikes;
				}
				else if (eater.RaceProps.Animal)
				{
					num += foodDef.ingestible.optimalityOffsetFeedingAnimals;
				}
			}
			if (eater.story != null && eater.story.traits.AnyTraitHasIngestibleOverrides)
			{
				List<Trait> allTraits = eater.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					List<IngestibleModifiers> ingestibleModifiers = allTraits[j].CurrentData.ingestibleModifiers;
					if (!ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
					{
						for (int k = 0; k < ingestibleModifiers.Count; k++)
						{
							if (ingestibleModifiers[k].ingestible == foodDef)
							{
								num += ingestibleModifiers[k].optimalityOffset;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x0011B444 File Offset: 0x00119644
		private static Thing SpawnedFoodSearchInnerScan(Pawn eater, IntVec3 root, List<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null)
		{
			if (searchSet == null)
			{
				return null;
			}
			Pawn pawn = traverseParams.pawn ?? eater;
			int num = 0;
			int num2 = 0;
			Thing result = null;
			float num3 = float.MinValue;
			for (int i = 0; i < searchSet.Count; i++)
			{
				Thing thing = searchSet[i];
				num2++;
				float num4 = (float)(root - thing.Position).LengthManhattan;
				if (num4 <= maxDistance)
				{
					float num5 = FoodUtility.FoodOptimality(eater, thing, FoodUtility.GetFinalIngestibleDef(thing, false), num4, false);
					if (num5 >= num3 && pawn.Map.reachability.CanReach(root, thing, peMode, traverseParams) && thing.Spawned && (validator == null || validator(thing)))
					{
						result = thing;
						num3 = num5;
						num++;
					}
				}
			}
			return result;
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x0011B51C File Offset: 0x0011971C
		public static void DebugFoodSearchFromMouse_Update()
		{
			IntVec3 root = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Thing thing = FoodUtility.SpawnedFoodSearchInnerScan(pawn, root, Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false), 9999f, null);
			if (thing != null)
			{
				GenDraw.DrawLineBetween(root.ToVector3Shifted(), thing.Position.ToVector3Shifted());
			}
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x0011B598 File Offset: 0x00119798
		public static void DebugFoodSearchFromMouse_OnGUI()
		{
			IntVec3 a = UI.MouseCell();
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
				float num = FoodUtility.FoodOptimality(pawn, thing, finalIngestibleDef, (a - thing.Position).LengthHorizontal, false);
				Vector2 vector = thing.DrawPos.MapToUIPosition();
				Rect rect = new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f);
				string text = num.ToString("F0");
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(pawn, thing, finalIngestibleDef);
				for (int i = 0; i < list.Count; i++)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						list[i].thought.defName,
						"(",
						FoodUtility.FoodOptimalityEffectFromMoodCurve.Evaluate(list[i].thought.stages[0].baseMoodEffect).ToString("F0"),
						")"
					});
				}
				Widgets.Label(rect, text);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x0011B750 File Offset: 0x00119950
		private static Pawn BestPawnToHuntForPredator(Pawn predator, bool forceScanWholeMap)
		{
			if (predator.meleeVerbs.TryGetMeleeVerb(null) == null)
			{
				return null;
			}
			bool flag = false;
			if (predator.health.summaryHealth.SummaryHealthPercent < 0.25f)
			{
				flag = true;
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			if (FoodUtility.GetMaxRegionsToScan(predator, forceScanWholeMap) < 0)
			{
				FoodUtility.tmpPredatorCandidates.AddRange(predator.Map.mapPawns.AllPawnsSpawned);
			}
			else
			{
				TraverseParms traverseParms = TraverseParms.For(predator, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
				RegionTraverser.BreadthFirstTraverse(predator.Position, predator.Map, (Region from, Region to) => to.Allows(traverseParms, true), delegate(Region x)
				{
					List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
					for (int j = 0; j < list.Count; j++)
					{
						FoodUtility.tmpPredatorCandidates.Add((Pawn)list[j]);
					}
					return false;
				}, 999999, RegionType.Set_Passable);
			}
			Pawn pawn = null;
			float num = 0f;
			bool tutorialMode = TutorSystem.TutorialMode;
			for (int i = 0; i < FoodUtility.tmpPredatorCandidates.Count; i++)
			{
				Pawn pawn2 = FoodUtility.tmpPredatorCandidates[i];
				if (predator.GetRoom(RegionType.Set_All) == pawn2.GetRoom(RegionType.Set_All) && predator != pawn2 && (!flag || pawn2.Downed) && FoodUtility.IsAcceptablePreyFor(predator, pawn2) && predator.CanReach(pawn2, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn2.IsForbidden(predator) && (!tutorialMode || pawn2.Faction != Faction.OfPlayer))
				{
					float preyScoreFor = FoodUtility.GetPreyScoreFor(predator, pawn2);
					if (preyScoreFor > num || pawn == null)
					{
						num = preyScoreFor;
						pawn = pawn2;
					}
				}
			}
			FoodUtility.tmpPredatorCandidates.Clear();
			return pawn;
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x0011B8D0 File Offset: 0x00119AD0
		public static bool IsAcceptablePreyFor(Pawn predator, Pawn prey)
		{
			if (!prey.RaceProps.canBePredatorPrey)
			{
				return false;
			}
			if (!prey.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.predatorsHuntHumanlikes && prey.RaceProps.Humanlike)
			{
				return false;
			}
			if (prey.BodySize > predator.RaceProps.maxPreyBodySize)
			{
				return false;
			}
			if (!prey.Downed)
			{
				if (prey.kindDef.combatPower > 2f * predator.kindDef.combatPower)
				{
					return false;
				}
				float num = prey.kindDef.combatPower * prey.health.summaryHealth.SummaryHealthPercent * prey.ageTracker.CurLifeStage.bodySizeFactor;
				float num2 = predator.kindDef.combatPower * predator.health.summaryHealth.SummaryHealthPercent * predator.ageTracker.CurLifeStage.bodySizeFactor;
				if (num >= num2)
				{
					return false;
				}
			}
			return (predator.Faction == null || prey.Faction == null || predator.HostileTo(prey)) && (predator.Faction == null || prey.HostFaction == null || predator.HostileTo(prey)) && (predator.Faction != Faction.OfPlayer || prey.Faction != Faction.OfPlayer) && (!predator.RaceProps.herdAnimal || predator.def != prey.def);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x0011BA2C File Offset: 0x00119C2C
		public static float GetPreyScoreFor(Pawn predator, Pawn prey)
		{
			float num = prey.kindDef.combatPower / predator.kindDef.combatPower;
			float num2 = prey.health.summaryHealth.SummaryHealthPercent;
			float bodySizeFactor = prey.ageTracker.CurLifeStage.bodySizeFactor;
			float lengthHorizontal = (predator.Position - prey.Position).LengthHorizontal;
			if (prey.Downed)
			{
				num2 = Mathf.Min(num2, 0.2f);
			}
			float num3 = -lengthHorizontal - 56f * num2 * num2 * num * bodySizeFactor;
			if (prey.RaceProps.Humanlike)
			{
				num3 -= 35f;
			}
			else if (FoodUtility.IsPreyProtectedFromPredatorByFence(predator, prey))
			{
				num3 -= 17f;
			}
			return num3;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x0011BADC File Offset: 0x00119CDC
		private static bool IsPreyProtectedFromPredatorByFence(Pawn predator, Pawn prey)
		{
			if (predator.GetDistrict(RegionType.Set_Passable) == prey.GetDistrict(RegionType.Set_Passable))
			{
				return false;
			}
			TraverseParms traverseParams = TraverseParms.For(predator, Danger.Deadly, TraverseMode.ByPawn, false, false, false).WithFenceblocked(true);
			return !predator.Map.reachability.CanReach(predator.Position, prey.Position, PathEndMode.ClosestTouch, traverseParams);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x0011BB38 File Offset: 0x00119D38
		public static void DebugDrawPredatorFoodSource()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				return;
			}
			Thing thing;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, false, false, true, false, false, false, false, false, FoodPreferability.Undefined))
			{
				GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), thing.Position.ToVector3Shifted());
				if (!(thing is Pawn))
				{
					Pawn pawn2 = FoodUtility.BestPawnToHuntForPredator(pawn, true);
					if (pawn2 != null)
					{
						GenDraw.DrawLineBetween(pawn.Position.ToVector3Shifted(), pawn2.Position.ToVector3Shifted());
					}
				}
			}
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x0011BBCC File Offset: 0x00119DCC
		public static List<FoodUtility.ThoughtFromIngesting> ThoughtsFromIngesting(Pawn ingester, Thing foodSource, ThingDef foodDef)
		{
			FoodUtility.ingestThoughts.Clear();
			FoodUtility.extraIngestThoughtsFromTraits.Clear();
			if (ingester.needs == null || ingester.needs.mood == null)
			{
				return FoodUtility.ingestThoughts;
			}
			MeatSourceCategory meatSourceCategory;
			if (foodSource.def.IsCorpse)
			{
				meatSourceCategory = FoodUtility.GetMeatSourceCategoryFromCorpse(foodSource);
			}
			else
			{
				meatSourceCategory = FoodUtility.GetMeatSourceCategory(foodDef);
			}
			Pawn_StoryTracker story = ingester.story;
			if (story != null)
			{
				TraitSet traits = story.traits;
				if (traits != null)
				{
					traits.GetExtraThoughtsFromIngestion(FoodUtility.extraIngestThoughtsFromTraits, foodDef, meatSourceCategory, true);
				}
			}
			if (!ingester.story.traits.HasTrait(TraitDefOf.Ascetic) && foodDef.ingestible.tasteThought != null)
			{
				FoodUtility.TryAddIngestThought(ingester, foodDef.ingestible.tasteThought, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			CompIngredients compIngredients = foodSource.TryGetComp<CompIngredients>();
			Building_NutrientPasteDispenser building_NutrientPasteDispenser = foodSource as Building_NutrientPasteDispenser;
			for (int i = 0; i < FoodUtility.extraIngestThoughtsFromTraits.Count; i++)
			{
				FoodUtility.TryAddIngestThought(ingester, FoodUtility.extraIngestThoughtsFromTraits[i], null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (compIngredients != null)
			{
				bool flag = false;
				bool flag2 = false;
				for (int j = 0; j < compIngredients.ingredients.Count; j++)
				{
					bool flag3;
					bool flag4;
					FoodUtility.AddIngestThoughtsFromIngredient(compIngredients.ingredients[j], ingester, FoodUtility.ingestThoughts, out flag3, out flag4);
					if (flag3)
					{
						flag = true;
					}
					if (flag4)
					{
						flag2 = true;
					}
				}
				if (ModsConfig.IdeologyActive && flag2 && !flag)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonFungusMealWithPlants, ingester, foodDef, meatSourceCategory);
				}
			}
			else if (building_NutrientPasteDispenser != null)
			{
				Thing thing = building_NutrientPasteDispenser.FindFeedInAnyHopper();
				if (thing != null)
				{
					bool flag5;
					bool flag6;
					FoodUtility.AddIngestThoughtsFromIngredient(thing.def, ingester, FoodUtility.ingestThoughts, out flag5, out flag6);
				}
			}
			if (foodDef.ingestible.specialThoughtDirect != null)
			{
				FoodUtility.TryAddIngestThought(ingester, foodDef.ingestible.specialThoughtDirect, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (foodSource.IsNotFresh())
			{
				FoodUtility.TryAddIngestThought(ingester, ThoughtDefOf.AteRottenFood, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (ModsConfig.RoyaltyActive && FoodUtility.InappropriateForTitle(foodDef, ingester, false))
			{
				FoodUtility.TryAddIngestThought(ingester, ThoughtDefOf.AteFoodInappropriateForTitle, null, FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
			}
			if (ingester.Ideo != null)
			{
				if (FoodUtility.IsHumanlikeMeatOrHumanlikeCorpse(foodSource, foodDef))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeat, ingester, foodDef, meatSourceCategory);
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeatDirect, ingester, foodDef, meatSourceCategory);
				}
				else if ((!foodDef.IsDrug || foodDef.ingestible.drugCategory != DrugCategory.Medical) && foodDef.ingestible.CachedNutrition > 0f)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonCannibalFood, ingester, foodDef, meatSourceCategory);
				}
				if (FoodUtility.IsConsideredMeatIfIngested(foodSource) || FoodUtility.IsConsideredMeatIfIngested(foodDef))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteMeat, ingester, foodDef, meatSourceCategory);
				}
				else if (!foodDef.IsDrug && foodDef.ingestible.CachedNutrition > 0f)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonMeat, ingester, foodDef, meatSourceCategory);
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(foodDef, ingester, foodSource))
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester, foodDef, meatSourceCategory);
				}
				if (meatSourceCategory == MeatSourceCategory.Insect)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteInsectMeatDirect, ingester, foodDef, meatSourceCategory);
				}
				if (ModsConfig.IdeologyActive && foodDef.thingCategories != null && foodDef.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
				{
					if (foodDef.IsFungus)
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteFungus, ingester, foodDef, meatSourceCategory);
					}
					else
					{
						FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteNonFungusPlant, ingester, foodDef, meatSourceCategory);
					}
				}
				if (foodDef.ingestible.ateEvent != null)
				{
					FoodUtility.AddThoughtsFromIdeo(foodDef.ingestible.ateEvent, ingester, foodDef, meatSourceCategory);
				}
			}
			return FoodUtility.ingestThoughts;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x0011BEF8 File Offset: 0x0011A0F8
		private static void AddIngestThoughtsFromIngredient(ThingDef ingredient, Pawn ingester, List<FoodUtility.ThoughtFromIngesting> ingestThoughts, out bool ateFungus, out bool ateNonFungusRawPlant)
		{
			ateFungus = false;
			ateNonFungusRawPlant = false;
			if (ingredient.ingestible == null)
			{
				return;
			}
			MeatSourceCategory meatSourceCategory = FoodUtility.GetMeatSourceCategory(ingredient);
			FoodUtility.extraIngestThoughtsFromTraits.Clear();
			Pawn_StoryTracker story = ingester.story;
			if (story != null)
			{
				TraitSet traits = story.traits;
				if (traits != null)
				{
					traits.GetExtraThoughtsFromIngestion(FoodUtility.extraIngestThoughtsFromTraits, ingredient, meatSourceCategory, false);
				}
			}
			for (int i = 0; i < FoodUtility.extraIngestThoughtsFromTraits.Count; i++)
			{
				FoodUtility.TryAddIngestThought(ingester, FoodUtility.extraIngestThoughtsFromTraits[i], null, ingestThoughts, ingredient, meatSourceCategory);
			}
			if (ingredient.ingestible.specialThoughtAsIngredient != null)
			{
				FoodUtility.TryAddIngestThought(ingester, ingredient.ingestible.specialThoughtAsIngredient, null, ingestThoughts, ingredient, meatSourceCategory);
			}
			if (meatSourceCategory == MeatSourceCategory.Humanlike)
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeat, ingester, ingredient, meatSourceCategory);
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteHumanMeatAsIngredient, ingester, ingredient, meatSourceCategory);
			}
			else if (FoodUtility.IsVeneratedAnimalMeatOrCorpse(ingredient, ingester, null))
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester, ingredient, meatSourceCategory);
			}
			if (meatSourceCategory == MeatSourceCategory.Insect)
			{
				FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteInsectMeatAsIngredient, ingester, ingredient, meatSourceCategory);
			}
			if (ModsConfig.IdeologyActive && ingredient.thingCategories != null && ingredient.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
			{
				if (ingredient.IsFungus)
				{
					FoodUtility.AddThoughtsFromIdeo(HistoryEventDefOf.AteFungusAsIngredient, ingester, ingredient, meatSourceCategory);
					ateFungus = true;
					return;
				}
				ateNonFungusRawPlant = true;
			}
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x0011C01C File Offset: 0x0011A21C
		private static void TryAddIngestThought(Pawn ingester, ThoughtDef def, Precept fromPrecept, List<FoodUtility.ThoughtFromIngesting> ingestThoughts, ThingDef foodDef, MeatSourceCategory meatSourceCategory)
		{
			FoodUtility.ThoughtFromIngesting item = new FoodUtility.ThoughtFromIngesting
			{
				thought = def,
				fromPrecept = fromPrecept
			};
			if (ingester.story != null && ingester.story.traits != null)
			{
				if (!ingester.story.traits.IsThoughtFromIngestionDisallowed(def, foodDef, meatSourceCategory))
				{
					ingestThoughts.Add(item);
					return;
				}
			}
			else
			{
				ingestThoughts.Add(item);
			}
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x0011C080 File Offset: 0x0011A280
		private static void AddThoughtsFromIdeo(HistoryEventDef eventDef, Pawn ingester, ThingDef foodDef, MeatSourceCategory meatSourceCategory)
		{
			if (ingester.Ideo == null)
			{
				return;
			}
			List<Precept> preceptsListForReading = ingester.Ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				List<PreceptComp> comps = preceptsListForReading[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought;
					if ((preceptComp_SelfTookMemoryThought = (comps[j] as PreceptComp_SelfTookMemoryThought)) != null && preceptComp_SelfTookMemoryThought.eventDef == eventDef)
					{
						FoodUtility.TryAddIngestThought(ingester, preceptComp_SelfTookMemoryThought.thought, preceptsListForReading[i], FoodUtility.ingestThoughts, foodDef, meatSourceCategory);
					}
				}
			}
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0011C10C File Offset: 0x0011A30C
		public static bool IsHumanlikeMeatOrHumanlikeCorpse(Thing source, ThingDef foodDef)
		{
			if (source.def.IsCorpse)
			{
				return FoodUtility.GetMeatSourceCategoryFromCorpse(source) == MeatSourceCategory.Humanlike;
			}
			return FoodUtility.GetMeatSourceCategory(foodDef) == MeatSourceCategory.Humanlike;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x0011C130 File Offset: 0x0011A330
		public static bool IsVeneratedAnimalMeatOrCorpse(ThingDef foodDef, Pawn ingester, Thing source = null)
		{
			if (ingester.Ideo == null)
			{
				return false;
			}
			if (source != null && source.def.IsCorpse)
			{
				return ingester.Ideo.IsVeneratedAnimal(((Corpse)source).InnerPawn);
			}
			return foodDef.IsMeat && ingester.Ideo.IsVeneratedAnimal(foodDef.ingestible.sourceDef);
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x0011C190 File Offset: 0x0011A390
		public static MeatSourceCategory GetMeatSourceCategory(ThingDef source)
		{
			if ((source.ingestible.foodType & FoodTypeFlags.Meat) != FoodTypeFlags.Meat)
			{
				return MeatSourceCategory.NotMeat;
			}
			if (source.ingestible.sourceDef != null && source.ingestible.sourceDef.race != null && source.ingestible.sourceDef.race.Humanlike)
			{
				return MeatSourceCategory.Humanlike;
			}
			if (source.ingestible.sourceDef != null && source.ingestible.sourceDef.race.FleshType != null && source.ingestible.sourceDef.race.FleshType == FleshTypeDefOf.Insectoid)
			{
				return MeatSourceCategory.Insect;
			}
			return MeatSourceCategory.Undefined;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x0011C22C File Offset: 0x0011A42C
		private static MeatSourceCategory GetMeatSourceCategoryFromCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse == null)
			{
				return MeatSourceCategory.NotMeat;
			}
			if (corpse.InnerPawn.RaceProps.Humanlike)
			{
				return MeatSourceCategory.Humanlike;
			}
			if (corpse.InnerPawn.RaceProps.Insect)
			{
				return MeatSourceCategory.Insect;
			}
			return MeatSourceCategory.Undefined;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x0011C26E File Offset: 0x0011A46E
		public static bool HasMeatEatingRequiredPrecept(this Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Disapproved) || ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Horrible) || ideo.HasPrecept(PreceptDefOf.MeatEating_NonMeat_Abhorrent);
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x0011C297 File Offset: 0x0011A497
		public static bool HasHumanMeatEatingRequiredPrecept(this Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.Cannibalism_Preferred) || ideo.HasPrecept(PreceptDefOf.Cannibalism_RequiredRavenous) || ideo.HasPrecept(PreceptDefOf.Cannibalism_RequiredStrong);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x0011C2C0 File Offset: 0x0011A4C0
		public static void GenerateGoodIngredients(Thing meal, Ideo ideo)
		{
			CompIngredients compIngredients = meal.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				compIngredients.ingredients.Clear();
				if (ideo.HasMeatEatingRequiredPrecept())
				{
					compIngredients.ingredients.Add((from d in DefDatabase<ThingDef>.AllDefsListForReading
					where d.race != null && d.race.IsFlesh && d.race.Animal
					select d.race.meatDef).RandomElement<ThingDef>());
				}
				if (ideo.HasHumanMeatEatingRequiredPrecept())
				{
					compIngredients.ingredients.Add(ThingDefOf.Meat_Human);
				}
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0011C364 File Offset: 0x0011A564
		public static int WillIngestStackCountOf(Pawn ingester, ThingDef def, float singleFoodNutrition)
		{
			int num = Mathf.Min(def.ingestible.maxNumToIngestAtOnce, FoodUtility.StackCountForNutrition(ingester.needs.food.NutritionWanted, singleFoodNutrition));
			if (num < 1)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0011C39F File Offset: 0x0011A59F
		public static float GetBodyPartNutrition(Corpse corpse, BodyPartRecord part)
		{
			return FoodUtility.GetBodyPartNutrition(corpse.GetStatValue(StatDefOf.Nutrition, true), corpse.InnerPawn, part);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x0011C3BC File Offset: 0x0011A5BC
		public static float GetBodyPartNutrition(float currentCorpseNutrition, Pawn pawn, BodyPartRecord part)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			float coverageOfNotMissingNaturalParts = hediffSet.GetCoverageOfNotMissingNaturalParts(pawn.RaceProps.body.corePart);
			if (coverageOfNotMissingNaturalParts <= 0f)
			{
				return 0f;
			}
			float num = hediffSet.GetCoverageOfNotMissingNaturalParts(part) / coverageOfNotMissingNaturalParts;
			return currentCorpseNutrition * num;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x0011C407 File Offset: 0x0011A607
		public static int StackCountForNutrition(float wantedNutrition, float singleFoodNutrition)
		{
			if (wantedNutrition <= 0.0001f)
			{
				return 0;
			}
			return Mathf.Max(Mathf.RoundToInt(wantedNutrition / singleFoodNutrition), 1);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0011C421 File Offset: 0x0011A621
		public static bool ShouldBeFedBySomeone(Pawn pawn)
		{
			return FeedPatientUtility.ShouldBeFed(pawn) || WardenFeedUtility.ShouldBeFed(pawn);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x0011C434 File Offset: 0x0011A634
		public static void AddFoodPoisoningHediff(Pawn pawn, Thing ingestible, FoodPoisonCause cause)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.FoodPoisoning, false);
			if (firstHediffOfDef != null)
			{
				if (firstHediffOfDef.CurStageIndex != 2)
				{
					firstHediffOfDef.Severity = HediffDefOf.FoodPoisoning.stages[2].minSeverity - 0.001f;
				}
			}
			else
			{
				pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.FoodPoisoning, pawn, null), null, null, null);
			}
			if (PawnUtility.ShouldSendNotificationAbout(pawn) && MessagesRepeatAvoider.MessageShowAllowed("MessageFoodPoisoning-" + pawn.thingIDNumber, 0.1f))
			{
				Messages.Message("MessageFoodPoisoning".Translate(pawn.LabelShort, ingestible.LabelCapNoCount, cause.ToStringHuman().CapitalizeFirst(), pawn.Named("PAWN"), ingestible.Named("FOOD")).CapitalizeFirst(), pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x0011C53C File Offset: 0x0011A73C
		public static float GetFoodPoisonChanceFactor(Pawn ingester)
		{
			float num = Find.Storyteller.difficulty.foodPoisonChanceFactor;
			if (ingester.health != null && ingester.health.hediffSet != null)
			{
				foreach (Hediff hediff in ingester.health.hediffSet.hediffs)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null)
					{
						num *= curStage.foodPoisoningChanceFactor;
					}
				}
			}
			return num;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x0011C5CC File Offset: 0x0011A7CC
		public static bool TryGetFoodPoisoningChanceOverrideFromTraits(Pawn pawn, Thing ingestible, out float poisonChanceOverride)
		{
			if (pawn.story != null && pawn.story.traits.AnyTraitHasIngestibleOverrides)
			{
				List<Trait> allTraits = pawn.story.traits.allTraits;
				for (int i = 0; i < allTraits.Count; i++)
				{
					List<IngestibleModifiers> ingestibleModifiers = allTraits[i].CurrentData.ingestibleModifiers;
					if (!ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
					{
						for (int j = 0; j < ingestibleModifiers.Count; j++)
						{
							if (ingestibleModifiers[j].ingestible == ingestible.def)
							{
								poisonChanceOverride = ingestibleModifiers[j].poisonChanceOverride;
								return true;
							}
						}
					}
				}
			}
			poisonChanceOverride = 0f;
			return false;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x0011C66F File Offset: 0x0011A86F
		public static bool Starving(this Pawn p)
		{
			return p.needs != null && p.needs.food != null && p.needs.food.Starving;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x0011C698 File Offset: 0x0011A898
		public static float GetNutrition(Thing foodSource, ThingDef foodDef)
		{
			if (foodSource == null || foodDef == null)
			{
				return 0f;
			}
			if (foodSource.def == foodDef)
			{
				return foodSource.GetStatValue(StatDefOf.Nutrition, true);
			}
			return foodDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x0011C6C8 File Offset: 0x0011A8C8
		public static bool WillIngestFromInventoryNow(Pawn pawn, Thing inv)
		{
			return (inv.def.IsNutritionGivingIngestible || inv.def.IsNonMedicalDrug) && inv.IngestibleNow && pawn.WillEat(inv, null, true);
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x0011C6F8 File Offset: 0x0011A8F8
		public static void IngestFromInventoryNow(Pawn pawn, Thing inv)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, inv);
			job.count = Mathf.Min(inv.stackCount, FoodUtility.WillIngestStackCountOf(pawn, inv.def, inv.GetStatValue(StatDefOf.Nutrition, true)));
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x0011C754 File Offset: 0x0011A954
		public static bool IsConsideredMeatIfIngested(Thing food)
		{
			if (FoodUtility.IsConsideredMeatIfIngested(food.def))
			{
				return true;
			}
			CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				for (int i = 0; i < compIngredients.ingredients.Count; i++)
				{
					if (FoodUtility.IsConsideredMeatIfIngested(compIngredients.ingredients[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x0011C7A8 File Offset: 0x0011A9A8
		public static bool IsConsideredMeatIfIngested(ThingDef foodDef)
		{
			return foodDef.IsMeat || foodDef.IsEgg || (foodDef.ingestible != null && (foodDef.ingestible.foodType.HasFlag(FoodTypeFlags.Meat) || foodDef.ingestible.foodType.HasFlag(FoodTypeFlags.Corpse)));
		}

		// Token: 0x04001D18 RID: 7448
		public const int FoodPoisoningStageInitial = 2;

		// Token: 0x04001D19 RID: 7449
		public const int FoodPoisoningStageMajor = 1;

		// Token: 0x04001D1A RID: 7450
		public const int FoodPoisoningStageRecovering = 0;

		// Token: 0x04001D1B RID: 7451
		private static HashSet<Thing> filtered = new HashSet<Thing>();

		// Token: 0x04001D1C RID: 7452
		private static readonly SimpleCurve FoodOptimalityEffectFromMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(-100f, -600f),
				true
			},
			{
				new CurvePoint(-10f, -100f),
				true
			},
			{
				new CurvePoint(-5f, -70f),
				true
			},
			{
				new CurvePoint(-1f, -50f),
				true
			},
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(100f, 800f),
				true
			}
		};

		// Token: 0x04001D1D RID: 7453
		private static List<Pawn> tmpPredatorCandidates = new List<Pawn>();

		// Token: 0x04001D1E RID: 7454
		private static List<FoodUtility.ThoughtFromIngesting> ingestThoughts = new List<FoodUtility.ThoughtFromIngesting>();

		// Token: 0x04001D1F RID: 7455
		private static List<ThoughtDef> extraIngestThoughtsFromTraits = new List<ThoughtDef>();

		// Token: 0x02001DE0 RID: 7648
		public struct ThoughtFromIngesting
		{
			// Token: 0x040072B6 RID: 29366
			public ThoughtDef thought;

			// Token: 0x040072B7 RID: 29367
			public Precept fromPrecept;
		}
	}
}
