using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001A3 RID: 419
	public sealed class FogGrid : IExposable
	{
		// Token: 0x06000BBD RID: 3005 RVA: 0x0003FA4B File Offset: 0x0003DC4B
		public FogGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0003FA5A File Offset: 0x0003DC5A
		public void ExposeData()
		{
			DataExposeUtility.BoolArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0003FA78 File Offset: 0x0003DC78
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

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0003FB0C File Offset: 0x0003DD0C
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

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0003FBA4 File Offset: 0x0003DDA4
		public bool IsFogged(IntVec3 c)
		{
			return c.InBounds(this.map) && this.fogGrid != null && this.fogGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0003FBD6 File Offset: 0x0003DDD6
		public bool IsFogged(int index)
		{
			return this.fogGrid[index];
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0003FBE0 File Offset: 0x0003DDE0
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

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0003FC34 File Offset: 0x0003DE34
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
			this.FloodUnfogAdjacent(c, true);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0003FC91 File Offset: 0x0003DE91
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			this.FloodUnfogAdjacent(door.Position, false);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0003FCBC File Offset: 0x0003DEBC
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

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0003FD58 File Offset: 0x0003DF58
		private void FloodUnfogAdjacent(IntVec3 c, bool sendLetters = true)
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
			if (flag && sendLetters)
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

		// Token: 0x040009C2 RID: 2498
		private Map map;

		// Token: 0x040009C3 RID: 2499
		public bool[] fogGrid;

		// Token: 0x040009C4 RID: 2500
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;
	}
}
