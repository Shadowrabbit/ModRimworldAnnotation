using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002026 RID: 8230
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x0600AE4A RID: 44618 RVA: 0x0007143D File Offset: 0x0006F63D
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x0600AE4B RID: 44619 RVA: 0x0007144A File Offset: 0x0006F64A
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x0600AE4C RID: 44620 RVA: 0x00071701 File Offset: 0x0006F901
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x0600AE4D RID: 44621 RVA: 0x0007170E File Offset: 0x0006F90E
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		// Token: 0x0600AE4E RID: 44622
		protected abstract bool IsRoot(int tile);

		// Token: 0x0600AE4F RID: 44623 RVA: 0x000715E2 File Offset: 0x0006F7E2
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600AE50 RID: 44624 RVA: 0x0007171B File Offset: 0x0006F91B
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRoots();
			this.CalculateRootsWithoutSmallPassages();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600AE51 RID: 44625 RVA: 0x0032B584 File Offset: 0x00329784
		private void CalculateRoots()
		{
			this.roots.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
		}

		// Token: 0x0600AE52 RID: 44626 RVA: 0x0032B5E4 File Offset: 0x003297E4
		private void CalculateRootsWithoutSmallPassages()
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		// Token: 0x0600AE53 RID: 44627 RVA: 0x0032B63C File Offset: 0x0032983C
		private void CalculateContiguousGroups()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			float maxPctOfWholeArea = this.MaxPctOfWholeArea;
			int maxPassageWidth = this.MaxPassageWidth;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			Predicate<int> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				int num = this.roots[i];
				if (!FeatureWorker.visited[num])
				{
					FeatureWorker_Protrusion.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
					int rootTile = num;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int x) => this.rootsSet.Contains(x)));
					}
					worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_Protrusion.tmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Protrusion.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Protrusion.tmpGroup[j]] = FeatureWorker_Protrusion.tmpGroup.Count;
					}
				}
			}
			FeatureWorker.ClearVisited();
			Predicate<int> <>9__2;
			Action<int> <>9__3;
			Predicate<int> <>9__4;
			Predicate<int> <>9__5;
			Predicate<int> <>9__6;
			Predicate<int> <>9__7;
			for (int k = 0; k < this.rootsWithoutSmallPassages.Count; k++)
			{
				int num2 = this.rootsWithoutSmallPassages[k];
				if (!FeatureWorker.visited[num2])
				{
					this.currentGroup.Clear();
					WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
					int rootTile2 = num2;
					Predicate<int> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((int x) => this.rootsWithoutSmallPassagesSet.Contains(x)));
					}
					Action<int> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							this.currentGroup.Add(x);
						});
					}
					worldFloodFiller3.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minSize)
					{
						List<int> tiles = this.currentGroup;
						int count = maxPassageWidth * 2;
						Predicate<int> extraPredicate;
						if ((extraPredicate = <>9__4) == null)
						{
							extraPredicate = (<>9__4 = ((int x) => this.rootsSet.Contains(x)));
						}
						GenPlanetMorphology.Dilate(tiles, count, extraPredicate);
						if (this.currentGroup.Count <= maxSize && (float)this.currentGroup.Count / (float)FeatureWorker.groupSize[num2] <= maxPctOfWholeArea)
						{
							if (!this.def.canTouchWorldEdge)
							{
								List<int> list = this.currentGroup;
								Predicate<int> predicate;
								if ((predicate = <>9__5) == null)
								{
									predicate = (<>9__5 = ((int x) => worldGrid.IsOnEdge(x)));
								}
								if (list.Any(predicate))
								{
									goto IL_30D;
								}
							}
							this.currentGroupMembers.Clear();
							for (int l = 0; l < this.currentGroup.Count; l++)
							{
								if (this.IsMember(this.currentGroup[l]))
								{
									this.currentGroupMembers.Add(this.currentGroup[l]);
								}
							}
							if (this.currentGroupMembers.Count >= minSize)
							{
								List<int> list2 = this.currentGroup;
								Predicate<int> predicate2;
								if ((predicate2 = <>9__6) == null)
								{
									predicate2 = (<>9__6 = ((int x) => worldGrid[x].feature == null));
								}
								if (list2.Any(predicate2))
								{
									List<int> list3 = this.currentGroup;
									Predicate<int> match;
									if ((match = <>9__7) == null)
									{
										match = (<>9__7 = ((int x) => worldGrid[x].feature != null));
									}
									list3.RemoveAll(match);
								}
								base.AddFeature(this.currentGroupMembers, this.currentGroup);
							}
						}
					}
				}
				IL_30D:;
			}
		}

		// Token: 0x040077A6 RID: 30630
		private List<int> roots = new List<int>();

		// Token: 0x040077A7 RID: 30631
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x040077A8 RID: 30632
		private List<int> rootsWithoutSmallPassages = new List<int>();

		// Token: 0x040077A9 RID: 30633
		private HashSet<int> rootsWithoutSmallPassagesSet = new HashSet<int>();

		// Token: 0x040077AA RID: 30634
		private List<int> currentGroup = new List<int>();

		// Token: 0x040077AB RID: 30635
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x040077AC RID: 30636
		private static List<int> tmpGroup = new List<int>();
	}
}
