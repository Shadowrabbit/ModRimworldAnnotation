using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C78 RID: 3192
	public sealed class FertilityGrid
	{
		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x00189ED4 File Offset: 0x001880D4
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

		// Token: 0x06004A6E RID: 19054 RVA: 0x00189F48 File Offset: 0x00188148
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x00189F57 File Offset: 0x00188157
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00189F60 File Offset: 0x00188160
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			return this.map.terrainGrid.TerrainAt(loc).fertility;
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x00189FAC File Offset: 0x001881AC
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x0001A4C7 File Offset: 0x000186C7
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x00189FD0 File Offset: 0x001881D0
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.FertilityAt(intVec) > 0.69f;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0018A020 File Offset: 0x00188220
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

		// Token: 0x04002D38 RID: 11576
		private Map map;

		// Token: 0x04002D39 RID: 11577
		private CellBoolDrawer drawerInt;

		// Token: 0x04002D3A RID: 11578
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		// Token: 0x04002D3B RID: 11579
		private static readonly Color LowFertilityColor = Color.yellow;

		// Token: 0x04002D3C RID: 11580
		private static readonly Color HighFertilityColor = Color.green;
	}
}
