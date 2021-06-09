using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001263 RID: 4707
	public sealed class FertilityGrid
	{
		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x001F9B60 File Offset: 0x001F7D60
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3610, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x00046216 File Offset: 0x00044416
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x00046225 File Offset: 0x00044425
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x001F9BD4 File Offset: 0x001F7DD4
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			return this.map.terrainGrid.TerrainAt(loc).fertility;
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x0004622E File Offset: 0x0004442E
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x001F9C20 File Offset: 0x001F7E20
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.FertilityAt(intVec) > 0.69f;
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x001F9C70 File Offset: 0x001F7E70
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			float num = this.FertilityAt(CellIndicesUtility.IndexToCell(index, this.map.Size.x));
			if (num <= 0.95f)
			{
				return FertilityGrid.LowFertilityColor;
			}
			if (num <= 1.1f)
			{
				return FertilityGrid.MediumFertilityColor;
			}
			if (num >= 1.1f)
			{
				return FertilityGrid.HighFertilityColor;
			}
			return Color.white;
		}

		// Token: 0x04004451 RID: 17489
		private Map map;

		// Token: 0x04004452 RID: 17490
		private CellBoolDrawer drawerInt;

		// Token: 0x04004453 RID: 17491
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		// Token: 0x04004454 RID: 17492
		private static readonly Color LowFertilityColor = Color.yellow;

		// Token: 0x04004455 RID: 17493
		private static readonly Color HighFertilityColor = Color.green;
	}
}
