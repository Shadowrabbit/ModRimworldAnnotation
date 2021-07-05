using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200020A RID: 522
	public static class RegionTraverser
	{
		// Token: 0x06000EC1 RID: 3777 RVA: 0x00053B70 File Offset: 0x00051D70
		public static District FloodAndSetDistricts(Region root, Map map, District existingRoom)
		{
			District floodingDistrict;
			if (existingRoom == null)
			{
				floodingDistrict = District.MakeNew(map);
			}
			else
			{
				floodingDistrict = existingRoom;
			}
			root.District = floodingDistrict;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return floodingDistrict;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.District != floodingDistrict;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.District = floodingDistrict;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingDistrict;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00053C00 File Offset: 0x00051E00
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.newRegionGroupIndex = newRegionGroupIndex;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00053C74 File Offset: 0x00051E74
		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return false;
			}
			Region regB = B.GetRegion(map, traversableRegionTypes);
			if (regB == null)
			{
				return false;
			}
			if (region == regB)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			bool found = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r == regB)
				{
					found = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
			return found;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00053CF4 File Offset: 0x00051EF4
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00053D24 File Offset: 0x00051F24
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00053D2F File Offset: 0x00051F2F
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00053D5C File Offset: 0x00051F5C
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00053D94 File Offset: 0x00051F94
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00053DBC File Offset: 0x00051FBC
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.");
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.");
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString());
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x04000BD8 RID: 3032
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x04000BD9 RID: 3033
		public static int NumWorkers = 8;

		// Token: 0x04000BDA RID: 3034
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x0200198A RID: 6538
		private class BFSWorker
		{
			// Token: 0x060098FD RID: 39165 RVA: 0x003603D4 File Offset: 0x0035E5D4
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x060098FE RID: 39166 RVA: 0x003603F5 File Offset: 0x0035E5F5
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x060098FF RID: 39167 RVA: 0x00360404 File Offset: 0x0035E604
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x06009900 RID: 39168 RVA: 0x0000313F File Offset: 0x0000133F
			private void FinalizeSearch()
			{
			}

			// Token: 0x06009901 RID: 39169 RVA: 0x0036045C File Offset: 0x0035E65C
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) == RegionType.None)
				{
					return;
				}
				this.closedIndex += 1U;
				this.open.Clear();
				this.numRegionsProcessed = 0;
				this.QueueNewOpenRegion(root);
				while (this.open.Count > 0)
				{
					Region region = this.open.Dequeue();
					if (DebugViewSettings.drawRegionTraversal)
					{
						region.Debug_Notify_Traversed();
					}
					if (regionProcessor != null && regionProcessor(region))
					{
						this.FinalizeSearch();
						return;
					}
					if (RegionTraverser.ShouldCountRegion(region))
					{
						this.numRegionsProcessed++;
					}
					if (this.numRegionsProcessed >= maxRegions)
					{
						this.FinalizeSearch();
						return;
					}
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							Region region2 = regionLink.regions[j];
							if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
							{
								this.QueueNewOpenRegion(region2);
							}
						}
					}
				}
				this.FinalizeSearch();
			}

			// Token: 0x04006209 RID: 25097
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x0400620A RID: 25098
			private int numRegionsProcessed;

			// Token: 0x0400620B RID: 25099
			private uint closedIndex = 1U;

			// Token: 0x0400620C RID: 25100
			private int closedArrayPos;

			// Token: 0x0400620D RID: 25101
			private const int skippableRegionSize = 4;
		}
	}
}
