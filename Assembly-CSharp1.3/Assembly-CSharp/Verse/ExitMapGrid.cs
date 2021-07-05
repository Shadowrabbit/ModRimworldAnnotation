using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001B2 RID: 434
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00041A08 File Offset: 0x0003FC08
		public bool MapUsesExitGrid
		{
			get
			{
				if (this.map.IsPlayerHome)
				{
					return false;
				}
				CaravansBattlefield caravansBattlefield = this.map.Parent as CaravansBattlefield;
				if (caravansBattlefield != null && caravansBattlefield.def.blockExitGridUntilBattleIsWon && !caravansBattlefield.WonBattle)
				{
					return false;
				}
				FormCaravanComp component = this.map.Parent.GetComponent<FormCaravanComp>();
				return component == null || !component.CanFormOrReformCaravanNow;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00041A70 File Offset: 0x0003FC70
		public CellBoolDrawer Drawer
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3690, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00041AD9 File Offset: 0x0003FCD9
		public BoolGrid Grid
		{
			get
			{
				if (!this.MapUsesExitGrid)
				{
					return null;
				}
				if (this.dirty)
				{
					this.Rebuild();
				}
				return this.exitMapGrid;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00041AF9 File Offset: 0x0003FCF9
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00041B14 File Offset: 0x0003FD14
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00041B2A File Offset: 0x0003FD2A
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00041B50 File Offset: 0x0003FD50
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00041B68 File Offset: 0x0003FD68
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00041B89 File Offset: 0x0003FD89
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00041B89 File Offset: 0x0003FD89
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00041B94 File Offset: 0x0003FD94
		private void Rebuild()
		{
			this.dirty = false;
			if (this.exitMapGrid == null)
			{
				this.exitMapGrid = new BoolGrid(this.map);
			}
			else
			{
				this.exitMapGrid.Clear();
			}
			CellRect cellRect = CellRect.WholeMap(this.map);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (i > 1 && i < cellRect.maxZ - 2 + 1 && j > 1 && j < cellRect.maxX - 2 + 1)
					{
						j = cellRect.maxX - 2 + 1;
					}
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (this.IsGoodExitCell(intVec))
					{
						this.exitMapGrid[intVec] = true;
					}
				}
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00041C68 File Offset: 0x0003FE68
		private bool IsGoodExitCell(IntVec3 cell)
		{
			if (!cell.CanBeSeenOver(this.map))
			{
				return false;
			}
			int num = GenRadial.NumCellsInRadius(2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = cell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.map) && intVec.OnEdge(this.map) && intVec.CanBeSeenOverFast(this.map) && GenSight.LineOfSight(cell, intVec, this.map, false, null, 0, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040009FD RID: 2557
		private Map map;

		// Token: 0x040009FE RID: 2558
		private bool dirty = true;

		// Token: 0x040009FF RID: 2559
		private BoolGrid exitMapGrid;

		// Token: 0x04000A00 RID: 2560
		private CellBoolDrawer drawerInt;

		// Token: 0x04000A01 RID: 2561
		private const int MaxDistToEdge = 2;
	}
}
