using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004AB RID: 1195
	public static class CellFinder
	{
		// Token: 0x06002467 RID: 9319 RVA: 0x000E114E File Offset: 0x000DF34E
		public static IntVec3 RandomCell(Map map)
		{
			return new IntVec3(Rand.Range(0, map.Size.x), 0, Rand.Range(0, map.Size.z));
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000E1178 File Offset: 0x000DF378
		public static IntVec3 RandomEdgeCell(Map map)
		{
			IntVec3 result = default(IntVec3);
			if (Rand.Value < 0.5f)
			{
				if (Rand.Value < 0.5f)
				{
					result.x = 0;
				}
				else
				{
					result.x = map.Size.x - 1;
				}
				result.z = Rand.Range(0, map.Size.z);
			}
			else
			{
				if (Rand.Value < 0.5f)
				{
					result.z = 0;
				}
				else
				{
					result.z = map.Size.z - 1;
				}
				result.x = Rand.Range(0, map.Size.x);
			}
			return result;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000E1220 File Offset: 0x000DF420
		public static IntVec3 RandomEdgeCell(Rot4 dir, Map map)
		{
			if (dir == Rot4.North)
			{
				return new IntVec3(Rand.Range(0, map.Size.x), 0, map.Size.z - 1);
			}
			if (dir == Rot4.South)
			{
				return new IntVec3(Rand.Range(0, map.Size.x), 0, 0);
			}
			if (dir == Rot4.West)
			{
				return new IntVec3(0, 0, Rand.Range(0, map.Size.z));
			}
			if (dir == Rot4.East)
			{
				return new IntVec3(map.Size.x - 1, 0, Rand.Range(0, map.Size.z));
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000E12E4 File Offset: 0x000DF4E4
		public static IntVec3 RandomNotEdgeCell(int minEdgeDistance, Map map)
		{
			if (minEdgeDistance > map.Size.x / 2 || minEdgeDistance > map.Size.z / 2)
			{
				return IntVec3.Invalid;
			}
			int newX = Rand.Range(minEdgeDistance, map.Size.x - minEdgeDistance);
			int newZ = Rand.Range(minEdgeDistance, map.Size.z - minEdgeDistance);
			return new IntVec3(newX, 0, newZ);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000E1348 File Offset: 0x000DF548
		public static bool TryFindClosestRegionWith(Region rootReg, TraverseParms traverseParms, Predicate<Region> validator, int maxRegions, out Region result, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (rootReg == null)
			{
				result = null;
				return false;
			}
			Region localResult = null;
			RegionTraverser.BreadthFirstTraverse(rootReg, (Region from, Region r) => r.Allows(traverseParms, true), delegate(Region r)
			{
				if (validator(r))
				{
					localResult = r;
					return true;
				}
				return false;
			}, maxRegions, traversableRegionTypes);
			result = localResult;
			return result != null;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000E13AC File Offset: 0x000DF5AC
		public static Region RandomRegionNear(Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}
			if (maxRegions <= 1)
			{
				return root;
			}
			CellFinder.workingRegions.Clear();
			RegionTraverser.BreadthFirstTraverse(root, (Region from, Region r) => (validator == null || validator(r)) && r.Allows(traverseParms, true) && (pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)), delegate(Region r)
			{
				CellFinder.workingRegions.Add(r);
				return false;
			}, maxRegions, traversableRegionTypes);
			Region result = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
			CellFinder.workingRegions.Clear();
			return result;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000E145C File Offset: 0x000DF65C
		public static void AllRegionsNear(List<Region> results, Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (results == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid results list", 60733193);
				return;
			}
			results.Clear();
			if (root == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid root", 9107839);
				return;
			}
			RegionTraverser.BreadthFirstTraverse(root, (Region from, Region r) => (validator == null || validator(r)) && r.Allows(traverseParms, true) && (pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)), delegate(Region r)
			{
				results.Add(r);
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000E14E4 File Offset: 0x000DF6E4
		public static bool TryFindRandomReachableCellNear(IntVec3 root, Map map, float radius, TraverseParms traverseParms, Predicate<IntVec3> cellValidator, Predicate<Region> regionValidator, out IntVec3 result, int maxRegions = 999999)
		{
			if (map == null)
			{
				Log.ErrorOnce("Tried to find reachable cell in a null map", 61037855);
				result = IntVec3.Invalid;
				return false;
			}
			Region region = root.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				result = IntVec3.Invalid;
				return false;
			}
			CellFinder.workingRegions.Clear();
			float radSquared = radius * radius;
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, true) && (radius > 1000f || r.extentsClose.ClosestDistSquaredTo(root) <= radSquared) && (regionValidator == null || regionValidator(r)), delegate(Region r)
			{
				CellFinder.workingRegions.Add(r);
				return false;
			}, maxRegions, RegionType.Set_Passable);
			Predicate<IntVec3> <>9__3;
			while (CellFinder.workingRegions.Count > 0)
			{
				Region region2 = CellFinder.workingRegions.RandomElementByWeight((Region r) => (float)r.CellCount);
				Region reg = region2;
				Predicate<IntVec3> validator;
				if ((validator = <>9__3) == null)
				{
					validator = (<>9__3 = ((IntVec3 c) => (float)(c - root).LengthHorizontalSquared <= radSquared && (cellValidator == null || cellValidator(c))));
				}
				if (reg.TryFindRandomCellInRegion(validator, out result))
				{
					CellFinder.workingRegions.Clear();
					return true;
				}
				CellFinder.workingRegions.Remove(region2);
			}
			result = IntVec3.Invalid;
			CellFinder.workingRegions.Clear();
			return false;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000E1640 File Offset: 0x000DF840
		public static IntVec3 RandomClosewalkCellNear(IntVec3 root, Map map, int radius, Predicate<IntVec3> extraValidator = null)
		{
			IntVec3 result;
			if (CellFinder.TryRandomClosewalkCellNear(root, map, radius, out result, extraValidator))
			{
				return result;
			}
			return root;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000E1660 File Offset: 0x000DF860
		public static bool TryRandomClosewalkCellNear(IntVec3 root, Map map, int radius, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false).WithFenceblocked(true), (IntVec3 c) => c.Standable(map) && (extraValidator == null || extraValidator(c)), null, out result, 999999);
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000E16B8 File Offset: 0x000DF8B8
		public static IntVec3 RandomClosewalkCellNearNotForbidden(Pawn pawn, int radius)
		{
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			IntVec3 result;
			if (!CellFinder.TryFindRandomReachableCellNear(position, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false).WithFenceblockedOf(pawn), (IntVec3 c) => !c.IsForbidden(pawn) && c.Standable(map), null, out result, 999999))
			{
				return CellFinder.RandomClosewalkCellNear(position, map, radius, null);
			}
			return result;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000E173C File Offset: 0x000DF93C
		public static bool TryFindRandomCellInRegion(this Region reg, Predicate<IntVec3> validator, out IntVec3 result)
		{
			for (int i = 0; i < 10; i++)
			{
				result = reg.RandomCell;
				if (validator == null || validator(result))
				{
					return true;
				}
			}
			CellFinder.workingCells.Clear();
			CellFinder.workingCells.AddRange(reg.Cells);
			CellFinder.workingCells.Shuffle<IntVec3>();
			for (int j = 0; j < CellFinder.workingCells.Count; j++)
			{
				result = CellFinder.workingCells[j];
				if (validator == null || validator(result))
				{
					return true;
				}
			}
			result = reg.RandomCell;
			return false;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000E17E0 File Offset: 0x000DF9E0
		public static bool TryFindRandomCellNear(IntVec3 root, Map map, int squareRadius, Predicate<IntVec3> validator, out IntVec3 result, int maxTries = -1)
		{
			int num = root.x - squareRadius;
			int num2 = root.x + squareRadius;
			int num3 = root.z - squareRadius;
			int num4 = root.z + squareRadius;
			int num5 = (num2 - num + 1) * (num4 - num3 + 1);
			if (num < 0)
			{
				num = 0;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num2 > map.Size.x)
			{
				num2 = map.Size.x;
			}
			if (num4 > map.Size.z)
			{
				num4 = map.Size.z;
			}
			int num6;
			bool flag;
			if (maxTries < 0 || maxTries >= num5)
			{
				num6 = 20;
				flag = false;
			}
			else
			{
				num6 = maxTries;
				flag = true;
			}
			for (int i = 0; i < num6; i++)
			{
				IntVec3 intVec = new IntVec3(Rand.RangeInclusive(num, num2), 0, Rand.RangeInclusive(num3, num4));
				if (validator == null || validator(intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(intVec, 0.5f, "found", 50);
					}
					result = intVec;
					return true;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(intVec, 0f, "inv", 50);
				}
			}
			if (flag)
			{
				result = root;
				return false;
			}
			CellFinder.workingListX.Clear();
			CellFinder.workingListZ.Clear();
			for (int j = num; j <= num2; j++)
			{
				CellFinder.workingListX.Add(j);
			}
			for (int k = num3; k <= num4; k++)
			{
				CellFinder.workingListZ.Add(k);
			}
			CellFinder.workingListX.Shuffle<int>();
			CellFinder.workingListZ.Shuffle<int>();
			for (int l = 0; l < CellFinder.workingListX.Count; l++)
			{
				for (int m = 0; m < CellFinder.workingListZ.Count; m++)
				{
					IntVec3 intVec = new IntVec3(CellFinder.workingListX[l], 0, CellFinder.workingListZ[m]);
					if (validator(intVec))
					{
						if (DebugViewSettings.drawDestSearch)
						{
							map.debugDrawer.FlashCell(intVec, 0.6f, "found2", 50);
						}
						result = intVec;
						return true;
					}
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(intVec, 0.25f, "inv2", 50);
					}
				}
			}
			result = root;
			return false;
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000E1A20 File Offset: 0x000DFC20
		public static bool TryFindRandomPawnExitCell(Pawn searcher, out IntVec3 result)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !searcher.Map.roofGrid.Roofed(c) && c.WalkableBy(searcher.Map, searcher) && searcher.CanReach(c, PathEndMode.OnCell, Danger.Some, false, false, TraverseMode.ByPawn), searcher.Map, 0f, out result);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000E1A5C File Offset: 0x000DFC5C
		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, float roadChance, out IntVec3 result)
		{
			if (Rand.Chance(roadChance))
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c)
				select c).TryRandomElement(out result);
				if (flag)
				{
					return flag;
				}
			}
			for (int i = 0; i < 100; i++)
			{
				result = CellFinder.RandomEdgeCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			if (CellFinder.mapEdgeCells == null || map.Size != CellFinder.mapEdgeCellsSize)
			{
				CellFinder.mapEdgeCellsSize = map.Size;
				CellFinder.mapEdgeCells = new List<IntVec3>();
				foreach (IntVec3 item in CellRect.WholeMap(map).EdgeCells)
				{
					CellFinder.mapEdgeCells.Add(item);
				}
			}
			CellFinder.mapEdgeCells.Shuffle<IntVec3>();
			for (int j = 0; j < CellFinder.mapEdgeCells.Count; j++)
			{
				try
				{
					if (validator(CellFinder.mapEdgeCells[j]))
					{
						result = CellFinder.mapEdgeCells[j];
						return true;
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"TryFindRandomEdgeCellWith exception validating ",
						CellFinder.mapEdgeCells[j],
						": ",
						ex.ToString()
					}));
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000E1C0C File Offset: 0x000DFE0C
		public static bool TryFindRandomEdgeCellWith(Predicate<IntVec3> validator, Map map, Rot4 dir, float roadChance, out IntVec3 result)
		{
			if (Rand.Value < roadChance)
			{
				bool flag = (from c in map.roadInfo.roadEdgeTiles
				where validator(c) && c.OnEdge(map, dir)
				select c).TryRandomElement(out result);
				if (flag)
				{
					return flag;
				}
			}
			for (int i = 0; i < 100; i++)
			{
				result = CellFinder.RandomEdgeCell(dir, map);
				if (validator(result))
				{
					return true;
				}
			}
			int asInt = dir.AsInt;
			if (CellFinder.mapSingleEdgeCells[asInt] == null || map.Size != CellFinder.mapSingleEdgeCellsSize)
			{
				CellFinder.mapSingleEdgeCellsSize = map.Size;
				CellFinder.mapSingleEdgeCells[asInt] = new List<IntVec3>();
				foreach (IntVec3 item in CellRect.WholeMap(map).GetEdgeCells(dir))
				{
					CellFinder.mapSingleEdgeCells[asInt].Add(item);
				}
			}
			List<IntVec3> list = CellFinder.mapSingleEdgeCells[asInt];
			list.Shuffle<IntVec3>();
			int j = 0;
			int count = list.Count;
			while (j < count)
			{
				try
				{
					if (validator(list[j]))
					{
						result = list[j];
						return true;
					}
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"TryFindRandomEdgeCellWith exception validating ",
						list[j],
						": ",
						ex.ToString()
					}));
				}
				j++;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000E1E00 File Offset: 0x000E0000
		public static bool TryFindRandomEdgeCellNearWith(IntVec3 near, float radius, Map map, Predicate<IntVec3> validator, out IntVec3 spot)
		{
			CellRect cellRect = CellRect.CenteredOn(near, Mathf.CeilToInt(radius)).ClipInsideMap(map);
			if (cellRect.ExpandedBy(1).Area == cellRect.ExpandedBy(1).ClipInsideMap(map).Area)
			{
				spot = IntVec3.Invalid;
				return false;
			}
			Predicate<IntVec3> predicate = (IntVec3 x) => x.InHorDistOf(near, radius) && x.OnEdge(map) && validator(x);
			if (CellRect.WholeMap(map).EdgeCellsCount < cellRect.Area)
			{
				return CellFinder.TryFindRandomEdgeCellWith(predicate, map, CellFinder.EdgeRoadChance_Ignore, out spot);
			}
			return CellFinder.TryFindRandomCellInsideWith(cellRect, predicate, out spot);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000E1ED8 File Offset: 0x000E00D8
		public static bool TryFindBestPawnStandCell(Pawn forPawn, out IntVec3 cell, bool cellByCell = false)
		{
			cell = IntVec3.Invalid;
			int num = -1;
			float radius = 10f;
			Func<IntVec3, IEnumerable<IntVec3>> <>9__0;
			Predicate<Thing> <>9__2;
			Func<IntVec3, IntVec3, float> <>9__1;
			for (;;)
			{
				CellFinder.tmpDistances.Clear();
				CellFinder.tmpParents.Clear();
				IntVec3 position = forPawn.Position;
				Func<IntVec3, IEnumerable<IntVec3>> neighborsGetter;
				if ((neighborsGetter = <>9__0) == null)
				{
					neighborsGetter = (<>9__0 = ((IntVec3 x) => CellFinder.GetAdjacentCardinalCellsForBestStandCell(x, radius, forPawn)));
				}
				Func<IntVec3, IntVec3, float> distanceGetter;
				if ((distanceGetter = <>9__1) == null)
				{
					distanceGetter = (<>9__1 = delegate(IntVec3 from, IntVec3 to)
					{
						float num4 = 1f;
						if (from.x != to.x && from.z != to.z)
						{
							num4 = 1.4142135f;
						}
						if (!to.Standable(forPawn.Map))
						{
							num4 += 3f;
						}
						if (PawnUtility.AnyPawnBlockingPathAt(to, forPawn, false, false, false))
						{
							List<Thing> thingList = to.GetThingList(forPawn.Map);
							Predicate<Thing> match;
							if ((match = <>9__2) == null)
							{
								match = (<>9__2 = ((Thing x) => x is Pawn && x.HostileTo(forPawn)));
							}
							if (thingList.Find(match) != null)
							{
								num4 += 40f;
							}
							else
							{
								num4 += 15f;
							}
						}
						Building_Door building_Door = to.GetEdifice(forPawn.Map) as Building_Door;
						if (building_Door != null && !building_Door.FreePassage)
						{
							if (building_Door.PawnCanOpen(forPawn))
							{
								num4 += 6f;
							}
							else
							{
								num4 += 50f;
							}
						}
						return num4;
					});
				}
				Dijkstra<IntVec3>.Run(position, neighborsGetter, distanceGetter, CellFinder.tmpDistances, CellFinder.tmpParents);
				if (CellFinder.tmpDistances.Count == num)
				{
					break;
				}
				float num2 = 0f;
				foreach (KeyValuePair<IntVec3, float> keyValuePair in CellFinder.tmpDistances)
				{
					if ((!cell.IsValid || keyValuePair.Value < num2) && keyValuePair.Key.WalkableBy(forPawn.Map, forPawn) && !PawnUtility.AnyPawnBlockingPathAt(keyValuePair.Key, forPawn, false, false, false))
					{
						Building_Door door = keyValuePair.Key.GetDoor(forPawn.Map);
						if (door == null || door.FreePassage)
						{
							cell = keyValuePair.Key;
							num2 = keyValuePair.Value;
						}
					}
				}
				if (cell.IsValid)
				{
					goto Block_5;
				}
				if (radius > (float)forPawn.Map.Size.x && radius > (float)forPawn.Map.Size.z)
				{
					return false;
				}
				radius *= 2f;
				num = CellFinder.tmpDistances.Count;
			}
			return false;
			Block_5:
			if (!cellByCell)
			{
				return true;
			}
			IntVec3 intVec = cell;
			int num3 = 0;
			while (intVec.IsValid && intVec != forPawn.Position)
			{
				num3++;
				if (num3 >= 10000)
				{
					Log.Error("Too many iterations.");
					break;
				}
				if (intVec.WalkableBy(forPawn.Map, forPawn))
				{
					Building_Door door2 = intVec.GetDoor(forPawn.Map);
					if (door2 == null || door2.FreePassage)
					{
						cell = intVec;
					}
				}
				intVec = CellFinder.tmpParents[intVec];
			}
			return true;
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000E2160 File Offset: 0x000E0360
		public static bool TryFindRandomCellInsideWith(CellRect cellRect, Predicate<IntVec3> predicate, out IntVec3 result)
		{
			int num = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)cellRect.Area)), 5);
			for (int i = 0; i < num; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (predicate(randomCell))
				{
					result = randomCell;
					return true;
				}
			}
			CellFinder.tmpCells.Clear();
			foreach (IntVec3 item in cellRect)
			{
				CellFinder.tmpCells.Add(item);
			}
			CellFinder.tmpCells.Shuffle<IntVec3>();
			int j = 0;
			int count = CellFinder.tmpCells.Count;
			while (j < count)
			{
				if (predicate(CellFinder.tmpCells[j]))
				{
					result = CellFinder.tmpCells[j];
					return true;
				}
				j++;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000E225C File Offset: 0x000E045C
		public static IntVec3 RandomSpawnCellForPawnNear(IntVec3 root, Map map, int firstTryWithRadius = 4)
		{
			IntVec3 result;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear(root, map, out result, firstTryWithRadius, null))
			{
				return result;
			}
			return root;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000E227C File Offset: 0x000E047C
		public static bool TryFindRandomSpawnCellForPawnNear(IntVec3 root, Map map, out IntVec3 result, int firstTryWithRadius = 4, Predicate<IntVec3> extraValidator = null)
		{
			if (root.Standable(map) && root.GetFirstPawn(map) == null)
			{
				result = root;
				return true;
			}
			bool rootFogged = root.Fogged(map);
			int num = firstTryWithRadius;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				Map map2 = map;
				float radius = (float)num;
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false);
				Predicate<IntVec3> cellValidator;
				if ((cellValidator = <>9__0) == null)
				{
					cellValidator = (<>9__0 = ((IntVec3 c) => c.Standable(map) && (rootFogged || !c.Fogged(map)) && c.GetFirstPawn(map) == null && (extraValidator == null || extraValidator(c))));
				}
				if (CellFinder.TryFindRandomReachableCellNear(root, map2, radius, traverseParms, cellValidator, null, out result, 999999))
				{
					return true;
				}
				num *= 2;
			}
			num = firstTryWithRadius + 1;
			while (!CellFinder.TryRandomClosewalkCellNear(root, map, num, out result, null))
			{
				if (num > map.Size.x / 2 && num > map.Size.z / 2)
				{
					result = root;
					return false;
				}
				num *= 2;
			}
			return true;
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000E2378 File Offset: 0x000E0578
		public static IntVec3 FindNoWipeSpawnLocNear(IntVec3 near, Map map, ThingDef thingToSpawn, Rot4 rot, int maxDist = 2, Predicate<IntVec3> extraValidator = null)
		{
			int num = GenRadial.NumCellsInRadius((float)maxDist);
			IntVec3 result = IntVec3.Invalid;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = near + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					CellRect cellRect = GenAdj.OccupiedRect(intVec, rot, thingToSpawn.size);
					if (cellRect.InBounds(map) && GenSight.LineOfSight(near, intVec, map, true, null, 0, 0) && (extraValidator == null || extraValidator(intVec)) && (thingToSpawn.category != ThingCategory.Building || GenConstruct.CanBuildOnTerrain(thingToSpawn, intVec, map, rot, null, null)))
					{
						bool flag = false;
						bool flag2 = false;
						CellFinder.tmpUniqueWipedThings.Clear();
						foreach (IntVec3 c in cellRect)
						{
							if (c.Impassable(map))
							{
								flag2 = true;
							}
							List<Thing> thingList = c.GetThingList(map);
							for (int j = 0; j < thingList.Count; j++)
							{
								if (thingList[j] is Pawn)
								{
									flag = true;
								}
								else if (GenSpawn.SpawningWipes(thingToSpawn, thingList[j].def) && !CellFinder.tmpUniqueWipedThings.Contains(thingList[j]))
								{
									CellFinder.tmpUniqueWipedThings.Add(thingList[j]);
								}
							}
						}
						if (flag && thingToSpawn.passability == Traversability.Impassable)
						{
							CellFinder.tmpUniqueWipedThings.Clear();
						}
						else if (flag2 && thingToSpawn.category == ThingCategory.Item)
						{
							CellFinder.tmpUniqueWipedThings.Clear();
						}
						else
						{
							float num3 = 0f;
							for (int k = 0; k < CellFinder.tmpUniqueWipedThings.Count; k++)
							{
								if (CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Building && !CellFinder.tmpUniqueWipedThings[k].def.CostList.NullOrEmpty<ThingDefCountClass>() && CellFinder.tmpUniqueWipedThings[k].def.CostStuffCount == 0)
								{
									List<ThingDefCountClass> list = CellFinder.tmpUniqueWipedThings[k].CostListAdjusted();
									for (int l = 0; l < list.Count; l++)
									{
										num3 += list[l].thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)list[l].count * (float)CellFinder.tmpUniqueWipedThings[k].stackCount;
									}
								}
								else
								{
									num3 += CellFinder.tmpUniqueWipedThings[k].MarketValue * (float)CellFinder.tmpUniqueWipedThings[k].stackCount;
								}
								if (CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Building || CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Item)
								{
									num3 = Mathf.Max(num3, 0.001f);
								}
							}
							CellFinder.tmpUniqueWipedThings.Clear();
							if (!result.IsValid || num3 < num2)
							{
								if (num3 == 0f)
								{
									return intVec;
								}
								result = intVec;
								num2 = num3;
							}
						}
					}
				}
			}
			if (!result.IsValid)
			{
				return near;
			}
			return result;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000E26B4 File Offset: 0x000E08B4
		private static IEnumerable<IntVec3> GetAdjacentCardinalCellsForBestStandCell(IntVec3 x, float radius, Pawn pawn)
		{
			if ((float)(x - pawn.Position).LengthManhattan > radius)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < 4; i = num + 1)
			{
				IntVec3 intVec = x + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(pawn.Map) && intVec.WalkableBy(pawn.Map, pawn))
				{
					Building_Door building_Door = intVec.GetEdifice(pawn.Map) as Building_Door;
					if (building_Door == null || building_Door.CanPhysicallyPass(pawn))
					{
						yield return intVec;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000E26D4 File Offset: 0x000E08D4
		public static IntVec3 StandableCellNear(IntVec3 root, Map map, float radius)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = GenRadial.RadialPattern[i] + root;
				if (intVec.Standable(map))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x040016C1 RID: 5825
		public static float EdgeRoadChance_Ignore = 0f;

		// Token: 0x040016C2 RID: 5826
		public static float EdgeRoadChance_Animal = 0f;

		// Token: 0x040016C3 RID: 5827
		public static float EdgeRoadChance_Hostile = 0.2f;

		// Token: 0x040016C4 RID: 5828
		public static float EdgeRoadChance_Neutral = 0.75f;

		// Token: 0x040016C5 RID: 5829
		public static float EdgeRoadChance_Friendly = 0.75f;

		// Token: 0x040016C6 RID: 5830
		public static float EdgeRoadChance_Always = 1f;

		// Token: 0x040016C7 RID: 5831
		private static List<IntVec3> workingCells = new List<IntVec3>();

		// Token: 0x040016C8 RID: 5832
		private static List<Region> workingRegions = new List<Region>();

		// Token: 0x040016C9 RID: 5833
		private static List<int> workingListX = new List<int>();

		// Token: 0x040016CA RID: 5834
		private static List<int> workingListZ = new List<int>();

		// Token: 0x040016CB RID: 5835
		private static List<IntVec3> mapEdgeCells;

		// Token: 0x040016CC RID: 5836
		private static IntVec3 mapEdgeCellsSize;

		// Token: 0x040016CD RID: 5837
		private static List<IntVec3>[] mapSingleEdgeCells = new List<IntVec3>[4];

		// Token: 0x040016CE RID: 5838
		private static IntVec3 mapSingleEdgeCellsSize;

		// Token: 0x040016CF RID: 5839
		private static Dictionary<IntVec3, float> tmpDistances = new Dictionary<IntVec3, float>();

		// Token: 0x040016D0 RID: 5840
		private static Dictionary<IntVec3, IntVec3> tmpParents = new Dictionary<IntVec3, IntVec3>();

		// Token: 0x040016D1 RID: 5841
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x040016D2 RID: 5842
		private static List<Thing> tmpUniqueWipedThings = new List<Thing>();
	}
}
