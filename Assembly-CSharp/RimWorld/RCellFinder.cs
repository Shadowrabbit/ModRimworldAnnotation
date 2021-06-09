using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001C13 RID: 7187
	public static class RCellFinder
	{
		// Token: 0x06009E21 RID: 40481 RVA: 0x00069412 File Offset: 0x00067612
		public static IntVec3 BestOrderedGotoDestNear(IntVec3 root, Pawn searcher)
		{
			return RCellFinder.BestOrderedGotoDestNear_NewTemp(root, searcher, null);
		}

		// Token: 0x06009E22 RID: 40482 RVA: 0x002E4BD4 File Offset: 0x002E2DD4
		public static IntVec3 BestOrderedGotoDestNear_NewTemp(IntVec3 root, Pawn searcher, Predicate<IntVec3> cellValidator = null)
		{
			Map map = searcher.Map;
			Predicate<IntVec3> predicate = delegate(IntVec3 c)
			{
				if (cellValidator != null && !cellValidator(c))
				{
					return false;
				}
				if (!map.pawnDestinationReservationManager.CanReserve(c, searcher, true) || !c.Standable(map) || !searcher.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && pawn != searcher && pawn.RaceProps.Humanlike && ((searcher.Faction == Faction.OfPlayer && pawn.Faction == searcher.Faction) || (searcher.Faction != Faction.OfPlayer && pawn.Faction != Faction.OfPlayer)))
					{
						return false;
					}
				}
				return true;
			};
			if (predicate(root))
			{
				return root;
			}
			int num = 1;
			IntVec3 result = default(IntVec3);
			float num2 = -1000f;
			bool flag = false;
			int num3 = GenRadial.NumCellsInRadius(30f);
			do
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[num];
				if (predicate(intVec))
				{
					float num4 = CoverUtility.TotalSurroundingCoverScore(intVec, map);
					if (num4 > num2)
					{
						num2 = num4;
						result = intVec;
						flag = true;
					}
				}
				if (num >= 8 && flag)
				{
					return result;
				}
				num++;
			}
			while (num < num3);
			return searcher.Position;
		}

		// Token: 0x06009E23 RID: 40483 RVA: 0x002E4C9C File Offset: 0x002E2E9C
		public static bool TryFindBestExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			if (mode == TraverseMode.PassAllDestroyableThings && !pawn.Map.reachability.CanReachMapEdge(pawn.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, true)))
			{
				return RCellFinder.TryFindRandomPawnEntryCell(out spot, pawn.Map, 0f, true, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, mode));
			}
			int num = 0;
			int num2 = 0;
			IntVec3 intVec2;
			for (;;)
			{
				num2++;
				if (num2 > 30)
				{
					break;
				}
				IntVec3 intVec;
				bool flag = CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, num, null, out intVec, -1);
				num += 4;
				if (flag)
				{
					int num3 = intVec.x;
					intVec2 = new IntVec3(0, 0, intVec.z);
					if (pawn.Map.Size.z - intVec.z < num3)
					{
						num3 = pawn.Map.Size.z - intVec.z;
						intVec2 = new IntVec3(intVec.x, 0, pawn.Map.Size.z - 1);
					}
					if (pawn.Map.Size.x - intVec.x < num3)
					{
						num3 = pawn.Map.Size.x - intVec.x;
						intVec2 = new IntVec3(pawn.Map.Size.x - 1, 0, intVec.z);
					}
					if (intVec.z < num3)
					{
						intVec2 = new IntVec3(intVec.x, 0, 0);
					}
					if (intVec2.Standable(pawn.Map) && pawn.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, true, mode))
					{
						goto Block_9;
					}
				}
			}
			spot = pawn.Position;
			return false;
			Block_9:
			spot = intVec2;
			return true;
		}

		// Token: 0x06009E24 RID: 40484 RVA: 0x002E4E98 File Offset: 0x002E3098
		public static bool TryFindRandomExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			Danger maxDanger = Danger.Some;
			int num = 0;
			IntVec3 intVec;
			for (;;)
			{
				num++;
				if (num > 40)
				{
					break;
				}
				if (num > 15)
				{
					maxDanger = Danger.Deadly;
				}
				intVec = CellFinder.RandomCell(pawn.Map);
				int num2 = Rand.RangeInclusive(0, 3);
				if (num2 == 0)
				{
					intVec.x = 0;
				}
				if (num2 == 1)
				{
					intVec.x = pawn.Map.Size.x - 1;
				}
				if (num2 == 2)
				{
					intVec.z = 0;
				}
				if (num2 == 3)
				{
					intVec.z = pawn.Map.Size.z - 1;
				}
				if (intVec.Standable(pawn.Map) && pawn.CanReach(intVec, PathEndMode.OnCell, maxDanger, false, mode))
				{
					goto Block_8;
				}
			}
			spot = pawn.Position;
			return false;
			Block_8:
			spot = intVec;
			return true;
		}

		// Token: 0x06009E25 RID: 40485 RVA: 0x002E4F58 File Offset: 0x002E3158
		public static bool TryFindExitSpotNear(Pawn pawn, IntVec3 near, float radius, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			return (mode == TraverseMode.PassAllDestroyableThings && CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out spot)) || CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, mode), out spot);
		}

		// Token: 0x06009E26 RID: 40486 RVA: 0x002E4FC8 File Offset: 0x002E31C8
		public static IntVec3 RandomWanderDestFor(Pawn pawn, IntVec3 root, float radius, Func<Pawn, IntVec3, IntVec3, bool> validator, Danger maxDanger)
		{
			if (radius > 12f)
			{
				Log.Warning(string.Concat(new object[]
				{
					"wanderRadius of ",
					radius,
					" is greater than Region.GridSize of ",
					12,
					" and will break."
				}), false);
			}
			bool flag = false;
			if (root.GetRegion(pawn.Map, RegionType.Set_Passable) != null)
			{
				int maxRegions = Mathf.Max((int)radius / 3, 13);
				CellFinder.AllRegionsNear(RCellFinder.regions, root.GetRegion(pawn.Map, RegionType.Set_Passable), maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region reg) => reg.extentsClose.ClosestDistSquaredTo(root) <= radius * radius, null, RegionType.Set_Passable);
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(root, 0.6f, "root", 50);
				}
				if (RCellFinder.regions.Count > 0)
				{
					for (int i = 0; i < 35; i++)
					{
						IntVec3 intVec = IntVec3.Invalid;
						for (int j = 0; j < 5; j++)
						{
							IntVec3 randomCell = RCellFinder.regions.RandomElementByWeight((Region reg) => (float)reg.CellCount).RandomCell;
							if ((float)randomCell.DistanceToSquared(root) <= radius * radius)
							{
								intVec = randomCell;
								break;
							}
						}
						if (!intVec.IsValid)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.32f, "distance", 50);
							}
						}
						else
						{
							if (RCellFinder.CanWanderToCell(intVec, pawn, root, validator, i, maxDanger))
							{
								if (flag)
								{
									pawn.Map.debugDrawer.FlashCell(intVec, 0.9f, "go!", 50);
								}
								RCellFinder.regions.Clear();
								return intVec;
							}
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.6f, "validation", 50);
							}
						}
					}
				}
				RCellFinder.regions.Clear();
			}
			IntVec3 position;
			if (!CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn) && (validator == null || validator(pawn, c, root)), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 20, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 30, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 5, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1))
			{
				position = pawn.Position;
			}
			if (flag)
			{
				pawn.Map.debugDrawer.FlashCell(position, 0.4f, "fallback", 50);
			}
			return position;
		}

		// Token: 0x06009E27 RID: 40487 RVA: 0x002E5378 File Offset: 0x002E3578
		private static bool CanWanderToCell(IntVec3 c, Pawn pawn, IntVec3 root, Func<Pawn, IntVec3, IntVec3, bool> validator, int tryIndex, Danger maxDanger)
		{
			bool flag = false;
			if (!c.Walkable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0f, "walk", 50);
				}
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "forbid", 50);
				}
				return false;
			}
			if (tryIndex < 10 && !c.Standable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "stand", 50);
				}
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.6f, "reach", 50);
				}
				return false;
			}
			if (PawnUtility.KnownDangerAt(c, pawn.Map, pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.1f, "trap", 50);
				}
				return false;
			}
			if (tryIndex < 10)
			{
				if (c.GetTerrain(pawn.Map).avoidWander)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.39f, "terr", 50);
					}
					return false;
				}
				if (pawn.Map.pathGrid.PerceivedPathCostAt(c) > 20)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "pcost", 50);
					}
					return false;
				}
				if (c.GetDangerFor(pawn, pawn.Map) > Danger.None)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "danger", 50);
					}
					return false;
				}
			}
			else if (tryIndex < 15 && c.GetDangerFor(pawn, pawn.Map) == Danger.Deadly)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.4f, "deadly", 50);
				}
				return false;
			}
			if (!pawn.Map.pawnDestinationReservationManager.CanReserve(c, pawn, false))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.75f, "resvd", 50);
				}
				return false;
			}
			if (validator != null && !validator(pawn, c, root))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.15f, "valid", 50);
				}
				return false;
			}
			if (c.GetDoor(pawn.Map) != null)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.32f, "door", 50);
				}
				return false;
			}
			if (c.ContainsStaticFire(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.9f, "fire", 50);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009E28 RID: 40488 RVA: 0x002E5628 File Offset: 0x002E3828
		public static bool TryFindGoodAdjacentSpotToTouch(Pawn toucher, Thing touchee, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (intVec.Standable(toucher.Map) && !PawnUtility.KnownDangerAt(intVec, toucher.Map, toucher))
				{
					result = intVec;
					return true;
				}
			}
			foreach (IntVec3 intVec2 in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (intVec2.Walkable(toucher.Map))
				{
					result = intVec2;
					return true;
				}
			}
			result = touchee.Position;
			return false;
		}

		// Token: 0x06009E29 RID: 40489 RVA: 0x002E5700 File Offset: 0x002E3900
		public static bool TryFindRandomPawnEntryCell(out IntVec3 result, Map map, float roadChance, bool allowFogged = false, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(map) && !map.roofGrid.Roofed(c) && map.reachability.CanReachColony(c) && c.GetRoom(map, RegionType.Set_Passable).TouchesMapEdge && (allowFogged || !c.Fogged(map)) && (extraValidator == null || extraValidator(c)), map, roadChance, out result);
		}

		// Token: 0x06009E2A RID: 40490 RVA: 0x002E5744 File Offset: 0x002E3944
		public static bool TryFindPrisonerReleaseCell(Pawn prisoner, Pawn warden, out IntVec3 result)
		{
			if (prisoner.Map != warden.Map)
			{
				result = IntVec3.Invalid;
				return false;
			}
			Region region = prisoner.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				result = default(IntVec3);
				return false;
			}
			TraverseParms traverseParms = TraverseParms.For(warden, Danger.Deadly, TraverseMode.ByPawn, false);
			bool needMapEdge = prisoner.Faction != warden.Faction;
			IntVec3 foundResult = IntVec3.Invalid;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (needMapEdge)
				{
					if (!r.Room.TouchesMapEdge)
					{
						return false;
					}
				}
				else if (r.Room.isPrisonCell)
				{
					return false;
				}
				foundResult = r.RandomCell;
				return true;
			};
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), regionProcessor, 999, RegionType.Set_Passable);
			if (foundResult.IsValid)
			{
				result = foundResult;
				return true;
			}
			result = default(IntVec3);
			return false;
		}

		// Token: 0x06009E2B RID: 40491 RVA: 0x002E5800 File Offset: 0x002E3A00
		public static IntVec3 RandomAnimalSpawnCell_MapGen(Map map)
		{
			int numStand = 0;
			int numRoom = 0;
			int numTouch = 0;
			IntVec3 intVec;
			if (!CellFinderLoose.TryGetRandomCellWith(delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					int num = numStand;
					numStand = num + 1;
					return false;
				}
				if (c.GetTerrain(map).avoidWander)
				{
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room == null)
				{
					int num = numRoom;
					numRoom = num + 1;
					return false;
				}
				if (!room.TouchesMapEdge)
				{
					int num = numTouch;
					numTouch = num + 1;
					return false;
				}
				return true;
			}, map, 1000, out intVec))
			{
				intVec = CellFinder.RandomCell(map);
				Log.Warning(string.Concat(new object[]
				{
					"RandomAnimalSpawnCell_MapGen failed: numStand=",
					numStand,
					", numRoom=",
					numRoom,
					", numTouch=",
					numTouch,
					". PlayerStartSpot=",
					MapGenerator.PlayerStartSpot,
					". Returning ",
					intVec
				}), false);
			}
			return intVec;
		}

		// Token: 0x06009E2C RID: 40492 RVA: 0x002E58DC File Offset: 0x002E3ADC
		public static bool TryFindSkygazeCell(IntVec3 root, Pawn searcher, out IntVec3 result)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 c) => !c.Roofed(searcher.Map) && !c.GetTerrain(searcher.Map).avoidWander;
			Predicate<Region> validator = delegate(Region r)
			{
				IntVec3 intVec;
				return r.Room.PsychologicallyOutdoors && !r.IsForbiddenEntirely(searcher) && r.TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out intVec);
			};
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
			Region root2;
			if (!CellFinder.TryFindClosestRegionWith(root.GetRegion(searcher.Map, RegionType.Set_Passable), traverseParms, validator, 300, out root2, RegionType.Set_Passable))
			{
				result = root;
				return false;
			}
			return CellFinder.RandomRegionNear(root2, 14, traverseParms, validator, searcher, RegionType.Set_Passable).TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out result);
		}

		// Token: 0x06009E2D RID: 40493 RVA: 0x002E5974 File Offset: 0x002E3B74
		public static bool TryFindTravelDestFrom(IntVec3 root, Map map, out IntVec3 travelDest)
		{
			travelDest = root;
			bool flag = false;
			Predicate<IntVec3> cellValidator = (IntVec3 c) => map.reachability.CanReach(root, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.None) && !map.roofGrid.Roofed(c);
			if (root.x == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == map.Size.x - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.x == map.Size.x - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == map.Size.z - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == map.Size.z - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 10000 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 2500 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			return flag;
		}

		// Token: 0x06009E2E RID: 40494 RVA: 0x0006941C File Offset: 0x0006761C
		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 originCell, Map map, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, null, out result, null);
		}

		// Token: 0x06009E2F RID: 40495 RVA: 0x00069428 File Offset: 0x00067628
		public static bool TryFindRandomSpotJustOutsideColony(Pawn searcher, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(searcher.Position, searcher.Map, searcher, out result, null);
		}

		// Token: 0x06009E30 RID: 40496 RVA: 0x002E5AD4 File Offset: 0x002E3CD4
		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 root, Map map, Pawn searcher, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			bool desperate = false;
			int minColonyBuildingsLOS = 0;
			int walkRadius = 0;
			int walkRadiusMaxImpassable = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (!room.PsychologicallyOutdoors || !room.TouchesMapEdge)
				{
					return false;
				}
				if (room == null || room.CellCount < 60)
				{
					return false;
				}
				if (root.IsValid)
				{
					TraverseParms traverseParams = (searcher != null) ? TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false) : TraverseMode.PassDoors;
					if (!map.reachability.CanReach(root, c, PathEndMode.Touch, traverseParams))
					{
						return false;
					}
				}
				if (!desperate && !map.reachability.CanReachColony(c))
				{
					return false;
				}
				if (extraValidator != null && !extraValidator(c))
				{
					return false;
				}
				int num = 0;
				foreach (IntVec3 loc in CellRect.CenteredOn(c, walkRadius))
				{
					Room room2 = loc.GetRoom(map, RegionType.Set_Passable);
					if (room2 != room)
					{
						num++;
						if (!desperate && room2 != null && room2.IsDoorway)
						{
							return false;
						}
					}
					if (num > walkRadiusMaxImpassable)
					{
						return false;
					}
				}
				if (minColonyBuildingsLOS > 0)
				{
					int colonyBuildingsLOSFound = 0;
					RCellFinder.tmpBuildings.Clear();
					RegionTraverser.BreadthFirstTraverse(c, map, (Region from, Region to) => true, delegate(Region reg)
					{
						Faction ofPlayer = Faction.OfPlayer;
						List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						for (int l = 0; l < list.Count; l++)
						{
							Thing thing = list[l];
							if (thing.Faction == ofPlayer && thing.Position.InHorDistOf(c, 16f) && GenSight.LineOfSight(thing.Position, c, map, true, null, 0, 0) && !RCellFinder.tmpBuildings.Contains(thing))
							{
								RCellFinder.tmpBuildings.Add(thing);
								int colonyBuildingsLOSFound = colonyBuildingsLOSFound;
								colonyBuildingsLOSFound++;
								if (colonyBuildingsLOSFound >= minColonyBuildingsLOS)
								{
									return true;
								}
							}
						}
						return false;
					}, 12, RegionType.Set_Passable);
					RCellFinder.tmpBuildings.Clear();
					if (colonyBuildingsLOSFound < minColonyBuildingsLOS)
					{
						return false;
					}
				}
				return true;
			};
			IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
			where b.def.building.ai_chillDestination
			select b;
			for (int i = 0; i < 120; i++)
			{
				Building building = null;
				if (!source.TryRandomElement(out building))
				{
					break;
				}
				desperate = (i > 60);
				walkRadius = 6 - i / 20;
				walkRadiusMaxImpassable = 6 - i / 20;
				minColonyBuildingsLOS = 5 - i / 30;
				if (CellFinder.TryFindRandomCellNear(building.Position, map, 10, validator, out result, 50))
				{
					return true;
				}
			}
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int j = 0; j < 120; j++)
			{
				Building building2 = null;
				if (!allBuildingsColonist.TryRandomElement(out building2))
				{
					break;
				}
				desperate = (j > 60);
				walkRadius = 6 - j / 20;
				walkRadiusMaxImpassable = 6 - j / 20;
				minColonyBuildingsLOS = 4 - j / 30;
				if (CellFinder.TryFindRandomCellNear(building2.Position, map, 15, validator, out result, 50))
				{
					return true;
				}
			}
			for (int k = 0; k < 50; k++)
			{
				Pawn pawn = null;
				if (!map.mapPawns.FreeColonistsAndPrisonersSpawned.TryRandomElement(out pawn))
				{
					break;
				}
				desperate = (k > 25);
				walkRadius = 3;
				walkRadiusMaxImpassable = 6;
				minColonyBuildingsLOS = 0;
				if (CellFinder.TryFindRandomCellNear(pawn.Position, map, 15, validator, out result, 50))
				{
					return true;
				}
			}
			desperate = true;
			walkRadius = 3;
			walkRadiusMaxImpassable = 6;
			minColonyBuildingsLOS = 0;
			return CellFinderLoose.TryGetRandomCellWith(validator, map, 1000, out result);
		}

		// Token: 0x06009E31 RID: 40497 RVA: 0x002E5CE4 File Offset: 0x002E3EE4
		public static bool TryFindRandomCellInRegionUnforbidden(this Region reg, Pawn pawn, Predicate<IntVec3> validator, out IntVec3 result)
		{
			if (reg == null)
			{
				throw new ArgumentNullException("reg");
			}
			if (reg.IsForbiddenEntirely(pawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			return reg.TryFindRandomCellInRegion((IntVec3 c) => !c.IsForbidden(pawn) && (validator == null || validator(c)), out result);
		}

		// Token: 0x06009E32 RID: 40498 RVA: 0x002E5D44 File Offset: 0x002E3F44
		public static bool TryFindDirectFleeDestination(IntVec3 root, float dist, Pawn pawn, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				result = root + IntVec3.FromVector3(Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * dist);
				if (result.Walkable(pawn.Map) && result.DistanceToSquared(pawn.Position) < result.DistanceToSquared(root) && GenSight.LineOfSight(root, result, pawn.Map, true, null, 0, 0))
				{
					return true;
				}
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int j = 0; j < 30; j++)
			{
				IntVec3 randomCell = CellFinder.RandomRegionNear(region, 15, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
				if (randomCell.Walkable(pawn.Map) && (float)(root - randomCell).LengthHorizontalSquared > dist * dist)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, randomCell, pawn, PathEndMode.OnCell))
					{
						if (PawnPathUtility.TryFindCellAtIndex(pawnPath, (int)dist + 3, out result))
						{
							return true;
						}
					}
				}
			}
			result = pawn.Position;
			return false;
		}

		// Token: 0x06009E33 RID: 40499 RVA: 0x002E5E8C File Offset: 0x002E408C
		public static bool TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(IntVec3 pos, Map map, float minDistToColony, out IntVec3 result)
		{
			int num = 30;
			CellRect cellRect = CellRect.CenteredOn(map.Center, num);
			cellRect.ClipInsideMap(map);
			List<IntVec3> list = new List<IntVec3>();
			if (minDistToColony > 0f)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					list.Add(pawn.Position);
				}
				foreach (Building building in map.listerBuildings.allBuildingsColonist)
				{
					list.Add(building.Position);
				}
			}
			float num2 = minDistToColony * minDistToColony;
			int num3 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num3++;
				if (num3 > 50)
				{
					if (num > map.Size.x)
					{
						goto IL_16C;
					}
					num = (int)((float)num * 1.5f);
					cellRect = CellRect.CenteredOn(map.Center, num);
					cellRect.ClipInsideMap(map);
					num3 = 0;
				}
				randomCell = cellRect.RandomCell;
				if (randomCell.Standable(map) && map.reachability.CanReach(randomCell, pos, PathEndMode.ClosestTouch, TraverseMode.NoPassClosedDoors, Danger.Deadly))
				{
					bool flag = false;
					for (int i = 0; i < list.Count; i++)
					{
						if ((float)(list[i] - randomCell).LengthHorizontalSquared < num2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			result = randomCell;
			return true;
			IL_16C:
			result = pos;
			return false;
		}

		// Token: 0x06009E34 RID: 40500 RVA: 0x002E602C File Offset: 0x002E422C
		public static bool TryFindRandomCellNearTheCenterOfTheMapWith(Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			int startingSearchRadius = Mathf.Clamp(Mathf.Max(map.Size.x, map.Size.z) / 20, 3, 25);
			return RCellFinder.TryFindRandomCellNearWith(map.Center, validator, map, out result, startingSearchRadius, int.MaxValue);
		}

		// Token: 0x06009E35 RID: 40501 RVA: 0x002E6074 File Offset: 0x002E4274
		public static bool TryFindRandomCellNearWith(IntVec3 near, Predicate<IntVec3> validator, Map map, out IntVec3 result, int startingSearchRadius = 5, int maxSearchRadius = 2147483647)
		{
			int num = startingSearchRadius;
			CellRect cellRect = CellRect.CenteredOn(near, num);
			cellRect.ClipInsideMap(map);
			int num2 = 0;
			IntVec3 randomCell;
			do
			{
				num2++;
				if (num2 > 30)
				{
					if (num >= maxSearchRadius || (num > map.Size.x * 2 && num > map.Size.z * 2))
					{
						goto IL_82;
					}
					num = Mathf.Min((int)((float)num * 1.5f), maxSearchRadius);
					cellRect = CellRect.CenteredOn(near, num);
					cellRect.ClipInsideMap(map);
					num2 = 0;
				}
				randomCell = cellRect.RandomCell;
			}
			while (!validator(randomCell));
			result = randomCell;
			return true;
			IL_82:
			result = near;
			return false;
		}

		// Token: 0x06009E36 RID: 40502 RVA: 0x002E610C File Offset: 0x002E430C
		public static IntVec3 SpotToChewStandingNear(Pawn pawn, Thing ingestible)
		{
			IntVec3 root = pawn.Position;
			Room rootRoom = pawn.GetRoom(RegionType.Set_Passable);
			bool desperate = false;
			bool ignoreDanger = false;
			float maxDist = 4f;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				IntVec3 intVec2 = root - c;
				if ((float)intVec2.LengthHorizontalSquared > maxDist * maxDist)
				{
					return false;
				}
				if (pawn.HostFaction != null && c.GetRoom(pawn.Map, RegionType.Set_Passable) != rootRoom)
				{
					return false;
				}
				if (!desperate)
				{
					if (!c.Standable(pawn.Map))
					{
						return false;
					}
					if (GenPlace.HaulPlaceBlockerIn(null, c, pawn.Map, false) != null)
					{
						return false;
					}
					if (c.GetRegion(pawn.Map, RegionType.Set_Passable).type == RegionType.Portal)
					{
						return false;
					}
				}
				return (ignoreDanger || c.GetDangerFor(pawn, pawn.Map) == Danger.None) && !c.ContainsStaticFire(pawn.Map) && !c.ContainsTrap(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(c, pawn, false) && Toils_Ingest.TryFindAdjacentIngestionPlaceSpot(c, ingestible.def, pawn, out intVec2);
			};
			int maxRegions = 1;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int i = 0; i < 30; i++)
			{
				if (i == 1)
				{
					desperate = true;
				}
				else if (i == 2)
				{
					desperate = false;
					maxRegions = 4;
				}
				else if (i == 6)
				{
					desperate = true;
				}
				else if (i == 10)
				{
					desperate = false;
					maxDist = 8f;
					maxRegions = 12;
				}
				else if (i == 15)
				{
					desperate = true;
				}
				else if (i == 20)
				{
					maxDist = 15f;
					maxRegions = 16;
				}
				else if (i == 26)
				{
					maxDist = 5f;
					maxRegions = 4;
					ignoreDanger = true;
				}
				else if (i == 29)
				{
					maxDist = 15f;
					maxRegions = 16;
				}
				IntVec3 intVec;
				if (CellFinder.RandomRegionNear(region, maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).TryFindRandomCellInRegionUnforbidden(pawn, validator, out intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						pawn.Map.debugDrawer.FlashCell(intVec, 0.5f, "go!", 50);
					}
					return intVec;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					pawn.Map.debugDrawer.FlashCell(intVec, 0f, i.ToString(), 50);
				}
			}
			return region.RandomCell;
		}

		// Token: 0x06009E37 RID: 40503 RVA: 0x002E62C8 File Offset: 0x002E44C8
		public static bool TryFindMarriageSite(Pawn firstFiance, Pawn secondFiance, out IntVec3 result)
		{
			if (!firstFiance.CanReach(secondFiance, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			Map map = firstFiance.Map;
			if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MarriageSpot)
			where MarriageSpotUtility.IsValidMarriageSpotFor(x.Position, firstFiance, secondFiance, null)
			select x.Position).TryRandomElement(out result))
			{
				return true;
			}
			Predicate<IntVec3> noMarriageSpotValidator = delegate(IntVec3 cell)
			{
				IntVec3 c = cell + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
				if (!c.InBounds(map))
				{
					return false;
				}
				if (c.IsForbidden(firstFiance) || c.IsForbidden(secondFiance))
				{
					return false;
				}
				if (!c.Standable(map))
				{
					return false;
				}
				Room room = cell.GetRoom(map, RegionType.Set_Passable);
				return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
			};
			foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
					if (MarriageSpotUtility.IsValidMarriageSpotFor(intVec, firstFiance, secondFiance, null) && noMarriageSpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(firstFiance.Position, firstFiance.Map, 25, (IntVec3 cell) => MarriageSpotUtility.IsValidMarriageSpotFor(cell, firstFiance, secondFiance, null) && noMarriageSpotValidator(cell), out result, -1))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E38 RID: 40504 RVA: 0x002E6470 File Offset: 0x002E4670
		public static bool TryFindGatheringSpot_NewTemp(Pawn organizer, GatheringDef gatheringDef, bool ignoreRequiredColonistCount, out IntVec3 result)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			Predicate<IntVec3> baseValidator = (IntVec3 cell) => GatheringsUtility.ValidateGatheringSpot_NewTemp(cell, gatheringDef, organizer, enjoyableOutside, ignoreRequiredColonistCount);
			List<ThingDef> gatherSpotDefs = gatheringDef.gatherSpotDefs;
			try
			{
				foreach (ThingDef def in gatherSpotDefs)
				{
					foreach (Building item in map.listerBuildings.AllBuildingsColonistOfDef(def))
					{
						RCellFinder.tmpSpotThings.Add(item);
					}
				}
				if ((from x in RCellFinder.tmpSpotThings
				where baseValidator(x.Position)
				select x.Position).TryRandomElement(out result))
				{
					return true;
				}
			}
			finally
			{
				RCellFinder.tmpSpotThings.Clear();
			}
			Predicate<IntVec3> noPartySpotValidator = delegate(IntVec3 cell)
			{
				Room room = cell.GetRoom(map, RegionType.Set_Passable);
				return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
			};
			foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
					if (baseValidator(intVec) && noPartySpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(organizer.Position, organizer.Map, 25, (IntVec3 cell) => baseValidator(cell) && noPartySpotValidator(cell), out result, -1))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E39 RID: 40505 RVA: 0x0006943E File Offset: 0x0006763E
		[Obsolete("Will be removed in a future update")]
		public static bool TryFindGatheringSpot(Pawn organizer, GatheringDef gatheringDef, out IntVec3 result)
		{
			return RCellFinder.TryFindGatheringSpot_NewTemp(organizer, gatheringDef, false, out result);
		}

		// Token: 0x06009E3A RID: 40506 RVA: 0x00069449 File Offset: 0x00067649
		[Obsolete]
		public static IntVec3 FindSiegePositionFrom(IntVec3 entrySpot, Map map)
		{
			return RCellFinder.FindSiegePositionFrom_NewTemp(entrySpot, map, false);
		}

		// Token: 0x06009E3B RID: 40507 RVA: 0x002E66C0 File Offset: 0x002E48C0
		public static IntVec3 FindSiegePositionFrom_NewTemp(IntVec3 entrySpot, Map map, bool allowRoofed = false)
		{
			if (!entrySpot.IsValid)
			{
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
				{
					intVec = CellFinder.RandomCell(map);
				}
				Log.Error("Tried to find a siege position from an invalid cell. Using " + intVec, false);
				return intVec;
			}
			IntVec3 result;
			for (int i = 70; i >= 20; i -= 10)
			{
				if (RCellFinder.TryFindSiegePosition(entrySpot, (float)i, map, allowRoofed, out result))
				{
					return result;
				}
			}
			if (RCellFinder.TryFindSiegePosition(entrySpot, 100f, map, allowRoofed, out result))
			{
				return result;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not find siege spot from ",
				entrySpot,
				", using ",
				entrySpot
			}), false);
			return entrySpot;
		}

		// Token: 0x06009E3C RID: 40508 RVA: 0x002E6798 File Offset: 0x002E4998
		private static bool TryFindSiegePosition(IntVec3 entrySpot, float minDistToColony, Map map, bool allowRoofed, out IntVec3 result)
		{
			CellRect cellRect = CellRect.CenteredOn(entrySpot, 60);
			cellRect.ClipInsideMap(map);
			cellRect = cellRect.ContractedBy(14);
			List<IntVec3> list = new List<IntVec3>();
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				list.Add(pawn.Position);
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonistCombatTargets)
			{
				list.Add(building.Position);
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num2++;
				if (num2 > 200)
				{
					goto IL_1E1;
				}
				randomCell = cellRect.RandomCell;
				if (randomCell.Standable(map) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light) && map.reachability.CanReach(randomCell, entrySpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some) && map.reachability.CanReachColony(randomCell))
				{
					bool flag = false;
					for (int i = 0; i < list.Count; i++)
					{
						if ((float)(list[i] - randomCell).LengthHorizontalSquared < num)
						{
							flag = true;
							break;
						}
					}
					if (!flag && (allowRoofed || !randomCell.Roofed(map)))
					{
						int num3 = 0;
						foreach (IntVec3 intVec in CellRect.CenteredOn(randomCell, 10).ClipInsideMap(map))
						{
							if (randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
							{
								num3++;
							}
						}
						if (num3 >= 35)
						{
							break;
						}
					}
				}
			}
			result = randomCell;
			return true;
			IL_1E1:
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E3D RID: 40509 RVA: 0x002E69BC File Offset: 0x002E4BBC
		public static bool TryFindEdgeCellWithPathToPositionAvoidingColony(IntVec3 target, float minDistToColony, float minDistanceToTarget, Map map, out IntVec3 result)
		{
			bool flag = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				RCellFinder.tmpSpotsToAvoid.Add(building.Position);
			}
			foreach (Pawn pawn in map.mapPawns.FreeColonistsAndPrisonersSpawned)
			{
				RCellFinder.tmpSpotsToAvoid.Add(pawn.Position);
			}
			if (flag)
			{
				for (int i = 0; i < RCellFinder.tmpSpotsToAvoid.Count; i++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[i], 1f, null, 50);
				}
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			IntVec3 intVec;
			for (;;)
			{
				num2++;
				if (num2 > 200)
				{
					goto IL_1CE;
				}
				intVec = CellFinder.RandomEdgeCell(map);
				if (!intVec.Roofed(map) && intVec.Standable(map) && map.reachability.CanReach(intVec, target, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some) && !target.InHorDistOf(intVec, minDistanceToTarget))
				{
					bool flag2 = false;
					foreach (IntVec3 b in GenSight.PointsOnLineOfSight(intVec, target))
					{
						for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
						{
							if ((float)(RCellFinder.tmpSpotsToAvoid[j] - b).LengthHorizontalSquared < num)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (flag)
					{
						map.debugDrawer.FlashLine(intVec, target, 50, flag2 ? SimpleColor.Red : SimpleColor.Green);
					}
					if (!flag2)
					{
						break;
					}
				}
			}
			result = intVec;
			return true;
			IL_1CE:
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E3E RID: 40510 RVA: 0x002E6BCC File Offset: 0x002E4DCC
		private static void FindBestAngleAvoidingSpots(IntVec3 position, List<IntVec3> spotsToAvoid, out float bestAngle, out float bestPerimeter)
		{
			if (RCellFinder.tmpSpotsToAvoid.Count == 0)
			{
				bestAngle = (float)Rand.Range(0, 360);
				bestPerimeter = 360f;
				return;
			}
			if (RCellFinder.tmpSpotsToAvoid.Count == 1)
			{
				float angleFlat = (RCellFinder.tmpSpotsToAvoid[0] - position).AngleFlat;
				bestAngle = angleFlat + 180f;
				bestPerimeter = 360f;
				return;
			}
			RCellFinder.tmpSpotsToAvoid.SortBy((IntVec3 s) => (s - position).AngleFlat);
			RCellFinder.tmpSpotsToAvoid.Add(RCellFinder.tmpSpotsToAvoid.First<IntVec3>());
			float num = 0f;
			float num2 = 0f;
			for (int i = 1; i < RCellFinder.tmpSpotsToAvoid.Count; i++)
			{
				IntVec3 intVec = RCellFinder.tmpSpotsToAvoid[i - 1] - position;
				IntVec3 intVec2 = RCellFinder.tmpSpotsToAvoid[i] - position;
				float angleFlat2 = intVec.AngleFlat;
				float angleFlat3 = intVec2.AngleFlat;
				float num3 = Mathf.Abs(angleFlat3 - angleFlat2);
				float num4 = (angleFlat3 < angleFlat2) ? (360f - num3) : num3;
				if (num4 > num2)
				{
					num2 = num4;
					num = intVec.AngleFlat;
				}
			}
			bestAngle = num + num2 / 2f;
			bestPerimeter = num2;
		}

		// Token: 0x06009E3F RID: 40511 RVA: 0x002E6D1C File Offset: 0x002E4F1C
		public static bool TryFindRandomSpotNearAvoidingHostilePawns(Thing thing, Map map, Func<IntVec3, bool> predicate, out IntVec3 result, float maxSearchDistance = 100f, float minDistance = 10f, float maxDistance = 50f, bool avoidColony = true)
		{
			IntVec3 thingPosition = thing.Position;
			bool drawDebug = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].HostileTo(thing) && allPawnsSpawned[i].Position.InHorDistOf(thingPosition, maxSearchDistance))
				{
					RCellFinder.tmpSpotsToAvoid.Add(allPawnsSpawned[i].Position);
				}
			}
			if (avoidColony)
			{
				foreach (Building building in map.listerBuildings.allBuildingsColonist)
				{
					if (building.Position.InHorDistOf(thingPosition, maxSearchDistance))
					{
						RCellFinder.tmpSpotsToAvoid.Add(building.Position);
					}
				}
			}
			if (drawDebug)
			{
				for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[j], 0.2f, null, 50);
				}
			}
			float num;
			float num2;
			RCellFinder.FindBestAngleAvoidingSpots(thingPosition, RCellFinder.tmpSpotsToAvoid, out num, out num2);
			RCellFinder.tmpSpotsToAvoid.Clear();
			Vector3 v = IntVec3.North.ToVector3();
			if (drawDebug)
			{
				Vector3 vect = v.RotatedBy(num) * maxDistance;
				Vector3 vect2 = v.RotatedBy(num + num2) * maxDistance;
				map.debugDrawer.FlashLine(thingPosition, thingPosition + vect.ToIntVec3(), 50, SimpleColor.Red);
				map.debugDrawer.FlashLine(thingPosition, thingPosition + vect2.ToIntVec3(), 50, SimpleColor.Red);
			}
			Func<Vector3, IntVec3> func = delegate(Vector3 direction)
			{
				IntVec3 intVec3 = thingPosition + (direction * minDistance).ToIntVec3();
				IntVec3 intVec4 = intVec3 + (direction * (maxDistance - minDistance)).ToIntVec3();
				if (drawDebug)
				{
					map.debugDrawer.FlashLine(intVec3, intVec4, 50, SimpleColor.White);
				}
				foreach (IntVec3 intVec5 in GenSight.PointsOnLineOfSight(intVec3, intVec4).InRandomOrder(null))
				{
					if (predicate(intVec5))
					{
						return intVec5;
					}
				}
				return IntVec3.Invalid;
			};
			float num3 = num2 / 4f;
			for (float num4 = 0f; num4 < num3; num4 += 5f)
			{
				Vector3 arg = v.RotatedBy(num + num4);
				IntVec3 intVec = func(arg);
				if (intVec.IsValid)
				{
					result = intVec;
					return true;
				}
				if (!Mathf.Approximately(num4, 0f))
				{
					Vector3 arg2 = v.RotatedBy(num - num4);
					IntVec3 intVec2 = func(arg2);
					if (intVec2.IsValid)
					{
						result = intVec2;
						return true;
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009E40 RID: 40512 RVA: 0x002E6FF4 File Offset: 0x002E51F4
		public static bool TryFindEdgeCellFromTargetAvoidingColony(IntVec3 position, Map map, Predicate<IntVec3> predicate, out IntVec3 result)
		{
			bool flag = false;
			RCellFinder.tmpSpotsToAvoid.Clear();
			List<Pawn> freeColonistsAndPrisonersSpawned = map.mapPawns.FreeColonistsAndPrisonersSpawned;
			for (int i = 0; i < freeColonistsAndPrisonersSpawned.Count; i++)
			{
				RCellFinder.tmpSpotsToAvoid.Add(freeColonistsAndPrisonersSpawned[i].Position);
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				RCellFinder.tmpSpotsToAvoid.Add(building.Position);
			}
			if (flag)
			{
				for (int j = 0; j < RCellFinder.tmpSpotsToAvoid.Count; j++)
				{
					map.debugDrawer.FlashCell(RCellFinder.tmpSpotsToAvoid[j], 0.2f, null, 50);
				}
			}
			float num;
			float num2;
			RCellFinder.FindBestAngleAvoidingSpots(position, RCellFinder.tmpSpotsToAvoid, out num, out num2);
			RCellFinder.tmpSpotsToAvoid.Clear();
			Vector3 v = IntVec3.North.ToVector3();
			if (flag)
			{
				Vector3 vect = v.RotatedBy(num) * map.Size.LengthHorizontal;
				Vector3 vect2 = v.RotatedBy(num + num2) * map.Size.LengthHorizontal;
				map.debugDrawer.FlashLine(position, position + vect.ToIntVec3(), 50, SimpleColor.Red);
				map.debugDrawer.FlashLine(position, position + vect2.ToIntVec3(), 50, SimpleColor.Red);
			}
			CellRect cellRect = CellRect.WholeMap(map);
			Vector3 normalized = v.RotatedBy(num).normalized;
			Vector3 vector = position.ToVector3();
			IntVec3 currentIntPosition = vector.ToIntVec3();
			while (!cellRect.IsOnEdge(currentIntPosition))
			{
				vector += normalized;
				currentIntPosition = vector.ToIntVec3();
				if (!cellRect.Contains(currentIntPosition))
				{
					Log.Error(string.Format("Failed to find map edge cell from position {0}", position), false);
					result = IntVec3.Invalid;
					return false;
				}
			}
			RCellFinder.tmpEdgeCells.Clear();
			foreach (IntVec3 item in cellRect.EdgeCells)
			{
				RCellFinder.tmpEdgeCells.Add(item);
			}
			RCellFinder.tmpEdgeCells.SortBy((IntVec3 p) => p.DistanceToSquared(currentIntPosition));
			for (int k = 0; k < RCellFinder.tmpEdgeCells.Count; k++)
			{
				if (predicate(RCellFinder.tmpEdgeCells[k]))
				{
					result = RCellFinder.tmpEdgeCells[k];
					RCellFinder.tmpEdgeCells.Clear();
					return true;
				}
			}
			RCellFinder.tmpEdgeCells.Clear();
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040064C6 RID: 25798
		private static List<Region> regions = new List<Region>();

		// Token: 0x040064C7 RID: 25799
		private static HashSet<Thing> tmpBuildings = new HashSet<Thing>();

		// Token: 0x040064C8 RID: 25800
		private static List<Thing> tmpSpotThings = new List<Thing>();

		// Token: 0x040064C9 RID: 25801
		private static List<IntVec3> tmpSpotsToAvoid = new List<IntVec3>();

		// Token: 0x040064CA RID: 25802
		private static List<IntVec3> tmpEdgeCells = new List<IntVec3>();
	}
}
