using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200020F RID: 527
	public class AutoBuildRoofAreaSetter
	{
		// Token: 0x06000F17 RID: 3863 RVA: 0x000556C0 File Offset: 0x000538C0
		public AutoBuildRoofAreaSetter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000556FB File Offset: 0x000538FB
		public void TryGenerateAreaFor(Room room)
		{
			this.queuedGenerateRooms.Add(room);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00055709 File Offset: 0x00053909
		public void AutoBuildRoofAreaSetterTick_First()
		{
			this.ResolveQueuedGenerateRoofs();
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00055714 File Offset: 0x00053914
		public void ResolveQueuedGenerateRoofs()
		{
			for (int i = 0; i < this.queuedGenerateRooms.Count; i++)
			{
				this.TryGenerateAreaNow(this.queuedGenerateRooms[i]);
			}
			this.queuedGenerateRooms.Clear();
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00055754 File Offset: 0x00053954
		private void TryGenerateAreaNow(Room room)
		{
			if (room.Dereferenced || room.TouchesMapEdge)
			{
				return;
			}
			if (room.RegionCount > 26 || room.CellCount > 320)
			{
				return;
			}
			if (room.IsDoorway)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in room.BorderCells)
			{
				Thing roofHolderOrImpassable = c.GetRoofHolderOrImpassable(this.map);
				if (roofHolderOrImpassable != null)
				{
					if (roofHolderOrImpassable.Faction != null && roofHolderOrImpassable.Faction != Faction.OfPlayer)
					{
						return;
					}
					if (roofHolderOrImpassable.def.building != null && !roofHolderOrImpassable.def.building.allowAutoroof)
					{
						return;
					}
					if (roofHolderOrImpassable.Faction == Faction.OfPlayer)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.innerCells.Clear();
			foreach (IntVec3 intVec in room.Cells)
			{
				if (!this.innerCells.Contains(intVec))
				{
					this.innerCells.Add(intVec);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = intVec + GenAdj.AdjacentCells[i];
					if (c2.InBounds(this.map))
					{
						Thing roofHolderOrImpassable2 = c2.GetRoofHolderOrImpassable(this.map);
						if (roofHolderOrImpassable2 != null && (roofHolderOrImpassable2.def.size.x > 1 || roofHolderOrImpassable2.def.size.z > 1))
						{
							CellRect cellRect = roofHolderOrImpassable2.OccupiedRect();
							cellRect.ClipInsideMap(this.map);
							for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
							{
								for (int k = cellRect.minX; k <= cellRect.maxX; k++)
								{
									IntVec3 item = new IntVec3(k, 0, j);
									if (!this.innerCells.Contains(item))
									{
										this.innerCells.Add(item);
									}
								}
							}
						}
					}
				}
			}
			this.cellsToRoof.Clear();
			foreach (IntVec3 a in this.innerCells)
			{
				for (int l = 0; l < 9; l++)
				{
					IntVec3 intVec2 = a + GenAdj.AdjacentCellsAndInside[l];
					if (intVec2.InBounds(this.map) && (l == 8 || intVec2.GetRoofHolderOrImpassable(this.map) != null) && !this.cellsToRoof.Contains(intVec2))
					{
						this.cellsToRoof.Add(intVec2);
					}
				}
			}
			this.justRoofedCells.Clear();
			foreach (IntVec3 intVec3 in this.cellsToRoof)
			{
				if (this.map.roofGrid.RoofAt(intVec3) == null && !this.justRoofedCells.Contains(intVec3) && !this.map.areaManager.NoRoof[intVec3] && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec3, this.map, true))
				{
					this.map.areaManager.BuildRoof[intVec3] = true;
					this.justRoofedCells.Add(intVec3);
				}
			}
		}

		// Token: 0x04000C06 RID: 3078
		private Map map;

		// Token: 0x04000C07 RID: 3079
		private List<Room> queuedGenerateRooms = new List<Room>();

		// Token: 0x04000C08 RID: 3080
		private HashSet<IntVec3> cellsToRoof = new HashSet<IntVec3>();

		// Token: 0x04000C09 RID: 3081
		private HashSet<IntVec3> innerCells = new HashSet<IntVec3>();

		// Token: 0x04000C0A RID: 3082
		private List<IntVec3> justRoofedCells = new List<IntVec3>();
	}
}
