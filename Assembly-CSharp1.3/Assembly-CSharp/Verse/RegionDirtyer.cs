﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000200 RID: 512
	public class RegionDirtyer
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0005258F File Offset: 0x0005078F
		public bool AnyDirty
		{
			get
			{
				return this.dirtyCells.Count > 0;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005259F File Offset: 0x0005079F
		public List<IntVec3> DirtyCells
		{
			get
			{
				return this.dirtyCells;
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000525A7 File Offset: 0x000507A7
		public RegionDirtyer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000525CC File Offset: 0x000507CC
		internal void Notify_WalkabilityChanged(IntVec3 c, bool newWalkability)
		{
			this.regionsToDirty.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(this.map))
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2);
					if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.valid)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, regionAt_NoRebuild_InvalidAllowed);
						this.regionsToDirty.Add(regionAt_NoRebuild_InvalidAllowed);
					}
				}
			}
			for (int j = 0; j < this.regionsToDirty.Count; j++)
			{
				this.SetRegionDirty(this.regionsToDirty[j], true);
			}
			this.regionsToDirty.Clear();
			if (newWalkability && !this.dirtyCells.Contains(c))
			{
				this.dirtyCells.Add(c);
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x000526A0 File Offset: 0x000508A0
		internal void Notify_ThingAffectingRegionsSpawned(Thing b)
		{
			this.regionsToDirty.Clear();
			foreach (IntVec3 c in b.OccupiedRect().ExpandedBy(1).ClipInsideMap(b.Map))
			{
				Region validRegionAt_NoRebuild = b.Map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					b.Map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild);
					this.regionsToDirty.Add(validRegionAt_NoRebuild);
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00052784 File Offset: 0x00050984
		internal void Notify_ThingAffectingRegionsDespawned(Thing b)
		{
			this.regionsToDirty.Clear();
			Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(b.Position);
			if (validRegionAt_NoRebuild != null)
			{
				this.map.temperatureCache.TryCacheRegionTempInfo(b.Position, validRegionAt_NoRebuild);
				this.regionsToDirty.Add(validRegionAt_NoRebuild);
			}
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(b))
			{
				if (c.InBounds(this.map))
				{
					Region validRegionAt_NoRebuild2 = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
					if (validRegionAt_NoRebuild2 != null)
					{
						this.map.temperatureCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild2);
						this.regionsToDirty.Add(validRegionAt_NoRebuild2);
					}
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
			if (b.def.size.x == 1 && b.def.size.z == 1)
			{
				this.dirtyCells.Add(b.Position);
				return;
			}
			CellRect cellRect = b.OccupiedRect();
			for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
			{
				for (int k = cellRect.minX; k <= cellRect.maxX; k++)
				{
					IntVec3 item = new IntVec3(k, 0, j);
					this.dirtyCells.Add(item);
				}
			}
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00052920 File Offset: 0x00050B20
		internal void SetAllClean()
		{
			for (int i = 0; i < this.dirtyCells.Count; i++)
			{
				this.map.temperatureCache.ResetCachedCellInfo(this.dirtyCells[i]);
			}
			this.dirtyCells.Clear();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0005296C File Offset: 0x00050B6C
		private void SetRegionDirty(Region reg, bool addCellsToDirtyCells = true)
		{
			if (!reg.valid)
			{
				return;
			}
			reg.valid = false;
			reg.District = null;
			for (int i = 0; i < reg.links.Count; i++)
			{
				reg.links[i].Deregister(reg);
			}
			reg.links.Clear();
			if (addCellsToDirtyCells)
			{
				foreach (IntVec3 intVec in reg.Cells)
				{
					this.dirtyCells.Add(intVec);
					if (DebugViewSettings.drawRegionDirties)
					{
						this.map.debugDrawer.FlashCell(intVec, 0f, null, 50);
					}
				}
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00052A2C File Offset: 0x00050C2C
		internal void SetAllDirty()
		{
			this.dirtyCells.Clear();
			foreach (IntVec3 item in this.map)
			{
				this.dirtyCells.Add(item);
			}
			foreach (Region reg in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				this.SetRegionDirty(reg, false);
			}
		}

		// Token: 0x04000BBC RID: 3004
		private Map map;

		// Token: 0x04000BBD RID: 3005
		private List<IntVec3> dirtyCells = new List<IntVec3>();

		// Token: 0x04000BBE RID: 3006
		private List<Region> regionsToDirty = new List<Region>();
	}
}
