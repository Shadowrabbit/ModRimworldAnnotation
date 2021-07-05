using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A60 RID: 2656
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x06003F35 RID: 16181 RVA: 0x0002F75B File Offset: 0x0002D95B
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0017CA64 File Offset: 0x0017AC64
		public void Init(CellRect end, TraverseParms traverseParms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				foreach (IntVec3 intVec in end)
				{
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null && region2.Allows(traverseParms, true))
						{
							this.destRegions.Add(region2);
						}
					}
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.", false);
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0017CBD4 File Offset: 0x0017ADD4
		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, 1);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, 1);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				return num3 + this.OctileDistanceToEndEps(cell);
			}
			return 10000;
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0017CCD0 File Offset: 0x0017AED0
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0017CD20 File Offset: 0x0017AF20
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x04002B87 RID: 11143
		private Map map;

		// Token: 0x04002B88 RID: 11144
		private IntVec3 endCell;

		// Token: 0x04002B89 RID: 11145
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x04002B8A RID: 11146
		private int moveTicksCardinal;

		// Token: 0x04002B8B RID: 11147
		private int moveTicksDiagonal;

		// Token: 0x04002B8C RID: 11148
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x04002B8D RID: 11149
		private Region cachedRegion;

		// Token: 0x04002B8E RID: 11150
		private RegionLink cachedBestLink;

		// Token: 0x04002B8F RID: 11151
		private RegionLink cachedSecondBestLink;

		// Token: 0x04002B90 RID: 11152
		private int cachedBestLinkCost;

		// Token: 0x04002B91 RID: 11153
		private int cachedSecondBestLinkCost;

		// Token: 0x04002B92 RID: 11154
		private bool cachedRegionIsDestination;

		// Token: 0x04002B93 RID: 11155
		private Region[] regionGrid;
	}
}
