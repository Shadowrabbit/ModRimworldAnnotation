using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000608 RID: 1544
	public class RegionCostCalculatorWrapper
	{
		// Token: 0x06002C7C RID: 11388 RVA: 0x00109845 File Offset: 0x00107A45
		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x0010986C File Offset: 0x00107A6C
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
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.");
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x001099DC File Offset: 0x00107BDC
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

		// Token: 0x06002C7F RID: 11391 RVA: 0x00109AD8 File Offset: 0x00107CD8
		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x00109B28 File Offset: 0x00107D28
		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}

		// Token: 0x04001B23 RID: 6947
		private Map map;

		// Token: 0x04001B24 RID: 6948
		private IntVec3 endCell;

		// Token: 0x04001B25 RID: 6949
		private HashSet<Region> destRegions = new HashSet<Region>();

		// Token: 0x04001B26 RID: 6950
		private int moveTicksCardinal;

		// Token: 0x04001B27 RID: 6951
		private int moveTicksDiagonal;

		// Token: 0x04001B28 RID: 6952
		private RegionCostCalculator regionCostCalculator;

		// Token: 0x04001B29 RID: 6953
		private Region cachedRegion;

		// Token: 0x04001B2A RID: 6954
		private RegionLink cachedBestLink;

		// Token: 0x04001B2B RID: 6955
		private RegionLink cachedSecondBestLink;

		// Token: 0x04001B2C RID: 6956
		private int cachedBestLinkCost;

		// Token: 0x04001B2D RID: 6957
		private int cachedSecondBestLinkCost;

		// Token: 0x04001B2E RID: 6958
		private bool cachedRegionIsDestination;

		// Token: 0x04001B2F RID: 6959
		private Region[] regionGrid;
	}
}
