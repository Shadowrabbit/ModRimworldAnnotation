using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200201F RID: 8223
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x0600AE28 RID: 44584 RVA: 0x0007143D File Offset: 0x0006F63D
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600AE29 RID: 44585 RVA: 0x0007144A File Offset: 0x0006F64A
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x0600AE2A RID: 44586 RVA: 0x000715C8 File Offset: 0x0006F7C8
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x0600AE2B RID: 44587 RVA: 0x000715D5 File Offset: 0x0006F7D5
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x0600AE2C RID: 44588
		protected abstract bool IsRoot(int tile);

		// Token: 0x0600AE2D RID: 44589 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected virtual bool IsPossiblyAllowed(int tile)
		{
			return false;
		}

		// Token: 0x0600AE2E RID: 44590 RVA: 0x000715E2 File Offset: 0x0006F7E2
		protected virtual bool IsMember(int tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600AE2F RID: 44591 RVA: 0x000715F7 File Offset: 0x0006F7F7
		public override void GenerateWhereAppropriate()
		{
			this.CalculateRootsAndPossiblyAllowedTiles();
			this.CalculateContiguousGroups();
		}

		// Token: 0x0600AE30 RID: 44592 RVA: 0x0032AF00 File Offset: 0x00329100
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

		// Token: 0x0600AE31 RID: 44593 RVA: 0x0032AF9C File Offset: 0x0032919C
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

		// Token: 0x0400778D RID: 30605
		private List<int> roots = new List<int>();

		// Token: 0x0400778E RID: 30606
		private HashSet<int> rootsSet = new HashSet<int>();

		// Token: 0x0400778F RID: 30607
		private List<int> possiblyAllowed = new List<int>();

		// Token: 0x04007790 RID: 30608
		private HashSet<int> possiblyAllowedSet = new HashSet<int>();

		// Token: 0x04007791 RID: 30609
		private List<int> currentGroup = new List<int>();

		// Token: 0x04007792 RID: 30610
		private List<int> currentGroupMembers = new List<int>();

		// Token: 0x04007793 RID: 30611
		private static List<int> tmpGroup = new List<int>();
	}
}
