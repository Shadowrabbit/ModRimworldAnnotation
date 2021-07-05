using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000607 RID: 1543
	public class RegionCostCalculator
	{
		// Token: 0x06002C67 RID: 11367 RVA: 0x00108B8C File Offset: 0x00106D8C
		public RegionCostCalculator(Map map)
		{
			this.map = map;
			this.preciseRegionLinkDistancesDistanceGetter = new Func<int, int, float>(this.PreciseRegionLinkDistancesDistanceGetter);
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x00108C00 File Offset: 0x00106E00
		public void Init(CellRect destination, HashSet<Region> destRegions, TraverseParms parms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted)
		{
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.traverseParms = parms;
			this.pathingContext = this.map.pathing.For(parms);
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

		// Token: 0x06002C69 RID: 11369 RVA: 0x00108E54 File Offset: 0x00107054
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
							num2 = PathFinder.GetBuildingCost(otherRegion.door, this.traverseParms, this.traverseParms.pawn, null);
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

		// Token: 0x06002C6A RID: 11370 RVA: 0x00109070 File Offset: 0x00107270
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

		// Token: 0x06002C6B RID: 11371 RVA: 0x001090EC File Offset: 0x001072EC
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

		// Token: 0x06002C6C RID: 11372 RVA: 0x001091AC File Offset: 0x001073AC
		private int GetCellCostFast(int index, bool ignoreAllowedAreaCost = false)
		{
			int num = this.pathingContext.pathGrid.pathGrid[index];
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

		// Token: 0x06002C6D RID: 11373 RVA: 0x00109248 File Offset: 0x00107448
		private int RegionLinkDistance(RegionLink a, RegionLink b, int minPathCost)
		{
			IntVec3 a2 = this.linkTargetCells.ContainsKey(a) ? this.linkTargetCells[a] : RegionCostCalculator.RegionLinkCenter(a);
			IntVec3 b2 = this.linkTargetCells.ContainsKey(b) ? this.linkTargetCells[b] : RegionCostCalculator.RegionLinkCenter(b);
			IntVec3 intVec = a2 - b2;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x001092D4 File Offset: 0x001074D4
		public int RegionLinkDistance(IntVec3 cell, RegionLink link, int minPathCost)
		{
			IntVec3 linkTargetCell = this.GetLinkTargetCell(cell, link);
			IntVec3 intVec = cell - linkTargetCell;
			int num = Math.Abs(intVec.x);
			int num2 = Math.Abs(intVec.z);
			return this.OctileDistance(num, num2) + minPathCost * Math.Max(num, num2) + minPathCost * Math.Min(num, num2);
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x00109324 File Offset: 0x00107524
		private static int SpanCenterX(EdgeSpan e)
		{
			return e.root.x + ((e.dir == SpanDirection.East) ? (e.length / 2) : 0);
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x00109346 File Offset: 0x00107546
		private static int SpanCenterZ(EdgeSpan e)
		{
			return e.root.z + ((e.dir == SpanDirection.North) ? (e.length / 2) : 0);
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x00109367 File Offset: 0x00107567
		private static IntVec3 RegionLinkCenter(RegionLink link)
		{
			return new IntVec3(RegionCostCalculator.SpanCenterX(link.span), 0, RegionCostCalculator.SpanCenterZ(link.span));
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x00109388 File Offset: 0x00107588
		private int MinimumRegionLinkDistance(IntVec3 cell, RegionLink link)
		{
			IntVec3 intVec = cell - RegionCostCalculator.LinkClosestCell(cell, link);
			return this.OctileDistance(Math.Abs(intVec.x), Math.Abs(intVec.z));
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x001093BF File Offset: 0x001075BF
		private int OctileDistance(int dx, int dz)
		{
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x001093D4 File Offset: 0x001075D4
		private IntVec3 GetLinkTargetCell(IntVec3 cell, RegionLink link)
		{
			return RegionCostCalculator.LinkClosestCell(cell, link);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x001093E0 File Offset: 0x001075E0
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

		// Token: 0x06002C76 RID: 11382 RVA: 0x00109454 File Offset: 0x00107654
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
						Log.ErrorOnce("Dijkstra couldn't reach one of the cells even though they are in the same region. There is most likely something wrong with the neighbor nodes getter.", 1938471531);
						num = 100f;
					}
					outDistances.Add(new Pair<RegionLink, int>(regionLink, (int)num));
				}
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x001095E4 File Offset: 0x001077E4
		private IEnumerable<int> PreciseRegionLinkDistancesNeighborsGetter(int node, Region region)
		{
			if (this.regionGrid[node] == null || this.regionGrid[node] != region)
			{
				return null;
			}
			return this.PathableNeighborIndices(node);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x00109604 File Offset: 0x00107804
		private float PreciseRegionLinkDistancesDistanceGetter(int a, int b)
		{
			return (float)(this.GetCellCostFast(b, false) + (this.AreCellsDiagonal(a, b) ? this.moveTicksDiagonal : this.moveTicksCardinal));
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x00109628 File Offset: 0x00107828
		private bool AreCellsDiagonal(int a, int b)
		{
			int x = this.map.Size.x;
			return a % x != b % x && a / x != b / x;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x0010965C File Offset: 0x0010785C
		private List<int> PathableNeighborIndices(int index)
		{
			RegionCostCalculator.tmpPathableNeighborIndices.Clear();
			PathGrid pathGrid = this.pathingContext.pathGrid;
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
			bool canBashFences = this.traverseParms.canBashFences;
			bool flag5 = !flag || PathFinder.BlocksDiagonalMovement(index - 1, this.pathingContext, canBashFences);
			bool flag6 = !flag2 || PathFinder.BlocksDiagonalMovement(index + 1, this.pathingContext, canBashFences);
			if (flag3 && !PathFinder.BlocksDiagonalMovement(index - x, this.pathingContext, canBashFences))
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
			if (flag4 && !PathFinder.BlocksDiagonalMovement(index + x, this.pathingContext, canBashFences))
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

		// Token: 0x04001B0D RID: 6925
		private Map map;

		// Token: 0x04001B0E RID: 6926
		private Region[] regionGrid;

		// Token: 0x04001B0F RID: 6927
		private TraverseParms traverseParms;

		// Token: 0x04001B10 RID: 6928
		private PathingContext pathingContext;

		// Token: 0x04001B11 RID: 6929
		private IntVec3 destinationCell;

		// Token: 0x04001B12 RID: 6930
		private int moveTicksCardinal;

		// Token: 0x04001B13 RID: 6931
		private int moveTicksDiagonal;

		// Token: 0x04001B14 RID: 6932
		private ByteGrid avoidGrid;

		// Token: 0x04001B15 RID: 6933
		private Area allowedArea;

		// Token: 0x04001B16 RID: 6934
		private bool drafted;

		// Token: 0x04001B17 RID: 6935
		private Func<int, int, float> preciseRegionLinkDistancesDistanceGetter;

		// Token: 0x04001B18 RID: 6936
		private Dictionary<int, RegionLink> regionMinLink = new Dictionary<int, RegionLink>();

		// Token: 0x04001B19 RID: 6937
		private Dictionary<RegionLink, int> distances = new Dictionary<RegionLink, int>();

		// Token: 0x04001B1A RID: 6938
		private FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry> queue = new FastPriorityQueue<RegionCostCalculator.RegionLinkQueueEntry>(new RegionCostCalculator.DistanceComparer());

		// Token: 0x04001B1B RID: 6939
		private Dictionary<Region, int> minPathCosts = new Dictionary<Region, int>();

		// Token: 0x04001B1C RID: 6940
		private List<Pair<RegionLink, int>> preciseRegionLinkDistances = new List<Pair<RegionLink, int>>();

		// Token: 0x04001B1D RID: 6941
		private Dictionary<RegionLink, IntVec3> linkTargetCells = new Dictionary<RegionLink, IntVec3>();

		// Token: 0x04001B1E RID: 6942
		private const int SampleCount = 11;

		// Token: 0x04001B1F RID: 6943
		private static int[] pathCostSamples = new int[11];

		// Token: 0x04001B20 RID: 6944
		private static List<int> tmpCellIndices = new List<int>();

		// Token: 0x04001B21 RID: 6945
		private static Dictionary<int, float> tmpDistances = new Dictionary<int, float>();

		// Token: 0x04001B22 RID: 6946
		private static List<int> tmpPathableNeighborIndices = new List<int>();

		// Token: 0x02001DB1 RID: 7601
		private struct RegionLinkQueueEntry
		{
			// Token: 0x17001A74 RID: 6772
			// (get) Token: 0x0600AB64 RID: 43876 RVA: 0x00390F6D File Offset: 0x0038F16D
			public Region From
			{
				get
				{
					return this.from;
				}
			}

			// Token: 0x17001A75 RID: 6773
			// (get) Token: 0x0600AB65 RID: 43877 RVA: 0x00390F75 File Offset: 0x0038F175
			public RegionLink Link
			{
				get
				{
					return this.link;
				}
			}

			// Token: 0x17001A76 RID: 6774
			// (get) Token: 0x0600AB66 RID: 43878 RVA: 0x00390F7D File Offset: 0x0038F17D
			public int Cost
			{
				get
				{
					return this.cost;
				}
			}

			// Token: 0x17001A77 RID: 6775
			// (get) Token: 0x0600AB67 RID: 43879 RVA: 0x00390F85 File Offset: 0x0038F185
			public int EstimatedPathCost
			{
				get
				{
					return this.estimatedPathCost;
				}
			}

			// Token: 0x0600AB68 RID: 43880 RVA: 0x00390F8D File Offset: 0x0038F18D
			public RegionLinkQueueEntry(Region from, RegionLink link, int cost, int estimatedPathCost)
			{
				this.from = from;
				this.link = link;
				this.cost = cost;
				this.estimatedPathCost = estimatedPathCost;
			}

			// Token: 0x04007213 RID: 29203
			private Region from;

			// Token: 0x04007214 RID: 29204
			private RegionLink link;

			// Token: 0x04007215 RID: 29205
			private int cost;

			// Token: 0x04007216 RID: 29206
			private int estimatedPathCost;
		}

		// Token: 0x02001DB2 RID: 7602
		private class DistanceComparer : IComparer<RegionCostCalculator.RegionLinkQueueEntry>
		{
			// Token: 0x0600AB69 RID: 43881 RVA: 0x00390FAC File Offset: 0x0038F1AC
			public int Compare(RegionCostCalculator.RegionLinkQueueEntry a, RegionCostCalculator.RegionLinkQueueEntry b)
			{
				return a.EstimatedPathCost.CompareTo(b.EstimatedPathCost);
			}
		}
	}
}
