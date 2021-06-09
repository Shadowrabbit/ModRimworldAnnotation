using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD9 RID: 7385
	public static class PlayerItemAccessibilityUtility
	{
		// Token: 0x0600A077 RID: 41079 RVA: 0x002EF078 File Offset: 0x002ED278
		public static bool Accessible(ThingDef thing, int count, Map map)
		{
			PlayerItemAccessibilityUtility.CacheAccessibleThings(map.Tile);
			int num = 0;
			for (int i = 0; i < PlayerItemAccessibilityUtility.cachedAccessibleThings.Count; i++)
			{
				if (PlayerItemAccessibilityUtility.cachedAccessibleThings[i].def == thing)
				{
					num += PlayerItemAccessibilityUtility.cachedAccessibleThings[i].stackCount;
				}
			}
			return num >= count;
		}

		// Token: 0x0600A078 RID: 41080 RVA: 0x002EF0D4 File Offset: 0x002ED2D4
		public static bool PossiblyAccessible(ThingDef thing, int count, Map map)
		{
			if (PlayerItemAccessibilityUtility.Accessible(thing, count, map))
			{
				return true;
			}
			PlayerItemAccessibilityUtility.CacheAccessibleThings(map.Tile);
			int num = 0;
			for (int i = 0; i < PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Count; i++)
			{
				if (PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings[i].ThingDef == thing)
				{
					num += PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings[i].Count;
				}
			}
			return num >= count;
		}

		// Token: 0x0600A079 RID: 41081 RVA: 0x0006AEFE File Offset: 0x000690FE
		public static bool PlayerCanMake(ThingDef thing, Map map)
		{
			PlayerItemAccessibilityUtility.CacheAccessibleThings(map.Tile);
			return PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Contains(thing);
		}

		// Token: 0x0600A07A RID: 41082 RVA: 0x002EF144 File Offset: 0x002ED344
		private static void CacheAccessibleThings(int nearTile)
		{
			if (nearTile == PlayerItemAccessibilityUtility.cachedAccessibleThingsForTile && RealTime.frameCount == PlayerItemAccessibilityUtility.cachedAccessibleThingsForFrame)
			{
				return;
			}
			PlayerItemAccessibilityUtility.cachedAccessibleThings.Clear();
			PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Clear();
			PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Clear();
			WorldGrid worldGrid = Find.WorldGrid;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (worldGrid.ApproxDistanceInTiles(nearTile, maps[i].Tile) <= 5f)
				{
					ThingOwnerUtility.GetAllThingsRecursively(maps[i], PlayerItemAccessibilityUtility.tmpThings, false, null);
					PlayerItemAccessibilityUtility.cachedAccessibleThings.AddRange(PlayerItemAccessibilityUtility.tmpThings);
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int j = 0; j < caravans.Count; j++)
			{
				if (caravans[j].IsPlayerControlled && worldGrid.ApproxDistanceInTiles(nearTile, caravans[j].Tile) <= 5f)
				{
					ThingOwnerUtility.GetAllThingsRecursively(caravans[j], PlayerItemAccessibilityUtility.tmpThings, false, null);
					PlayerItemAccessibilityUtility.cachedAccessibleThings.AddRange(PlayerItemAccessibilityUtility.tmpThings);
				}
			}
			for (int k = 0; k < PlayerItemAccessibilityUtility.cachedAccessibleThings.Count; k++)
			{
				Thing thing = PlayerItemAccessibilityUtility.cachedAccessibleThings[k];
				PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(thing.def, thing.stackCount));
				if (GenLeaving.CanBuildingLeaveResources(thing, DestroyMode.Deconstruct))
				{
					List<ThingDefCountClass> list = thing.CostListAdjusted();
					for (int l = 0; l < list.Count; l++)
					{
						int num = Mathf.RoundToInt((float)list[l].count * thing.def.resourcesFractionWhenDeconstructed);
						if (num > 0)
						{
							PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(list[l].thingDef, num));
							PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(list[l].thingDef);
						}
					}
				}
				Plant plant = thing as Plant;
				if (plant != null && (plant.HarvestableNow || plant.HarvestableSoon))
				{
					int num2 = Mathf.RoundToInt(plant.def.plant.harvestYield * Find.Storyteller.difficultyValues.cropYieldFactor);
					if (num2 > 0)
					{
						PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(plant.def.plant.harvestedThingDef, num2));
						PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(plant.def.plant.harvestedThingDef);
					}
				}
				if (!thing.def.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					for (int m = 0; m < thing.def.butcherProducts.Count; m++)
					{
						PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(thing.def.butcherProducts[m]);
						PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(thing.def.butcherProducts[m].thingDef);
					}
				}
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.meatDef != null)
					{
						int num3 = Mathf.RoundToInt(pawn.GetStatValue(StatDefOf.MeatAmount, true));
						if (num3 > 0)
						{
							PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(pawn.RaceProps.meatDef, num3));
							PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(pawn.RaceProps.meatDef);
						}
					}
					if (pawn.RaceProps.leatherDef != null)
					{
						int num4 = GenMath.RoundRandom(pawn.GetStatValue(StatDefOf.LeatherAmount, true));
						if (num4 > 0)
						{
							PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(pawn.RaceProps.leatherDef, num4));
							PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(pawn.RaceProps.leatherDef);
						}
					}
					if (!pawn.RaceProps.Humanlike)
					{
						PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
						if (curKindLifeStage.butcherBodyPart != null)
						{
							PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(curKindLifeStage.butcherBodyPart.thing, 1));
							PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(curKindLifeStage.butcherBodyPart.thing);
						}
					}
				}
				if (thing.def.smeltable)
				{
					List<ThingDefCountClass> list2 = thing.CostListAdjusted();
					for (int n = 0; n < list2.Count; n++)
					{
						if (!list2[n].thingDef.intricate)
						{
							int num5 = Mathf.RoundToInt((float)list2[n].count * 0.25f);
							if (num5 > 0)
							{
								PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(list2[n].thingDef, num5));
								PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(list2[n].thingDef);
							}
						}
					}
				}
				if (thing.def.smeltable && !thing.def.smeltProducts.NullOrEmpty<ThingDefCountClass>())
				{
					for (int num6 = 0; num6 < thing.def.smeltProducts.Count; num6++)
					{
						PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(thing.def.smeltProducts[num6]);
						PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(thing.def.smeltProducts[num6].thingDef);
					}
				}
			}
			int num7 = 0;
			for (int num8 = 0; num8 < PlayerItemAccessibilityUtility.cachedAccessibleThings.Count; num8++)
			{
				Pawn pawn2 = PlayerItemAccessibilityUtility.cachedAccessibleThings[num8] as Pawn;
				if (pawn2 != null && pawn2.IsFreeColonist && !pawn2.Dead && !pawn2.Downed && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Crafting))
				{
					num7++;
				}
			}
			if (num7 > 0)
			{
				PlayerItemAccessibilityUtility.tmpWorkTables.Clear();
				for (int num9 = 0; num9 < PlayerItemAccessibilityUtility.cachedAccessibleThings.Count; num9++)
				{
					Building_WorkTable building_WorkTable = PlayerItemAccessibilityUtility.cachedAccessibleThings[num9] as Building_WorkTable;
					if (building_WorkTable != null && building_WorkTable.Spawned && PlayerItemAccessibilityUtility.tmpWorkTables.Add(building_WorkTable.def))
					{
						List<RecipeDef> allRecipes = building_WorkTable.def.AllRecipes;
						for (int num10 = 0; num10 < allRecipes.Count; num10++)
						{
							if (allRecipes[num10].AvailableNow && allRecipes[num10].AvailableOnNow(building_WorkTable) && allRecipes[num10].products.Any<ThingDefCountClass>() && !allRecipes[num10].PotentiallyMissingIngredients(null, building_WorkTable.Map).Any<ThingDef>())
							{
								ThingDef stuffDef = allRecipes[num10].products[0].thingDef.MadeFromStuff ? GenStuff.DefaultStuffFor(allRecipes[num10].products[0].thingDef) : null;
								float num11 = allRecipes[num10].WorkAmountTotal(stuffDef);
								if (num11 > 0f)
								{
									int num12 = Mathf.FloorToInt((float)(num7 * 60000 * 5) * 0.09f / num11);
									if (num12 > 0)
									{
										for (int num13 = 0; num13 < allRecipes[num10].products.Count; num13++)
										{
											PlayerItemAccessibilityUtility.cachedPossiblyAccessibleThings.Add(new ThingDefCount(allRecipes[num10].products[num13].thingDef, allRecipes[num10].products[num13].count * num12));
											PlayerItemAccessibilityUtility.cachedMakeableItemDefs.Add(allRecipes[num10].products[num13].thingDef);
										}
									}
								}
							}
						}
					}
				}
			}
			PlayerItemAccessibilityUtility.cachedAccessibleThingsForTile = nearTile;
			PlayerItemAccessibilityUtility.cachedAccessibleThingsForFrame = RealTime.frameCount;
		}

		// Token: 0x0600A07B RID: 41083 RVA: 0x002EF8E4 File Offset: 0x002EDAE4
		public static bool PlayerOrQuestRewardHas(ThingFilter thingFilter)
		{
			ThingRequest bestThingRequest = thingFilter.BestThingRequest;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsMatching(bestThingRequest);
				for (int j = 0; j < list.Count; j++)
				{
					if (thingFilter.Allows(list[j]))
					{
						return true;
					}
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int k = 0; k < caravans.Count; k++)
			{
				if (caravans[k].IsPlayerControlled)
				{
					List<Thing> list2 = CaravanInventoryUtility.AllInventoryItems(caravans[k]);
					for (int l = 0; l < list2.Count; l++)
					{
						if (thingFilter.Allows(list2[l]))
						{
							return true;
						}
					}
				}
			}
			List<Site> sites = Find.WorldObjects.Sites;
			for (int m = 0; m < sites.Count; m++)
			{
				for (int n = 0; n < sites[m].parts.Count; n++)
				{
					SitePart sitePart = sites[m].parts[n];
					if (sitePart.things != null)
					{
						for (int num = 0; num < sitePart.things.Count; num++)
						{
							if (thingFilter.Allows(sitePart.things[num]))
							{
								return true;
							}
						}
					}
				}
				DefeatAllEnemiesQuestComp component = sites[m].GetComponent<DefeatAllEnemiesQuestComp>();
				if (component != null)
				{
					ThingOwner rewards = component.rewards;
					for (int num2 = 0; num2 < rewards.Count; num2++)
					{
						if (thingFilter.Allows(rewards[num2]))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A07C RID: 41084 RVA: 0x002EFA98 File Offset: 0x002EDC98
		public static bool PlayerOrQuestRewardHas(ThingDef thingDef, int count = 1)
		{
			if (count <= 0)
			{
				return true;
			}
			int num = 0;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (count == 1)
				{
					if (maps[i].listerThings.ThingsOfDef(thingDef).Count > 0)
					{
						return true;
					}
				}
				else
				{
					num += Mathf.Max(maps[i].resourceCounter.GetCount(thingDef), maps[i].listerThings.ThingsOfDef(thingDef).Count);
					if (num >= count)
					{
						return true;
					}
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int j = 0; j < caravans.Count; j++)
			{
				if (caravans[j].IsPlayerControlled)
				{
					List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravans[j]);
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].def == thingDef)
						{
							num += list[k].stackCount;
							if (num >= count)
							{
								return true;
							}
						}
					}
				}
			}
			List<Site> sites = Find.WorldObjects.Sites;
			for (int l = 0; l < sites.Count; l++)
			{
				for (int m = 0; m < sites[l].parts.Count; m++)
				{
					SitePart sitePart = sites[l].parts[m];
					if (sitePart.things != null)
					{
						for (int n = 0; n < sitePart.things.Count; n++)
						{
							if (sitePart.things[n].def == thingDef)
							{
								num += sitePart.things[n].stackCount;
								if (num >= count)
								{
									return true;
								}
							}
						}
					}
				}
				DefeatAllEnemiesQuestComp component = sites[l].GetComponent<DefeatAllEnemiesQuestComp>();
				if (component != null)
				{
					ThingOwner rewards = component.rewards;
					for (int num2 = 0; num2 < rewards.Count; num2++)
					{
						if (rewards[num2].def == thingDef)
						{
							num += rewards[num2].stackCount;
							if (num >= count)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A07D RID: 41085 RVA: 0x002EFCB0 File Offset: 0x002EDEB0
		public static bool ItemStashHas(ThingDef thingDef)
		{
			List<Site> sites = Find.WorldObjects.Sites;
			for (int i = 0; i < sites.Count; i++)
			{
				Site site = sites[i];
				for (int j = 0; j < site.parts.Count; j++)
				{
					SitePart sitePart = site.parts[j];
					if (sitePart.things != null)
					{
						for (int k = 0; k < sitePart.things.Count; k++)
						{
							if (sitePart.things[k].def == thingDef)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04006CFF RID: 27903
		private static List<Thing> cachedAccessibleThings = new List<Thing>();

		// Token: 0x04006D00 RID: 27904
		private static List<ThingDefCount> cachedPossiblyAccessibleThings = new List<ThingDefCount>();

		// Token: 0x04006D01 RID: 27905
		private static HashSet<ThingDef> cachedMakeableItemDefs = new HashSet<ThingDef>();

		// Token: 0x04006D02 RID: 27906
		private static int cachedAccessibleThingsForTile = -1;

		// Token: 0x04006D03 RID: 27907
		private static int cachedAccessibleThingsForFrame = -1;

		// Token: 0x04006D04 RID: 27908
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04006D05 RID: 27909
		private static HashSet<ThingDef> tmpWorkTables = new HashSet<ThingDef>();

		// Token: 0x04006D06 RID: 27910
		private const float MaxDistanceInTilesToConsiderAccessible = 5f;
	}
}
