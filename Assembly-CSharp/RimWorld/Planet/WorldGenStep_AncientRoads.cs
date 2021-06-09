using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002094 RID: 8340
	public class WorldGenStep_AncientRoads : WorldGenStep
	{
		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x0600B0D0 RID: 45264 RVA: 0x00072ED1 File Offset: 0x000710D1
		public override int SeedPart
		{
			get
			{
				return 773428712;
			}
		}

		// Token: 0x0600B0D1 RID: 45265 RVA: 0x00072ED8 File Offset: 0x000710D8
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientRoads();
		}

		// Token: 0x0600B0D2 RID: 45266 RVA: 0x00335680 File Offset: 0x00333880
		private void GenerateAncientRoads()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts(new int?(0));
			List<List<int>> list = this.GenerateProspectiveRoads();
			list.Sort((List<int> lhs, List<int> rhs) => -lhs.Count.CompareTo(rhs.Count));
			HashSet<int> used = new HashSet<int>();
			Predicate<int> <>9__1;
			for (int i = 0; i < list.Count; i++)
			{
				List<int> list2 = list[i];
				Predicate<int> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((int elem) => used.Contains(elem)));
				}
				if (!list2.Any(predicate))
				{
					if (list[i].Count < 4)
					{
						break;
					}
					foreach (int item in list[i])
					{
						used.Add(item);
					}
					for (int j = 0; j < list[i].Count - 1; j++)
					{
						float num = Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness;
						float costCutoff = num * 12000f;
						using (WorldPath worldPath = Find.WorldPathFinder.FindPath(list[i][j], list[i][j + 1], null, (float cost) => cost > costCutoff))
						{
							if (worldPath != null && worldPath != WorldPath.NotFound)
							{
								List<int> nodesReversed = worldPath.NodesReversed;
								if ((float)nodesReversed.Count <= Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness)
								{
									for (int k = 0; k < nodesReversed.Count - 1; k++)
									{
										if (Find.WorldGrid.GetRoadDef(nodesReversed[k], nodesReversed[k + 1], false) != null)
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltHighway);
										}
										else
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltRoad);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B0D3 RID: 45267 RVA: 0x0033591C File Offset: 0x00333B1C
		private List<List<int>> GenerateProspectiveRoads()
		{
			List<int> ancientSites = Find.World.genData.ancientSites;
			List<List<int>> list = new List<List<int>>();
			for (int i = 0; i < ancientSites.Count; i++)
			{
				for (int j = 0; j < ancientSites.Count; j++)
				{
					List<int> list2 = new List<int>();
					list2.Add(ancientSites[i]);
					List<int> list3 = ancientSites;
					float ang = Find.World.grid.GetHeadingFromTo(i, j);
					int current = ancientSites[i];
					Func<int, bool> <>9__0;
					Func<int, float> <>9__1;
					for (;;)
					{
						IEnumerable<int> source = list3;
						Func<int, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((int idx) => idx != current && Math.Abs(Find.World.grid.GetHeadingFromTo(current, idx) - ang) < this.maximumSiteCurve));
						}
						list3 = source.Where(predicate).ToList<int>();
						if (list3.Count == 0)
						{
							break;
						}
						IEnumerable<int> source2 = list3;
						Func<int, float> selector;
						if ((selector = <>9__1) == null)
						{
							selector = (<>9__1 = ((int idx) => Find.World.grid.ApproxDistanceInTiles(current, idx)));
						}
						int num = source2.MinBy(selector);
						ang = Find.World.grid.GetHeadingFromTo(current, num);
						current = num;
						list2.Add(current);
					}
					list.Add(list2);
				}
			}
			return list;
		}

		// Token: 0x040079C2 RID: 31170
		public float maximumSiteCurve;

		// Token: 0x040079C3 RID: 31171
		public float minimumChain;

		// Token: 0x040079C4 RID: 31172
		public float maximumSegmentCurviness;
	}
}
