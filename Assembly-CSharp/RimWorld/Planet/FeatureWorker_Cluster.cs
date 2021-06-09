using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002018 RID: 8216
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x0600AE02 RID: 44546 RVA: 0x00071416 File Offset: 0x0006F616
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x0600AE03 RID: 44547 RVA: 0x00071423 File Offset: 0x0006F623
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x0600AE04 RID: 44548 RVA: 0x00071430 File Offset: 0x0006F630
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x0600AE05 RID: 44549 RVA: 0x0007143D File Offset: 0x0006F63D
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x0600AE06 RID: 44550 RVA: 0x0007144A File Offset: 0x0006F64A
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x0600AE07 RID: 44551 RVA: 0x00071457 File Offset: 0x0006F657
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x0600AE08 RID: 44552
		protected abstract bool IsRoot(int tile);

		// Token: 0x0600AE09 RID: 44553 RVA: 0x00071464 File Offset: 0x0006F664
		protected virtual bool CanTraverse(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x0600AE0A RID: 44554 RVA: 0x0007146A File Offset: 0x0006F66A
		protected virtual bool IsMember(int tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600AE0B RID: 44555 RVA: 0x00071482 File Offset: 0x0006F682
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootTiles();
			this.CalculateRootsWithAreaInBetween();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600AE0C RID: 44556 RVA: 0x0032A7AC File Offset: 0x003289AC
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

		// Token: 0x0600AE0D RID: 44557 RVA: 0x0032A80C File Offset: 0x00328A0C
		private void CalculateRootsWithAreaInBetween()
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x0600AE0E RID: 44558 RVA: 0x0032A864 File Offset: 0x00328A64
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

		// Token: 0x04007775 RID: 30581
		private List<int> roots = new List<int>();

		// Token: 0x04007776 RID: 30582
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04007777 RID: 30583
		private List<int> rootsWithAreaInBetween = new List<int>();

		// Token: 0x04007778 RID: 30584
		private HashSet<int> rootsWithAreaInBetweenSet = new HashSet<int>();

		// Token: 0x04007779 RID: 30585
		private List<int> currentGroup = new List<int>();

		// Token: 0x0400777A RID: 30586
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x0400777B RID: 30587
		private HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x0400777C RID: 30588
		private static List<int> tmpGroup = new List<int>();
	}
}
