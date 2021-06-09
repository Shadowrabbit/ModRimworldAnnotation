using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000235 RID: 565
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00010D8F File Offset: 0x0000EF8F
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00010D9C File Offset: 0x0000EF9C
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000E72 RID: 3698
		public abstract string Label { get; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000E73 RID: 3699
		public abstract Color Color { get; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000E74 RID: 3700
		public abstract int ListPriority { get; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00010DA9 File Offset: 0x0000EFA9
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x170002AF RID: 687
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x170002B0 RID: 688
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x000B34A8 File Offset: 0x000B16A8
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 3650, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00010E20 File Offset: 0x0000F020
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00010E2D File Offset: 0x0000F02D
		public Area()
		{
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00010E3C File Offset: 0x0000F03C
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00010E73 File Offset: 0x0000F073
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00010DD0 File Offset: 0x0000EFD0
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000B34FC File Offset: 0x000B16FC
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] == val)
			{
				return;
			}
			this.innerGrid[index] = val;
			this.MarkDirty(c);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000B3540 File Offset: 0x000B1740
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00010E9C File Offset: 0x0000F09C
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00010EAA File Offset: 0x0000F0AA
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00010EC4 File Offset: 0x0000F0C4
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00010ED1 File Offset: 0x0000F0D1
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06000E8A RID: 3722
		public abstract string GetUniqueLoadID();

		// Token: 0x04000C01 RID: 3073
		public AreaManager areaManager;

		// Token: 0x04000C02 RID: 3074
		public int ID = -1;

		// Token: 0x04000C03 RID: 3075
		private BoolGrid innerGrid;

		// Token: 0x04000C04 RID: 3076
		private CellBoolDrawer drawer;

		// Token: 0x04000C05 RID: 3077
		private Texture2D colorTextureInt;
	}
}
