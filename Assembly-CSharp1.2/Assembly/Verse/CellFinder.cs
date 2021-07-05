using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000821 RID: 2081
	public static class CellFinder
	{
		// Token: 0x06003465 RID: 13413 RVA: 0x00029114 File Offset: 0x00027314
		public static IntVec3 RandomCell(Map map)
		{
			return new IntVec3(Rand.Range(0, map.Size.x), 0, Rand.Range(0, map.Size.z));
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x00152218 File Offset: 0x00150418
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

		// Token: 0x06003467 RID: 13415 RVA: 0x001522C0 File Offset: 0x001504C0
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

		// Token: 0x06003468 RID: 13416 RVA: 0x00152384 File Offset: 0x00150584
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

		// Token: 0x06003469 RID: 13417 RVA: 0x001523E8 File Offset: 0x001505E8
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

		// Token: 0x0600346A RID: 13418 RVA: 0x0015244C File Offset: 0x0015064C
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

		// Token: 0x0600346B RID: 13419 RVA: 0x001524FC File Offset: 0x001506FC
		public static void AllRegionsNear(List<Region> results, Region root, int maxRegions, TraverseParms traverseParms, Predicate<Region> validator = null, Pawn pawnToAllow = null, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (results == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid results list", 60733193, false);
				return;
			}
			results.Clear();
			if (root == null)
			{
				Log.ErrorOnce("Attempted to call AllRegionsNear with an invalid root", 9107839, false);
				return;
			}
			RegionTraverser.BreadthFirstTraverse(root, (Region from, Region r) => (validator == null || validator(r)) && r.Allows(traverseParms, true) && (pawnToAllow == null || !r.IsForbiddenEntirely(pawnToAllow)), delegate(Region r)
			{
				results.Add(r);
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x00152588 File Offset: 0x00150788
		public static bool TryFindRandomReachableCellNear(IntVec3 root, Map map, float radius, TraverseParms traverseParms, Predicate<IntVec3> cellValidator, Predicate<Region> regionValidator, out IntVec3 result, int maxRegions = 999999)
		{
			if (map == null)
			{
				Log.ErrorOnce("Tried to find reachable cell in a null map", 61037855, false);
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

		// Token: 0x0600346D RID: 13421 RVA: 0x001526E4 File Offset: 0x001508E4
		public static IntVec3 RandomClosewalkCellNear(IntVec3 root, Map map, int radius, Predicate<IntVec3> extraValidator = null)
		{
			IntVec3 result;
			if (CellFinder.TryRandomClosewalkCellNear(root, map, radius, out result, extraValidator))
			{
				return result;
			}
			return root;
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x00152704 File Offset: 0x00150904
		public static bool TryRandomClosewalkCellNear(IntVec3 root, Map map, int radius, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => c.Standable(map) && (extraValidator == null || extraValidator(c)), null, out result, 999999);
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x00152750 File Offset: 0x00150950
		public static IntVec3 RandomClosewalkCellNearNotForbidden(IntVec3 root, Map map, int radius, Pawn pawn)
		{
			IntVec3 result;
			if (!CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => !c.IsForbidden(pawn) && c.Standable(map), null, out result, 999999))
			{
				return CellFinder.RandomClosewalkCellNear(root, map, radius, null);
			}
			return result;
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x001527B0 File Offset: 0x001509B0
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

		// Token: 0x06003471 RID: 13425 RVA: 0x00152854 File Offset: 0x00150A54
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

		// Token: 0x06003472 RID: 13426 RVA: 0x00152A94 File Offset: 0x00150C94
		public static bool TryFindRandomPawnExitCell(Pawn searcher, out IntVec3 result)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => !searcher.Map.roofGrid.Roofed(c) && c.Walkable(searcher.Map) && searcher.CanReach(c, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn), searcher.Map, 0f, out result);
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x00152AD0 File Offset: 0x00150CD0
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
					}), false);
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x00152C80 File Offset: 0x00150E80
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
					}), false);
				}
				j++;
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x00152E74 File Offset: 0x00151074
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

		// Token: 0x06003476 RID: 13430 RVA: 0x00152F4C File Offset: 0x0015114C
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
					if ((!cell.IsValid || keyValuePair.Value < num2) && keyValuePair.Key.Walkable(forPawn.Map) && !PawnUtility.AnyPawnBlockingPathAt(keyValuePair.Key, forPawn, false, false, false))
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
					Log.Error("Too many iterations.", false);
					break;
				}
				if (intVec.Walkable(forPawn.Map))
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

		// Token: 0x06003477 RID: 13431 RVA: 0x001531CC File Offset: 0x001513CC
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

		// Token: 0x06003478 RID: 13432 RVA: 0x001532C8 File Offset: 0x001514C8
		public static IntVec3 RandomSpawnCellForPawnNear(IntVec3 root, Map map, int firstTryWithRadius = 4)
		{
			IntVec3 result;
			if (CellFinder.TryFindRandomSpawnCellForPawnNear_NewTmp(root, map, out result, firstTryWithRadius, null))
			{
				return result;
			}
			return root;
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0002913E File Offset: 0x0002733E
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static bool TryFindRandomSpawnCellForPawnNear(IntVec3 root, Map map, out IntVec3 result, int firstTryWithRadius = 4)
		{
			return CellFinder.TryFindRandomSpawnCellForPawnNear_NewTmp(root, map, out result, firstTryWithRadius, null);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x001532E8 File Offset: 0x001514E8
		public static bool TryFindRandomSpawnCellForPawnNear_NewTmp(IntVec3 root, Map map, out IntVec3 result, int firstTryWithRadius = 4, Predicate<IntVec3> extraValidator = null)
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
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
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

		// Token: 0x0600347B RID: 13435 RVA: 0x001533E4 File Offset: 0x001515E4
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
								if (CellFinder.tmpUniqueWipedThings[k].def.category == ThingCategory.Building && !CellFinder.tmpUniqueWipedThings[k].def.costList.NullOrEmpty<ThingDefCountClass>() && CellFinder.tmpUniqueWipedThings[k].def.costStuffCount == 0)
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

		// Token: 0x0600347C RID: 13436 RVA: 0x0002914A File Offset: 0x0002734A
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
				if (intVec.InBounds(pawn.Map) && intVec.Walkable(pawn.Map))
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

		// Token: 0x04002416 RID: 9238
		public static float EdgeRoadChance_Ignore = 0f;

		// Token: 0x04002417 RID: 9239
		public static float EdgeRoadChance_Animal = 0f;

		// Token: 0x04002418 RID: 9240
		public static float EdgeRoadChance_Hostile = 0.2f;

		// Token: 0x04002419 RID: 9241
		public static float EdgeRoadChance_Neutral = 0.75f;

		// Token: 0x0400241A RID: 9242
		public static float EdgeRoadChance_Friendly = 0.75f;

		// Token: 0x0400241B RID: 9243
		public static float EdgeRoadChance_Always = 1f;

		// Token: 0x0400241C RID: 9244
		private static List<IntVec3> workingCells = new List<IntVec3>();

		// Token: 0x0400241D RID: 9245
		private static List<Region> workingRegions = new List<Region>();

		// Token: 0x0400241E RID: 9246
		private static List<int> workingListX = new List<int>();

		// Token: 0x0400241F RID: 9247
		private static List<int> workingListZ = new List<int>();

		// Token: 0x04002420 RID: 9248
		private static List<IntVec3> mapEdgeCells;

		// Token: 0x04002421 RID: 9249
		private static IntVec3 mapEdgeCellsSize;

		// Token: 0x04002422 RID: 9250
		private static List<IntVec3>[] mapSingleEdgeCells = new List<IntVec3>[4];

		// Token: 0x04002423 RID: 9251
		private static IntVec3 mapSingleEdgeCellsSize;

		// Token: 0x04002424 RID: 9252
		private static Dictionary<IntVec3, float> tmpDistances = new Dictionary<IntVec3, float>();

		// Token: 0x04002425 RID: 9253
		private static Dictionary<IntVec3, IntVec3> tmpParents = new Dictionary<IntVec3, IntVec3>();

		// Token: 0x04002426 RID: 9254
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04002427 RID: 9255
		private static List<Thing> tmpUniqueWipedThings = new List<Thing>();
	}
}
