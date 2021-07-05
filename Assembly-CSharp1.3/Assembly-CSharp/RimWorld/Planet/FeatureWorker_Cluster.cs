using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001735 RID: 5941
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600890A RID: 35082 RVA: 0x00314325 File Offset: 0x00312525
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x0600890B RID: 35083 RVA: 0x00314332 File Offset: 0x00312532
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x0600890C RID: 35084 RVA: 0x0031433F File Offset: 0x0031253F
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x0600890D RID: 35085 RVA: 0x0031434C File Offset: 0x0031254C
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x0600890E RID: 35086 RVA: 0x00314359 File Offset: 0x00312559
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x0600890F RID: 35087 RVA: 0x00314366 File Offset: 0x00312566
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x06008910 RID: 35088
		protected abstract bool IsRoot(int tile);

		// Token: 0x06008911 RID: 35089 RVA: 0x00314373 File Offset: 0x00312573
		protected virtual bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x06008912 RID: 35090 RVA: 0x00314379 File Offset: 0x00312579
		protected virtual bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06008913 RID: 35091 RVA: 0x00314391 File Offset: 0x00312591
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootTiles();
			this.CalculateRootsWithAreaInBetween();
			this.CalculateContiguousGroups();
		}

		// Token: 0x06008914 RID: 35092 RVA: 0x003143A8 File Offset: 0x003125A8
		private void CalculateRootTiles()
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

		// Token: 0x06008915 RID: 35093 RVA: 0x00314408 File Offset: 0x00312608
		private void CalculateRootsWithAreaInBetween()
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x06008916 RID: 35094 RVA: 0x00314460 File Offset: 0x00312660
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int minRootGroupSize = this.MinRootGroupSize;
			int maxRootGroupSize = this.MaxRootGroupSize;
			int minOverallSize = this.MinOverallSize;
			int maxOverallSize = this.MaxOverallSize;
			int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			FeatureWorker.ClearGroupIDs();
			Predicate<int> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				int num = this.roots[i];
				if (!FeatureWorker.visited[num])
				{
					bool anyMember = false;
					FeatureWorker_Cluster.tmpGroup.Clear();
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
						FeatureWorker_Cluster.tmpGroup.Add(x);
						bool flag2;
						if (!anyMember && this.IsMember(x, out flag2))
						{
							anyMember = true;
						}
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
						if (anyMember)
						{
							FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
						}
					}
				}
			}
			FeatureWorker.ClearVisited();
			Predicate<int> <>9__2;
			Action<int> <>9__3;
			Predicate<int> <>9__4;
			Predicate<int> <>9__5;
			Predicate<int> <>9__6;
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2] && FeatureWorker.groupSize[num2] >= minRootGroupSize && FeatureWorker.groupSize[num2] <= maxRootGroupSize && FeatureWorker.groupSize[num2] <= maxOverallSize)
				{
					this.currentGroup.Clear();
					this.visitedValidGroupIDs.Clear();
					WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
					int rootTile2 = num2;
					Predicate<int> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = delegate(int x)
						{
							bool flag2;
							return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize));
						});
					}
					Action<int> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							this.currentGroup.Add(x);
							if (FeatureWorker.groupID[x] != 0 && FeatureWorker.groupSize[x] >= minRootGroupSize && FeatureWorker.groupSize[x] <= maxRootGroupSize)
							{
								this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x]);
							}
						});
					}
					worldFloodFiller3.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize && this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
					{
						if (!this.def.canTouchWorldEdge)
						{
							List<int> list = this.currentGroup;
							Predicate<int> predicate;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((int x) => worldGrid.IsOnEdge(x)));
							}
							if (list.Any(predicate))
							{
								goto IL_395;
							}
						}
						this.currentGroupMembers.Clear();
						for (int l = 0; l < this.currentGroup.Count; l++)
						{
							int num3 = this.currentGroup[l];
							bool flag;
							if (this.IsMember(num3, out flag) && (!flag || !this.rootsSet.Contains(num3) || (FeatureWorker.groupSize[num3] >= minRootGroupSize && FeatureWorker.groupSize[num3] <= maxRootGroupSize)))
							{
								this.currentGroupMembers.Add(this.currentGroup[l]);
							}
						}
						if (this.currentGroupMembers.Count >= minOverallSize)
						{
							List<int> list2 = this.currentGroup;
							Predicate<int> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((int x) => worldGrid[x].feature == null));
							}
							if (list2.Any(predicate2))
							{
								List<int> list3 = this.currentGroup;
								Predicate<int> match;
								if ((match = <>9__6) == null)
								{
									match = (<>9__6 = ((int x) => worldGrid[x].feature != null));
								}
								list3.RemoveAll(match);
							}
							base.AddFeature(this.currentGroupMembers, this.currentGroup);
						}
					}
				}
				IL_395:;
			}
		}

		// Token: 0x040056FC RID: 22268
		private List<int> roots = new List<int>();

		// Token: 0x040056FD RID: 22269
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x040056FE RID: 22270
		private List<int> rootsWithAreaInBetween = new List<int>();

		// Token: 0x040056FF RID: 22271
		private HashSet<int> rootsWithAreaInBetweenSet = new HashSet<int>();

		// Token: 0x04005700 RID: 22272
		private List<int> currentGroup = new List<int>();

		// Token: 0x04005701 RID: 22273
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04005702 RID: 22274
		private HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x04005703 RID: 22275
		private static List<int> tmpGroup = new List<int>();
	}
}
