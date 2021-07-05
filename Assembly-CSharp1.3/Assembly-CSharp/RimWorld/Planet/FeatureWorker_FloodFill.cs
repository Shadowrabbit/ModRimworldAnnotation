using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001739 RID: 5945
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06008923 RID: 35107 RVA: 0x0031434C File Offset: 0x0031254C
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06008924 RID: 35108 RVA: 0x00314359 File Offset: 0x00312559
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x06008925 RID: 35109 RVA: 0x00314A92 File Offset: 0x00312C92
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06008926 RID: 35110 RVA: 0x00314A9F File Offset: 0x00312C9F
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x06008927 RID: 35111
		protected abstract bool IsRoot(int tile);

		// Token: 0x06008928 RID: 35112 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		// Token: 0x06008929 RID: 35113 RVA: 0x00314AAC File Offset: 0x00312CAC
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600892A RID: 35114 RVA: 0x00314AC1 File Offset: 0x00312CC1
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600892B RID: 35115 RVA: 0x00314AD0 File Offset: 0x00312CD0
		private void CalculateRootsAndPossiblyAllowedTiles()
		{
			this.roots.Clear();
			this.possiblyAllowed.Clear();
			int tilesCount = Find.WorldGrid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.roots.Add(i);
				}
				if (this.IsPossiblyAllowed(i))
				{
					this.possiblyAllowed.Add(i);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
			this.possiblyAllowedSet.Clear();
			this.possiblyAllowedSet.AddRange(this.possiblyAllowed);
		}

		// Token: 0x0600892C RID: 35116 RVA: 0x00314B6C File Offset: 0x00312D6C
		private void CalculateContiguousGroups()
		{
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			int maxPossiblyAllowedSizeToTake = this.MaxPossiblyAllowedSizeToTake;
			float maxPossiblyAllowedSizePctOfMeToTake = this.MaxPossiblyAllowedSizePctOfMeToTake;
			FeatureWorker.ClearVisited();
			FeatureWorker.ClearGroupSizes();
			Predicate<int> <>9__0;
			for (int i = 0; i < this.possiblyAllowed.Count; i++)
			{
				int num = this.possiblyAllowed[i];
				if (!FeatureWorker.visited[num] && !this.rootsSet.Contains(num))
				{
					FeatureWorker_FloodFill.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller2 = worldFloodFiller;
					int rootTile = num;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int x) => this.possiblyAllowedSet.Contains(x) && !this.rootsSet.Contains(x)));
					}
					worldFloodFiller2.FloodFill(rootTile, passCheck, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						FeatureWorker_FloodFill.tmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_FloodFill.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_FloodFill.tmpGroup[j]] = FeatureWorker_FloodFill.tmpGroup.Count;
					}
				}
			}
			Predicate<int> <>9__2;
			Predicate<int> <>9__4;
			Predicate<int> <>9__8;
			Predicate<int> <>9__9;
			Predicate<int> <>9__10;
			for (int k = 0; k < this.roots.Count; k++)
			{
				int num2 = this.roots[k];
				if (!FeatureWorker.visited[num2])
				{
					int initialMembersCountClamped = 0;
					WorldFloodFiller worldFloodFiller3 = worldFloodFiller;
					int rootTile2 = num2;
					Predicate<int> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((int x) => (this.rootsSet.Contains(x) || this.possiblyAllowedSet.Contains(x)) && this.IsMember(x)));
					}
					worldFloodFiller3.FloodFill(rootTile2, passCheck2, delegate(int x)
					{
						FeatureWorker.visited[x] = true;
						int initialMembersCountClamped = initialMembersCountClamped;
						initialMembersCountClamped++;
						return initialMembersCountClamped >= minSize;
					}, int.MaxValue, null);
					if (initialMembersCountClamped >= minSize)
					{
						int initialRootsCount = 0;
						WorldFloodFiller worldFloodFiller4 = worldFloodFiller;
						int rootTile3 = num2;
						Predicate<int> passCheck3;
						if ((passCheck3 = <>9__4) == null)
						{
							passCheck3 = (<>9__4 = ((int x) => this.rootsSet.Contains(x)));
						}
						worldFloodFiller4.FloodFill(rootTile3, passCheck3, delegate(int x)
						{
							FeatureWorker.visited[x] = true;
							int initialRootsCount = initialRootsCount;
							initialRootsCount++;
						}, int.MaxValue, null);
						if (initialRootsCount >= minSize && initialRootsCount <= maxSize)
						{
							int traversedRootsCount = 0;
							this.currentGroup.Clear();
							worldFloodFiller.FloodFill(num2, (int x) => this.rootsSet.Contains(x) || (this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x] <= maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(traversedRootsCount, initialRootsCount) && FeatureWorker.groupSize[x] < maxSize), delegate(int x)
							{
								FeatureWorker.visited[x] = true;
								if (this.rootsSet.Contains(x))
								{
									int traversedRootsCount = traversedRootsCount;
									traversedRootsCount++;
								}
								this.currentGroup.Add(x);
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minSize && this.currentGroup.Count <= maxSize)
							{
								if (!this.def.canTouchWorldEdge)
								{
									List<int> list = this.currentGroup;
									Predicate<int> predicate;
									if ((predicate = <>9__8) == null)
									{
										predicate = (<>9__8 = ((int x) => worldGrid.IsOnEdge(x)));
									}
									if (list.Any(predicate))
									{
										goto IL_41F;
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
									if ((predicate2 = <>9__9) == null)
									{
										predicate2 = (<>9__9 = ((int x) => worldGrid[x].feature == null));
									}
									if (list2.Any(predicate2))
									{
										List<int> list3 = this.currentGroup;
										Predicate<int> match;
										if ((match = <>9__10) == null)
										{
											match = (<>9__10 = ((int x) => worldGrid[x].feature != null));
										}
										list3.RemoveAll(match);
									}
									base.AddFeature(this.currentGroupMembers, this.currentGroup);
								}
							}
						}
					}
				}
				IL_41F:;
			}
		}

		// Token: 0x04005706 RID: 22278
		private List<int> roots = new List<int>();

		// Token: 0x04005707 RID: 22279
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x04005708 RID: 22280
		private List<int> possiblyAllowed = new List<int>();

		// Token: 0x04005709 RID: 22281
		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		// Token: 0x0400570A RID: 22282
		private List<int> currentGroup = new List<int>();

		// Token: 0x0400570B RID: 22283
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x0400570C RID: 22284
		private static List<int> tmpGroup = new List<int>();
	}
}
