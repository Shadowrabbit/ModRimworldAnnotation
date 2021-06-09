using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A5C RID: 2652
	public class RegionCostCalculator
	{
		// Token: 0x06003F17 RID: 16151 RVA: 0x0017BEAC File Offset: 0x0017A0AC
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0017BF20 File Offset: 0x0017A120
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.destinationCell = destination.CenterCell;
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.avoidGrid = avoidGrid;
			this.allowedArea = allowedArea;
			this.drafted = drafted;
			this.regionMinLink.Clear();
			this.distances.Clear();
			this.linkTargetCells.Clear();
			this.queue.Clear();
			this.minPathCosts.Clear();
			foreach (Region region in destRegions)
			{
				int minPathCost = this.RegionMedianPathCost(region);
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
					{
						int num = this.RegionLinkDistance(this.destinationCell, regionLink, minPathCost);
						int num2;
						if (this.distances.TryGetValue(regionLink, out num2))
						{
							if (num < num2)
							{
								this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
							}
							num = Math.Min(num2, num);
						}
						else
						{
							this.linkTargetCells[regionLink] = this.GetLinkTargetCell(this.destinationCell, regionLink);
						}
						this.distances[regionLink] = num;
					}
				}
				this.GetPreciseRegionLinkDistances(region, destination, this.preciseRegionLinkDistances);
				for (int j = 0; j < this.preciseRegionLinkDistances.Count; j++)
				{
					Pair<RegionLink, int> pair = this.preciseRegionLinkDistances[j];
					RegionLink first = pair.First;
					int num3 = this.distances[first];
					int num4;
					if (pair.Second > num3)
					{
						this.distances[first] = pair.Second;
						num4 = pair.Second;
					}
					else
					{
						num4 = num3;
					}
					this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(region, first, num4, num4));
				}
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0017C15C File Offset: 0x0017A35C
		public int GetRegionDistance(Region region, out RegionLink minLink)
		{
			if (this.regionMinLink.TryGetValue(region.id, out minLink))
			{
				return this.distances[minLink];
			}
			while (this.queue.Count != 0)
			{
				RegionCostCalculator.RegionLinkQueueEntry regionLinkQueueEntry = this.queue.Pop();
				int num = this.distances[regionLinkQueueEntry.Link];
				if (regionLinkQueueEntry.Cost == num)
				{
					Region otherRegion = regionLinkQueueEntry.Link.GetOtherRegion(regionLinkQueueEntry.From);
					if (otherRegion != null && otherRegion.valid)
					{
						int num2 = 0;
						if (otherRegion.door != null)
						{
							num2 = PathFinder.GetBuildingCost(otherRegion.door, this.traverseParms, this.traverseParms.pawn);
							if (num2 == 2147483647)
							{
								continue;
							}
							num2 += this.OctileDistance(1, 0);
						}
						int minPathCost = this.RegionMedianPathCost(otherRegion);
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							RegionLink regionLink = otherRegion.links[i];
							if (regionLink != regionLinkQueueEntry.Link && regionLink.GetOtherRegion(otherRegion).type.Passable())
							{
								int num3 = (otherRegion.door != null) ? num2 : this.RegionLinkDistance(regionLinkQueueEntry.Link, regionLink, minPathCost);
								num3 = Math.Max(num3, 1);
								int num4 = num + num3;
								int estimatedPathCost = this.MinimumRegionLinkDistance(this.destinationCell, regionLink) + num4;
								int num5;
								if (this.distances.TryGetValue(regionLink, out num5))
								{
									if (num4 < num5)
									{
										this.distances[regionLink] = num4;
										this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
									}
								}
								else
								{
									this.distances.Add(regionLink, num4);
									this.queue.Push(new RegionCostCalculator.RegionLinkQueueEntry(otherRegion, regionLink, num4, estimatedPathCost));
								}
							}
						}
						if (!this.regionMinLink.ContainsKey(otherRegion.id))
						{
							this.regionMinLink.Add(otherRegion.id, regionLinkQueueEntry.Link);
							if (otherRegion == region)
							{
								minLink = regionLinkQueueEntry.Link;
								return regionLinkQueueEntry.Cost;
							}
						}
					}
				}
			}
			return 10000;
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0017C374 File Offset: 0x0017A574
		public int GetRegionBestDistances(Region region, out RegionLink bestLink, out RegionLink secondBestLink, out int secondBestCost)
		{
			int regionDistance = this.GetRegionDistance(region, out bestLink);
			secondBestLink = null;
			secondBestCost = int.MaxValue;
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				int num;
				if (regionLink != bestLink && regionLink.GetOtherRegion(region).type.Passable() && this.distances.TryGetValue(regionLink, out num) && num < secondBestCost)
				{
					secondBestCost = num;
					secondBestLink = regionLink;
				}
			}
			return regionDistance;
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0017C3F0 File Offset: 0x0017A5F0
		public int RegionMedianPathCost(Region region)
		{
			int result;
			if (this.minPathCosts.TryGetValue(region, out result))
			{
				return result;
			}
			bool ignoreAllowedAreaCost = this.allowedArea != null && region.OverlapWith(this.allowedArea) > AreaOverlap.None;
			CellIndices cellIndices = this.map.cellIndices;
			Rand.PushState();
			Rand.Seed = cellIndices.CellToIndex(region.extentsClose.CenterCell) * (region.links.Count + 1);
			for (int i = 0; i < 11; i++)
			{
				RegionCostCalculator.pathCostSamples[i] = this.GetCellCostFast(cellIndices.CellToIndex(region.RandomCell), ignoreAllowedAreaCost);
			}
			Rand.PopState();
			Array.Sort<int>(RegionCostCalculator.pathCostSamples);
			return this.minPathCosts[region] = RegionCostCalculator.pathCostSamples[4];
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0017C4B0 File Offset: 0x0017A6B0
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.map.pathGrid.pathGrid[index];
			if (this.avoidGrid != null)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedArea != null && !ignoreAllowedAreaCost && !this.allowedArea[index])
			{
				num += 600;
			}
			if (this.drafted)
			{
				num += this.map.terrainGrid.topGrid[index].extraDraftedPerceivedPathCost;
			}
			else
			{
				num += this.map.terrainGrid.topGrid[index].extraNonDraftedPerceivedPathCost;
			}
			return num;
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0017C54C File Offset: 0x0017A74C
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 a2 = this.linkTargetCells.ContainsKey(a) ? this.linkTargetCells[a] : RegionCostCalculator.RegionLinkCenter(a);
			IntVec3 b2 = this.linkTargetCells.ContainsKey(b) ? this.linkTargetCells[b] : RegionCostCalculator.RegionLinkCenter(b);
			IntVec3 intVec = a2 - b2;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0017C5D8 File Offset: 0x0017A7D8
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0002F619 File Offset: 0x0002D819
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0002F63B File Offset: 0x0002D83B
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0002F65C File Offset: 0x0002D85C
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0017C628 File Offset: 0x0017A828
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0002F67A File Offset: 0x0002D87A
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0002F68F File Offset: 0x0002D88F
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0017C660 File Offset: 0x0017A860
		private static IntVec3 LinkClosestCell(IntVec3 cell, RegionLink link)
		{
			EdgeSpan span = link.span;
			int num = 0;
			int num2 = 0;
			if (span.dir == SpanDirection.North)
			{
				num2 = span.length - 1;
			}
			else
			{
				num = span.length - 1;
			}
			IntVec3 root = span.root;
			return new IntVec3(Mathf.Clamp(cell.x, root.x, root.x + num), 0, Mathf.Clamp(cell.z, root.z, root.z + num2));
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0017C6D4 File Offset: 0x0017A8D4
		private void GetPreciseRegionLinkDistances(Region region, CellRect destination, List<Pair<RegionLink, int>> outDistances)
		{
			outDistances.Clear();
			RegionCostCalculator.tmpCellIndices.Clear();
			if (destination.Width == 1 && destination.Height == 1)
			{
				RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(destination.CenterCell));
			}
			else
			{
				foreach (IntVec3 c in destination)
				{
					if (c.InBounds(this.map))
					{
						RegionCostCalculator.tmpCellIndices.Add(this.map.cellIndices.CellToIndex(c));
					}
				}
			}
			Dijkstra<int>.Run(RegionCostCalculator.tmpCellIndices, (int x) => this.PreciseRegionLinkDistancesNeighborsGetter(x, region), this.preciseRegionLinkDistancesDistanceGetter, RegionCostCalculator.tmpDistances, null);
			for (int i = 0; i < region.links.Count; i++)
			{
				RegionLink regionLink = region.links[i];
				if (regionLink.GetOtherRegion(region).Allows(this.traverseParms, false))
				{
					float num;
					if (!RegionCostCalculator.tmpDistances.TryGetValue(this.map.cellIndices.CellToIndex(this.linkTargetCells[regionLink]), out num))
					{
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531, false);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x0002F698 File Offset: 0x0002D898
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				return null;
			}
			return this.PathableNeighborIndices(node);
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0002F6B8 File Offset: 0x0002D8B8
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)(this.GetCellCostFast(b, false) + (this.AreCellsDiagonal(a, b) ? this.moveTicksDiagonal : this.moveTicksCardinal));
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0017C864 File Offset: 0x0017AA64
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0017C898 File Offset: 0x0017AA98
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.map.pathGrid;
			int x = this.map.Size.x;
			bool flag = index % x > 0;
			bool flag2 = index % x < x - 1;
			bool flag3 = index >= x;
			bool flag4 = index / x < this.map.Size.z - 1;
			if (flag3 && pathGrid.WalkableFast(index - x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x);
			}
			if (flag2 && pathGrid.WalkableFast(index + 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + 1);
			}
			if (flag && pathGrid.WalkableFast(index - 1))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index - 1);
			}
			if (flag4 && pathGrid.WalkableFast(index + x))
			{
				RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x);
			}
			bool flag5 = !flag || PathFinder.BlocksDiagonalMovement(index - 1, this.map);
			bool flag6 = !flag2 || PathFinder.BlocksDiagonalMovement(index + 1, this.map);
			if (flag3 && !PathFinder.BlocksDiagonalMovement(index - x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index - x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index - x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index - x - 1);
				}
			}
			if (flag4 && !PathFinder.BlocksDiagonalMovement(index + x, this.map))
			{
				if (!flag6 && pathGrid.WalkableFast(index + x + 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x + 1);
				}
				if (!flag5 && pathGrid.WalkableFast(index + x - 1))
				{
					RegionCostCalculator.tmpPathableNeighborIndices.Add(index + x - 1);
				}
			}
			return RegionCostCalculator.tmpPathableNeighborIndices;
		}

		// Token: 0x04002B6C RID: 11116
		private Map map;

		// Token: 0x04002B6D RID: 11117
		private Region[] regionGrid;

		// Token: 0x04002B6E RID: 11118
		private TraverseParms traverseParms;

		// Token: 0x04002B6F RID: 11119
		private IntVec3 destinationCell;

		// Token: 0x04002B70 RID: 11120
		private int moveTicksCardinal;

		// Token: 0x04002B71 RID: 11121
		private int moveTicksDiagonal;

		// Token: 0x04002B72 RID: 11122
		private ByteGrid avoidGrid;

		// Token: 0x04002B73 RID: 11123
		private Area allowedArea;

		// Token: 0x04002B74 RID: 11124
		private bool drafted;

		// Token: 0x04002B75 RID: 11125
		private Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x04002B76 RID: 11126
		private Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x04002B77 RID: 11127
		private Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x04002B78 RID: 11128
		private FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x04002B79 RID: 11129
		private Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x04002B7A RID: 11130
		private List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x04002B7B RID: 11131
		private Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x04002B7C RID: 11132
		private const int SampleCount = 11;

		// Token: 0x04002B7D RID: 11133
		private static int[] pathCostSamples = new int[11];

		// Token: 0x04002B7E RID: 11134
		private static List<int> tmpCellIndices = new List<int>();

		// Token: 0x04002B7F RID: 11135
		private static Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x04002B80 RID: 11136
		private static List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x02000A5D RID: 2653
		private struct RegionLinkQueueEntry
		{
			// Token: 0x170009DB RID: 2523
			// (get) Token: 0x06003F2C RID: 16172 RVA: 0x0002F708 File Offset: 0x0002D908
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x170009DC RID: 2524
			// (get) Token: 0x06003F2D RID: 16173 RVA: 0x0002F710 File Offset: 0x0002D910
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x170009DD RID: 2525
			// (get) Token: 0x06003F2E RID: 16174 RVA: 0x0002F718 File Offset: 0x0002D918
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x170009DE RID: 2526
			// (get) Token: 0x06003F2F RID: 16175 RVA: 0x0002F720 File Offset: 0x0002D920
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			// Token: 0x06003F30 RID: 16176 RVA: 0x0002F728 File Offset: 0x0002D928
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x04002B81 RID: 11137
			private Region from;

			// Token: 0x04002B82 RID: 11138
			private RegionLink link;

			// Token: 0x04002B83 RID: 11139
			private int cost;

			// Token: 0x04002B84 RID: 11140
			private int estimatedPathCost;
		}

		// Token: 0x02000A5E RID: 2654
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x06003F31 RID: 16177 RVA: 0x0017CA40 File Offset: 0x0017AC40
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
