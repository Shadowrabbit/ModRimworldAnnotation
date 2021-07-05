using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000603 RID: 1539
	public static class PawnPathUtility
	{
		// Token: 0x06002C3F RID: 11327 RVA: 0x0010750C File Offset: 0x0010570C
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn)
		{
			if (!path.Found)
			{
				cellBefore = IntVec3.Invalid;
				return null;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			if (nodesReversed.Count == 1)
			{
				cellBefore = nodesReversed[0];
				return null;
			}
			Building building = null;
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = nodesReversed.Count - 2; i >= 0; i--)
			{
				Building edifice = nodesReversed[i].GetEdifice(pawn.Map);
				if (edifice != null)
				{
					Building_Door building_Door = edifice as Building_Door;
					bool flag = building_Door != null && !building_Door.FreePassage && !building_Door.PawnCanOpen(pawn);
					bool flag2 = edifice.def.IsFence && !pawn.def.race.CanPassFences;
					if (flag || flag2 || edifice.def.passability == Traversability.Impassable)
					{
						if (building != null)
						{
							cellBefore = intVec;
							return building;
						}
						cellBefore = nodesReversed[i + 1];
						return edifice;
					}
				}
				if (edifice != null && edifice.def.passability == Traversability.PassThroughOnly && edifice.def.Fillage == FillCategory.Full)
				{
					if (building == null)
					{
						building = edifice;
						intVec = nodesReversed[i + 1];
					}
				}
				else if (edifice == null || edifice.def.passability != Traversability.PassThroughOnly)
				{
					building = null;
				}
			}
			cellBefore = nodesReversed[0];
			return null;
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x0010765C File Offset: 0x0010585C
		public static bool TryFindLastCellBeforeBlockingDoor(this PawnPath path, Pawn pawn, out IntVec3 result)
		{
			if (path.NodesReversed.Count == 1)
			{
				result = path.NodesReversed[0];
				return false;
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = nodesReversed.Count - 2; i >= 1; i--)
			{
				Building_Door building_Door = nodesReversed[i].GetEdifice(pawn.Map) as Building_Door;
				if (building_Door != null && !building_Door.CanPhysicallyPass(pawn))
				{
					result = nodesReversed[i + 1];
					return true;
				}
			}
			result = nodesReversed[0];
			return false;
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x001076E8 File Offset: 0x001058E8
		public static bool TryFindCellAtIndex(PawnPath path, int index, out IntVec3 result)
		{
			if (path.NodesReversed.Count <= index || index < 0)
			{
				result = IntVec3.Invalid;
				return false;
			}
			result = path.NodesReversed[path.NodesReversed.Count - 1 - index];
			return true;
		}
	}
}
