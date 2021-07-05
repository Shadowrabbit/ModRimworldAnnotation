using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF4 RID: 3316
	public static class WatchBuildingUtility
	{
		// Token: 0x06004C37 RID: 19511 RVA: 0x000362C9 File Offset: 0x000344C9
		public static IEnumerable<IntVec3> CalculateWatchCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			List<int> allowedDirections = WatchBuildingUtility.CalculateAllowedDirections(def, rot);
			int num;
			for (int i = 0; i < allowedDirections.Count; i = num + 1)
			{
				foreach (IntVec3 intVec in WatchBuildingUtility.GetWatchCellRect(def, center, rot, allowedDirections[i]))
				{
					if (WatchBuildingUtility.EverPossibleToWatchFrom(intVec, center, map, true, def))
					{
						yield return intVec;
					}
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x001A8FF4 File Offset: 0x001A71F4
		public static bool TryFindBestWatchCell(Thing toWatch, Pawn pawn, bool desireSit, out IntVec3 result, out Building chair)
		{
			List<int> list = WatchBuildingUtility.CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < list.Count; i++)
			{
				CellRect watchCellRect = WatchBuildingUtility.GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]);
				IntVec3 centerCell = watchCellRect.CenterCell;
				int num = watchCellRect.Area * 4;
				for (int j = 0; j < num; j++)
				{
					IntVec3 intVec2 = centerCell + GenRadial.RadialPattern[j];
					if (watchCellRect.Contains(intVec2))
					{
						bool flag = false;
						Building building = null;
						if (WatchBuildingUtility.EverPossibleToWatchFrom(intVec2, toWatch.Position, toWatch.Map, false, toWatch.def) && !intVec2.IsForbidden(pawn) && pawn.CanReserve(intVec2, 1, -1, null, false) && pawn.Map.pawnDestinationReservationManager.CanReserve(intVec2, pawn, false))
						{
							if (desireSit)
							{
								building = intVec2.GetEdifice(pawn.Map);
								if (building != null && building.def.building.isSittable && pawn.CanReserve(building, 1, -1, null, false))
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
						if (flag)
						{
							if (!desireSit || !(building.Rotation != new Rot4(list[i]).Opposite))
							{
								result = intVec2;
								chair = building;
								return true;
							}
							intVec = intVec2;
						}
					}
				}
			}
			if (intVec.IsValid)
			{
				result = intVec;
				chair = intVec.GetEdifice(pawn.Map);
				return true;
			}
			result = IntVec3.Invalid;
			chair = null;
			return false;
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x001A91A8 File Offset: 0x001A73A8
		public static bool CanWatchFromBed(Pawn pawn, Building_Bed bed, Thing toWatch)
		{
			if (!WatchBuildingUtility.EverPossibleToWatchFrom(pawn.Position, toWatch.Position, pawn.Map, true, toWatch.def))
			{
				return false;
			}
			if (toWatch.def.rotatable)
			{
				Rot4 rotation = bed.Rotation;
				CellRect cellRect = toWatch.OccupiedRect();
				if (rotation == Rot4.North && cellRect.maxZ < pawn.Position.z)
				{
					return false;
				}
				if (rotation == Rot4.South && cellRect.minZ > pawn.Position.z)
				{
					return false;
				}
				if (rotation == Rot4.East && cellRect.maxX < pawn.Position.x)
				{
					return false;
				}
				if (rotation == Rot4.West && cellRect.minX > pawn.Position.x)
				{
					return false;
				}
			}
			List<int> list = WatchBuildingUtility.CalculateAllowedDirections(toWatch.def, toWatch.Rotation);
			for (int i = 0; i < list.Count; i++)
			{
				if (WatchBuildingUtility.GetWatchCellRect(toWatch.def, toWatch.Position, toWatch.Rotation, list[i]).Contains(pawn.Position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x001A92D4 File Offset: 0x001A74D4
		private static CellRect GetWatchCellRect(ThingDef def, IntVec3 center, Rot4 rot, int watchRot)
		{
			Rot4 a = new Rot4(watchRot);
			if (def.building == null)
			{
				def = (def.entityDefToBuild as ThingDef);
			}
			CellRect result;
			if (a.IsHorizontal)
			{
				int num = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.min;
				int num2 = center.x + GenAdj.CardinalDirections[watchRot].x * def.building.watchBuildingStandDistanceRange.max;
				int num3 = center.z + def.building.watchBuildingStandRectWidth / 2;
				int num4 = center.z - def.building.watchBuildingStandRectWidth / 2;
				if (def.building.watchBuildingStandRectWidth % 2 == 0)
				{
					if (a == Rot4.West)
					{
						num4++;
					}
					else
					{
						num3--;
					}
				}
				result = new CellRect(Mathf.Min(num, num2), num4, Mathf.Abs(num - num2) + 1, num3 - num4 + 1);
			}
			else
			{
				int num5 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.min;
				int num6 = center.z + GenAdj.CardinalDirections[watchRot].z * def.building.watchBuildingStandDistanceRange.max;
				int num7 = center.x + def.building.watchBuildingStandRectWidth / 2;
				int num8 = center.x - def.building.watchBuildingStandRectWidth / 2;
				if (def.building.watchBuildingStandRectWidth % 2 == 0)
				{
					if (a == Rot4.North)
					{
						num8++;
					}
					else
					{
						num7--;
					}
				}
				result = new CellRect(num8, Mathf.Min(num5, num6), num7 - num8 + 1, Mathf.Abs(num5 - num6) + 1);
			}
			return result;
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x001A94A8 File Offset: 0x001A76A8
		private static bool EverPossibleToWatchFrom(IntVec3 watchCell, IntVec3 buildingCenter, Map map, bool bedAllowed, ThingDef def)
		{
			Room room = (def.building != null && def.building.watchBuildingInSameRoom) ? buildingCenter.GetRoom(map, RegionType.Set_Passable) : null;
			return (room == null || room.ContainsCell(watchCell)) && (watchCell.Standable(map) || (bedAllowed && watchCell.GetEdifice(map) is Building_Bed)) && GenSight.LineOfSight(buildingCenter, watchCell, map, true, null, 0, 0);
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x001A950C File Offset: 0x001A770C
		private static List<int> CalculateAllowedDirections(ThingDef toWatchDef, Rot4 toWatchRot)
		{
			WatchBuildingUtility.allowedDirections.Clear();
			if (toWatchDef.rotatable)
			{
				WatchBuildingUtility.allowedDirections.Add(toWatchRot.AsInt);
			}
			else
			{
				WatchBuildingUtility.allowedDirections.Add(0);
				WatchBuildingUtility.allowedDirections.Add(1);
				WatchBuildingUtility.allowedDirections.Add(2);
				WatchBuildingUtility.allowedDirections.Add(3);
			}
			return WatchBuildingUtility.allowedDirections;
		}

		// Token: 0x04003232 RID: 12850
		private static List<int> allowedDirections = new List<int>();
	}
}
