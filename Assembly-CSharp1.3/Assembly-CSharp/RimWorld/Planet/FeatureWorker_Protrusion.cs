using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200173D RID: 5949
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06008936 RID: 35126 RVA: 0x0031434C File Offset: 0x0031254C
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17001640 RID: 5696
		// (get) Token: 0x06008937 RID: 35127 RVA: 0x00314359 File Offset: 0x00312559
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x06008938 RID: 35128 RVA: 0x003150CB File Offset: 0x003132CB
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06008939 RID: 35129 RVA: 0x003150D8 File Offset: 0x003132D8
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		// Token: 0x0600893A RID: 35130
		protected abstract bool IsRoot(int tile);

		// Token: 0x0600893B RID: 35131 RVA: 0x00314AAC File Offset: 0x00312CAC
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600893C RID: 35132 RVA: 0x003150E5 File Offset: 0x003132E5
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRoots();
			this.CalculateRootsWithoutSmallPassages();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600893D RID: 35133 RVA: 0x003150FC File Offset: 0x003132FC
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

		// Token: 0x0600893E RID: 35134 RVA: 0x0031515C File Offset: 0x0031335C
		private void CalculateRootsWithoutSmallPassages()
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		// Token: 0x0600893F RID: 35135 RVA: 0x003151B4 File Offset: 0x003133B4
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

		// Token: 0x0400570D RID: 22285
		private List<int> roots = new List<int>();

		// Token: 0x0400570E RID: 22286
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x0400570F RID: 22287
		private List<int> rootsWithoutSmallPassages = new List<int>();

		// Token: 0x04005710 RID: 22288
		private HashSet<int> rootsWithoutSmallPassagesSet = new HashSet<int>();

		// Token: 0x04005711 RID: 22289
		private List<int> currentGroup = new List<int>();

		// Token: 0x04005712 RID: 22290
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04005713 RID: 22291
		private static List<int> tmpGroup = new List<int>();
	}
}
