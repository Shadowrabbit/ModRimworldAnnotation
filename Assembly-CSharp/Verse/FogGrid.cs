using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000257 RID: 599
	public sealed class FogGrid : IExposable
	{
		// Token: 0x06000F31 RID: 3889 RVA: 0x00011635 File Offset: 0x0000F835
		public FogGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00011644 File Offset: 0x0000F844
		public void ExposeData()
		{
			DataExposeUtility.BoolArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000B5990 File Offset: 0x000B3B90
		public void Unfog(IntVec3 c)
		{
			this.UnfogWorker(c);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.Fillage == FillCategory.Full)
				{
					foreach (IntVec3 c2 in thing.OccupiedRect().Cells)
					{
						this.UnfogWorker(c2);
					}
				}
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x000B5A24 File Offset: 0x000B3C24
		private void UnfogWorker(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (!this.fogGrid[num])
			{
				return;
			}
			this.fogGrid[num] = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Things | MapMeshFlag.FogOfWar);
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.Mine);
			if (designation != null && c.GetFirstMineable(this.map) == null)
			{
				designation.Delete();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00011661 File Offset: 0x0000F861
		public bool IsFogged(IntVec3 c)
		{
			return c.InBounds(this.map) && this.fogGrid != null && this.fogGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00011693 File Offset: 0x0000F893
		public bool IsFogged(int index)
		{
			return this.fogGrid[index];
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x000B5ABC File Offset: 0x000B3CBC
		public void ClearAllFog()
		{
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.Unfog(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000B5B10 File Offset: 0x000B3D10
		public void Notify_FogBlockerRemoved(IntVec3 c)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(this.map) && !this.IsFogged(c2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			this.FloodUnfogAdjacent(c);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0001169D File Offset: 0x0000F89D
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			this.FloodUnfogAdjacent(door.Position);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000B5B6C File Offset: 0x000B3D6C
		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			if (this.fogGrid == null)
			{
				this.fogGrid = new bool[cellIndices.NumGridCells];
			}
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.fogGrid[cellIndices.CellToIndex(c)] = true;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000B5C08 File Offset: 0x000B3E08
		private void FloodUnfogAdjacent(IntVec3 c)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult floodUnfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						floodUnfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[j];
				if (c2.InBounds(this.map))
				{
					Building edifice2 = c2.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c2);
					}
				}
			}
			if (flag)
			{
				if (floodUnfogResult.mechanoidFound)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.ThreatBig, new TargetInfo(c, this.map, false), null, null, null, null);
					return;
				}
				if (!floodUnfogResult.allOnScreen || floodUnfogResult.cellsUnfogged >= 600)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(c, this.map, false), null, null, null, null);
				}
			}
		}

		// Token: 0x04000C7B RID: 3195
		private Map map;

		// Token: 0x04000C7C RID: 3196
		public bool[] fogGrid;

		// Token: 0x04000C7D RID: 3197
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;
	}
}
