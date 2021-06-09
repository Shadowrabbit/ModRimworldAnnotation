using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002E2 RID: 738
	public static class RegionTraverser
	{
		// Token: 0x060012C8 RID: 4808 RVA: 0x000C8388 File Offset: 0x000C6588
		public static Room FloodAndSetRooms(Region root, Map map, Room existingRoom)
		{
			Room floodingRoom;
			if (existingRoom == null)
			{
				floodingRoom = Room.MakeNew(map);
			}
			else
			{
				floodingRoom = existingRoom;
			}
			root.Room = floodingRoom;
			if (!root.type.AllowsMultipleRegionsPerRoom())
			{
				return floodingRoom;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.Room != floodingRoom;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.Room = floodingRoom;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingRoom;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000C8418 File Offset: 0x000C6618
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerRoom())
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

		// Token: 0x060012CA RID: 4810 RVA: 0x000C848C File Offset: 0x000C668C
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

		// Token: 0x060012CB RID: 4811 RVA: 0x000C850C File Offset: 0x000C670C
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000136F7 File Offset: 0x000118F7
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00013702 File Offset: 0x00011902
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000C853C File Offset: 0x000C673C
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000C8574 File Offset: 0x000C6774
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x000C859C File Offset: 0x000C679C
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers + ", or a bug has put this system in an inconsistent state. Resetting.", false);
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.", false);
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString(), false);
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x04000F00 RID: 3840
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x04000F01 RID: 3841
		public static int NumWorkers = 8;

		// Token: 0x04000F02 RID: 3842
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x020002E3 RID: 739
		private class BFSWorker
		{
			// Token: 0x060012D1 RID: 4817 RVA: 0x0001372E File Offset: 0x0001192E
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x060012D2 RID: 4818 RVA: 0x0001374F File Offset: 0x0001194F
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x060012D3 RID: 4819 RVA: 0x000C8644 File Offset: 0x000C6844
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x060012D4 RID: 4820 RVA: 0x00006A05 File Offset: 0x00004C05
			private void FinalizeSearch()
			{
			}

			// Token: 0x060012D5 RID: 4821 RVA: 0x000C869C File Offset: 0x000C689C
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

			// Token: 0x04000F03 RID: 3843
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x04000F04 RID: 3844
			private int numRegionsProcessed;

			// Token: 0x04000F05 RID: 3845
			private uint closedIndex = 1U;

			// Token: 0x04000F06 RID: 3846
			private int closedArrayPos;

			// Token: 0x04000F07 RID: 3847
			private const int skippableRegionSize = 4;
		}
	}
}
