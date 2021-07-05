using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017D RID: 381
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0003B0FC File Offset: 0x000392FC
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0003B109 File Offset: 0x00039309
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000ACA RID: 2762
		public abstract string Label { get; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000ACB RID: 2763
		public abstract Color Color { get; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000ACC RID: 2764
		public abstract int ListPriority { get; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0003B116 File Offset: 0x00039316
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

		// Token: 0x17000218 RID: 536
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

		// Token: 0x17000219 RID: 537
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0003B190 File Offset: 0x00039390
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0003B1E1 File Offset: 0x000393E1
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0003B1EE File Offset: 0x000393EE
		public Area()
		{
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0003B1FD File Offset: 0x000393FD
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0003B234 File Offset: 0x00039434
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", Array.Empty<object>());
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0003B13D File Offset: 0x0003933D
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0001A4C7 File Offset: 0x000186C7
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003B260 File Offset: 0x00039460
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

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003B2A4 File Offset: 0x000394A4
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003B2D5 File Offset: 0x000394D5
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003B2E3 File Offset: 0x000394E3
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003B2FD File Offset: 0x000394FD
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0003B30A File Offset: 0x0003950A
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06000AE2 RID: 2786
		public abstract string GetUniqueLoadID();

		// Token: 0x04000912 RID: 2322
		public AreaManager areaManager;

		// Token: 0x04000913 RID: 2323
		public int ID = -1;

		// Token: 0x04000914 RID: 2324
		private BoolGrid innerGrid;

		// Token: 0x04000915 RID: 2325
		private CellBoolDrawer drawer;

		// Token: 0x04000916 RID: 2326
		private Texture2D colorTextureInt;
	}
}
