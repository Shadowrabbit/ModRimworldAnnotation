using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000269 RID: 617
	public sealed class ExitMapGrid : ICellBoolGiver
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x000B7428 File Offset: 0x000B5628
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

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x000B7490 File Offset: 0x000B5690
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

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00011C5F File Offset: 0x0000FE5F
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

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x00011C7F File Offset: 0x0000FE7F
		public Color Color
		{
			get
			{
				return new Color(0.35f, 1f, 0.35f, 0.12f);
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00011C9A File Offset: 0x0000FE9A
		public ExitMapGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00011CB0 File Offset: 0x0000FEB0
		public bool GetCellBool(int index)
		{
			return this.Grid[index] && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00011CD6 File Offset: 0x0000FED6
		public bool IsExitCell(IntVec3 c)
		{
			return this.MapUsesExitGrid && this.Grid[c];
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00011CEE File Offset: 0x0000FEEE
		public void ExitMapGridUpdate()
		{
			if (!this.MapUsesExitGrid)
			{
				return;
			}
			this.Drawer.MarkForDraw();
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00011D0F File Offset: 0x0000FF0F
		public void Notify_LOSBlockerSpawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00011D0F File Offset: 0x0000FF0F
		public void Notify_LOSBlockerDespawned()
		{
			this.dirty = true;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000B74FC File Offset: 0x000B56FC
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

		// Token: 0x06000FBB RID: 4027 RVA: 0x000B75D0 File Offset: 0x000B57D0
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

		// Token: 0x04000CC0 RID: 3264
		private Map map;

		// Token: 0x04000CC1 RID: 3265
		private bool dirty = true;

		// Token: 0x04000CC2 RID: 3266
		private BoolGrid exitMapGrid;

		// Token: 0x04000CC3 RID: 3267
		private CellBoolDrawer drawerInt;

		// Token: 0x04000CC4 RID: 3268
		private const int MaxDistToEdge = 2;
	}
}
