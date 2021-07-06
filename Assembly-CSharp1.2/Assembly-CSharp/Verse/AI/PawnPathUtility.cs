using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5B RID: 2651
	public static class PawnPathUtility
	{
		// Token: 0x06003F12 RID: 16146 RVA: 0x0017BB94 File Offset: 0x00179D94
		public static Thing FirstBlockingBuilding(this PawnPath path, out IntVec3 cellBefore, Pawn pawn = null)
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
					if ((building_Door != null && !building_Door.FreePassage && (pawn == null || !building_Door.PawnCanOpen(pawn))) || edifice.def.passability == Traversability.Impassable)
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

		// Token: 0x06003F13 RID: 16147 RVA: 0x0017BCC0 File Offset: 0x00179EC0
		public static IntVec3 FinalWalkableNonDoorCell(this PawnPath path, Map map)
		{
			if (path.NodesReversed.Count == 1)
			{
				return path.NodesReversed[0];
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = 0; i < nodesReversed.Count; i++)
			{
				Building edifice = nodesReversed[i].GetEdifice(map);
				if (edifice == null || edifice.def.passability != Traversability.Impassable)
				{
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door == null || building_Door.FreePassage)
					{
						return nodesReversed[i];
					}
				}
			}
			return nodesReversed[0];
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0017BD40 File Offset: 0x00179F40
		public static IntVec3 LastCellBeforeBlockerOrFinalCell(this PawnPath path, Map map)
		{
			if (path.NodesReversed.Count == 1)
			{
				return path.NodesReversed[0];
			}
			List<IntVec3> nodesReversed = path.NodesReversed;
			for (int i = nodesReversed.Count - 2; i >= 1; i--)
			{
				Building edifice = nodesReversed[i].GetEdifice(map);
				if (edifice != null)
				{
					if (edifice.def.passability == Traversability.Impassable)
					{
						return nodesReversed[i + 1];
					}
					Building_Door building_Door = edifice as Building_Door;
					if (building_Door != null && !building_Door.FreePassage)
					{
						return nodesReversed[i + 1];
					}
				}
			}
			return nodesReversed[0];
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0017BDD0 File Offset: 0x00179FD0
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

		// Token: 0x06003F16 RID: 16150 RVA: 0x0017BE5C File Offset: 0x0017A05C
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
