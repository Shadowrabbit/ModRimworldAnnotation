using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000215 RID: 533
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00056848 File Offset: 0x00054A48
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3620, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00056899 File Offset: 0x00054A99
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000568AF File Offset: 0x00054AAF
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x000568D4 File Offset: 0x00054AD4
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, delegate(IntVec3 c)
			{
				if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null)
				{
					return this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash;
				}
				return 0;
			}, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000568FE File Offset: 0x00054AFE
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00056920 File Offset: 0x00054B20
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00056943 File Offset: 0x00054B43
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00056950 File Offset: 0x00054B50
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0005696E File Offset: 0x00054B6E
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0005698B File Offset: 0x00054B8B
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00056995 File Offset: 0x00054B95
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x000569AF File Offset: 0x00054BAF
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x000569CC File Offset: 0x00054BCC
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] == def)
			{
				return;
			}
			this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
			this.map.glowGrid.MarkGlowGridDirty(c);
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
			if (validRegionAt_NoRebuild != null)
			{
				validRegionAt_NoRebuild.District.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Roofs);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00056A65 File Offset: 0x00054C65
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04000C14 RID: 3092
		private Map map;

		// Token: 0x04000C15 RID: 3093
		private RoofDef[] roofGrid;

		// Token: 0x04000C16 RID: 3094
		private CellBoolDrawer drawerInt;
	}
}
