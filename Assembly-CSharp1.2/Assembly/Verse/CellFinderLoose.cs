﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000830 RID: 2096
	public static class CellFinderLoose
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x00153BF4 File Offset: 0x00151DF4
		public static IntVec3 RandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries = 1000)
		{
			IntVec3 result;
			CellFinderLoose.TryGetRandomCellWith(validator, map, maxTries, out result);
			return result;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x00153C10 File Offset: 0x00151E10
		public static bool TryGetRandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries, out IntVec3 result)
		{
			for (int i = 0; i < maxTries; i++)
			{
				result = CellFinder.RandomCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00153C54 File Offset: 0x00151E54
		public static bool TryFindRandomNotEdgeCellWith(int minEdgeDistance, Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			for (int i = 0; i < 1000; i++)
			{
				result = CellFinder.RandomNotEdgeCell(minEdgeDistance, map);
				if (result.IsValid && validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000292D1 File Offset: 0x000274D1
		public static IntVec3 GetFleeDest(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			if (pawn.RaceProps.Animal)
			{
				return CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
			}
			return CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00153CA4 File Offset: 0x00151EA4
		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			for (float num2 = 200f; num2 <= 360f; num2 += 10f)
			{
				IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range(-num2 / 2f, num2 / 2f)) * num).ToIntVec3();
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
				{
					return intVec;
				}
			}
			float num3 = num;
			while (num3 * 3f > num)
			{
				IntVec3 intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
				{
					return intVec2;
				}
				num3 -= distance / 10f;
			}
			return pawn.Position;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00153D94 File Offset: 0x00151F94
		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			return location.Standable(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(location, pawn, false) && location.GetRegion(pawn.Map, RegionType.Set_Passable).type != RegionType.Portal && pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00153DF4 File Offset: 0x00151FF4
		public static IntVec3 GetFleeDestToolUser(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			IntVec3 bestPos = pawn.Position;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				Danger danger = reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if (intVec.Standable(map) && !reg.IsDoorway)
					{
						Thing thing = null;
						float num = 0f;
						for (int i = 0; i < threats.Count; i++)
						{
							float num2 = (float)intVec.DistanceToSquared(threats[i].Position);
							if (thing == null || num2 < num)
							{
								thing = threats[i];
								num = num2;
							}
						}
						float num3 = Mathf.Sqrt(num);
						float num4 = Mathf.Pow(Mathf.Min(num3, distance), 1.2f);
						num4 *= Mathf.InverseLerp(50f, 0f, (intVec - pawn.Position).LengthHorizontal);
						if (intVec.GetRoom(map, RegionType.Set_Passable) != thing.GetRoom(RegionType.Set_Passable))
						{
							num4 *= 4.2f;
						}
						else if (num3 < 8f)
						{
							num4 *= 0.05f;
						}
						if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
						{
							num4 *= 0.5f;
						}
						if (danger == Danger.Deadly)
						{
							num4 *= 0.8f;
						}
						if (num4 > bestScore)
						{
							bestPos = intVec;
							bestScore = num4;
						}
					}
				}
				return false;
			}, 20, RegionType.Set_Passable);
			return bestPos;
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00153E80 File Offset: 0x00152080
		public static IntVec3 TryFindCentralCell(Map map, int tightness, int minCellCount, Predicate<IntVec3> extraValidator = null)
		{
			int debug_numStand = 0;
			int debug_numRoom = 0;
			int debug_numTouch = 0;
			int debug_numRoomCellCount = 0;
			int debug_numExtraValidator = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					int num2 = debug_numStand;
					debug_numStand = num2 + 1;
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room == null)
				{
					int num2 = debug_numRoom;
					debug_numRoom = num2 + 1;
					return false;
				}
				if (!room.TouchesMapEdge)
				{
					int num2 = debug_numTouch;
					debug_numTouch = num2 + 1;
					return false;
				}
				if (room.CellCount < minCellCount)
				{
					int num2 = debug_numRoomCellCount;
					debug_numRoomCellCount = num2 + 1;
					return false;
				}
				if (extraValidator != null && !extraValidator(c))
				{
					int num2 = debug_numExtraValidator;
					debug_numExtraValidator = num2 + 1;
					return false;
				}
				return true;
			};
			for (int i = tightness; i >= 1; i--)
			{
				int num = map.Size.x / i;
				IntVec3 result;
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith((map.Size.x - num) / 2, validator, map, out result))
				{
					return result;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Found no good central spot. Choosing randomly. numStand=",
				debug_numStand,
				", numRoom=",
				debug_numRoom,
				", numTouch=",
				debug_numTouch,
				", numRoomCellCount=",
				debug_numRoomCellCount,
				", numExtraValidator=",
				debug_numExtraValidator
			}), false);
			return CellFinderLoose.RandomCellWith((IntVec3 x) => x.Standable(map), map, 1000);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00153FBC File Offset: 0x001521BC
		public static bool TryFindSkyfallerCell(ThingDef skyfaller, Map map, out IntVec3 cell, int minDistToEdge = 10, IntVec3 nearLoc = default(IntVec3), int nearLocMaxDist = -1, bool allowRoofedCells = true, bool allowCellsWithItems = false, bool allowCellsWithBuildings = false, bool colonyReachable = false, bool avoidColonistsIfExplosive = true, bool alwaysAvoidColonists = false, Predicate<IntVec3> extraValidator = null)
		{
			bool avoidColonists = (avoidColonistsIfExplosive && skyfaller.skyfaller.CausesExplosion) || alwaysAvoidColonists;
			Predicate<IntVec3> validator = delegate(IntVec3 x)
			{
				foreach (IntVec3 c in GenAdj.OccupiedRect(x, Rot4.North, skyfaller.size))
				{
					if (!c.InBounds(map) || c.Fogged(map) || !c.Standable(map) || (c.Roofed(map) && c.GetRoof(map).isThickRoof))
					{
						return false;
					}
					if (!allowRoofedCells && c.Roofed(map))
					{
						return false;
					}
					if (!allowCellsWithItems && c.GetFirstItem(map) != null)
					{
						return false;
					}
					if (!allowCellsWithBuildings && c.GetFirstBuilding(map) != null)
					{
						return false;
					}
					if (c.GetFirstSkyfaller(map) != null)
					{
						return false;
					}
					using (List<Thing>.Enumerator enumerator2 = c.GetThingList(map).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.preventSkyfallersLandingOn)
							{
								return false;
							}
						}
					}
				}
				return (!avoidColonists || !SkyfallerUtility.CanPossiblyFallOnColonist(skyfaller, x, map)) && (minDistToEdge <= 0 || x.DistanceToEdge(map) >= minDistToEdge) && (!colonyReachable || map.reachability.CanReachColony(x)) && (extraValidator == null || extraValidator(x));
			};
			if (nearLocMaxDist > 0)
			{
				return CellFinder.TryFindRandomCellNear(nearLoc, map, nearLocMaxDist, validator, out cell, -1);
			}
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(minDistToEdge, validator, map, out cell);
		}
	}
}
