using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000301 RID: 769
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x000CBA38 File Offset: 0x000C9C38
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

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00014132 File Offset: 0x00012332
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00014148 File Offset: 0x00012348
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0001416D File Offset: 0x0001236D
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

		// Token: 0x060013B4 RID: 5044 RVA: 0x00014197 File Offset: 0x00012397
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x000141B9 File Offset: 0x000123B9
		public Color GetCellExtraColor(int index)
		{
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				return Color.gray;
			}
			return Color.white;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000141DC File Offset: 0x000123DC
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x000141E9 File Offset: 0x000123E9
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00014207 File Offset: 0x00012407
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00014224 File Offset: 0x00012424
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0001422E File Offset: 0x0001242E
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00014248 File Offset: 0x00012448
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000CBA8C File Offset: 0x000C9C8C
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
				validRegionAt_NoRebuild.Room.Notify_RoofChanged();
			}
			if (this.drawerInt != null)
			{
				this.drawerInt.SetDirty();
			}
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Roofs);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00014263 File Offset: 0x00012463
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04000F95 RID: 3989
		private Map map;

		// Token: 0x04000F96 RID: 3990
		private RoofDef[] roofGrid;

		// Token: 0x04000F97 RID: 3991
		private CellBoolDrawer drawerInt;
	}
}
