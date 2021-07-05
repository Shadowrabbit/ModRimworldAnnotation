using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200178B RID: 6027
	public class WorldGenStep_Roads : WorldGenStep
	{
		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x06008B05 RID: 35589 RVA: 0x0031E727 File Offset: 0x0031C927
		public override int SeedPart
		{
			get
			{
				return 1538475135;
			}
		}

		// Token: 0x06008B06 RID: 35590 RVA: 0x0031E72E File Offset: 0x0031C92E
		public override void GenerateFresh(string seed)
		{
			this.GenerateRoadEndpoints();
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.PopState();
		}

		// Token: 0x06008B07 RID: 35591 RVA: 0x0031E751 File Offset: 0x0031C951
		public override void GenerateWithoutWorldData(string seed)
		{
			Rand.PushState();
			Rand.Seed = GenText.StableStringHash(seed);
			this.GenerateRoadNetwork();
			Rand.PopState();
		}

		// Token: 0x06008B08 RID: 35592 RVA: 0x0031E770 File Offset: 0x0031C970
		private void GenerateRoadEndpoints()
		{
			List<int> list = (from wo in Find.WorldObjects.AllWorldObjects
			where Rand.Value > 0.05f
			select wo.Tile).ToList<int>();
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * WorldGenStep_Roads.ExtraRoadNodesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				list.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
			List<int> list2 = new List<int>();
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = Mathf.Max(0, WorldGenStep_Roads.RoadDistanceFromSettlement.RandomInRange);
				int num3 = list[j];
				for (int k = 0; k < num2; k++)
				{
					Find.WorldGrid.GetTileNeighbors(num3, list2);
					num3 = list2.RandomElement<int>();
				}
				if (Find.WorldReachability.CanReach(list[j], num3))
				{
					list[j] = num3;
				}
			}
			list = list.Distinct<int>().ToList<int>();
			Find.World.genData.roadNodes = list;
		}

		// Token: 0x06008B09 RID: 35593 RVA: 0x0031E8B8 File Offset: 0x0031CAB8
		private void GenerateRoadNetwork()
		{
			Find.WorldPathGrid.RecalculateAllPerceivedPathCosts(new int?(0));
			List<WorldGenStep_Roads.Link> linkProspective = this.GenerateProspectiveLinks(Find.World.genData.roadNodes);
			List<WorldGenStep_Roads.Link> linkFinal = this.GenerateFinalLinks(linkProspective, Find.World.genData.roadNodes.Count);
			this.DrawLinksOnWorld(linkFinal, Find.World.genData.roadNodes);
		}

		// Token: 0x06008B0A RID: 35594 RVA: 0x0031E920 File Offset: 0x0031CB20
		private List<WorldGenStep_Roads.Link> GenerateProspectiveLinks(List<int> indexToTile)
		{
			WorldGenStep_Roads.<>c__DisplayClass14_0 CS$<>8__locals1 = new WorldGenStep_Roads.<>c__DisplayClass14_0();
			CS$<>8__locals1.tileToIndexLookup = new Dictionary<int, int>();
			for (int i = 0; i < indexToTile.Count; i++)
			{
				CS$<>8__locals1.tileToIndexLookup[indexToTile[i]] = i;
			}
			CS$<>8__locals1.linkProspective = new List<WorldGenStep_Roads.Link>();
			List<int> list = new List<int>();
			int srcIndex;
			int srcIndex2;
			for (srcIndex = 0; srcIndex < indexToTile.Count; srcIndex = srcIndex2)
			{
				int srcTile = indexToTile[srcIndex];
				list.Clear();
				list.Add(srcTile);
				int found = 0;
				Find.WorldPathFinder.FloodPathsWithCost(list, (int src, int dst) => Caravan_PathFollower.CostToMove(3300, src, dst, null, true, null, null, false), null, delegate(int tile, float distance)
				{
					int found;
					if (tile != srcTile && CS$<>8__locals1.tileToIndexLookup.ContainsKey(tile))
					{
						found++;
						found = found;
						CS$<>8__locals1.linkProspective.Add(new WorldGenStep_Roads.Link
						{
							distance = distance,
							indexA = srcIndex,
							indexB = CS$<>8__locals1.tileToIndexLookup[tile]
						});
					}
					return found >= 8;
				});
				srcIndex2 = srcIndex + 1;
			}
			CS$<>8__locals1.linkProspective.Sort((WorldGenStep_Roads.Link lhs, WorldGenStep_Roads.Link rhs) => lhs.distance.CompareTo(rhs.distance));
			return CS$<>8__locals1.linkProspective;
		}

		// Token: 0x06008B0B RID: 35595 RVA: 0x0031EA5C File Offset: 0x0031CC5C
		private List<WorldGenStep_Roads.Link> GenerateFinalLinks(List<WorldGenStep_Roads.Link> linkProspective, int endpointCount)
		{
			List<WorldGenStep_Roads.Connectedness> list = new List<WorldGenStep_Roads.Connectedness>();
			for (int i = 0; i < endpointCount; i++)
			{
				list.Add(new WorldGenStep_Roads.Connectedness());
			}
			List<WorldGenStep_Roads.Link> list2 = new List<WorldGenStep_Roads.Link>();
			for (int j = 0; j < linkProspective.Count; j++)
			{
				WorldGenStep_Roads.Link prospective = linkProspective[j];
				if (list[prospective.indexA].Group() != list[prospective.indexB].Group() || (Rand.Value <= 0.015f && !list2.Any((WorldGenStep_Roads.Link link) => link.indexB == prospective.indexA && link.indexA == prospective.indexB)))
				{
					if (Rand.Value > 0.1f)
					{
						list2.Add(prospective);
					}
					if (list[prospective.indexA].Group() != list[prospective.indexB].Group())
					{
						WorldGenStep_Roads.Connectedness parent = new WorldGenStep_Roads.Connectedness();
						list[prospective.indexA].Group().parent = parent;
						list[prospective.indexB].Group().parent = parent;
					}
				}
			}
			return list2;
		}

		// Token: 0x06008B0C RID: 35596 RVA: 0x0031EB9C File Offset: 0x0031CD9C
		private void DrawLinksOnWorld(List<WorldGenStep_Roads.Link> linkFinal, List<int> indexToTile)
		{
			foreach (WorldGenStep_Roads.Link link in linkFinal)
			{
				WorldPath worldPath = Find.WorldPathFinder.FindPath(indexToTile[link.indexA], indexToTile[link.indexB], null, null);
				List<int> nodesReversed = worldPath.NodesReversed;
				RoadDef roadDef = (from rd in DefDatabase<RoadDef>.AllDefsListForReading
				where !rd.ancientOnly
				select rd).RandomElementWithFallback(null);
				for (int i = 0; i < nodesReversed.Count - 1; i++)
				{
					Find.WorldGrid.OverlayRoad(nodesReversed[i], nodesReversed[i + 1], roadDef);
				}
				worldPath.ReleaseToPool();
			}
		}

		// Token: 0x04005886 RID: 22662
		private static readonly FloatRange ExtraRoadNodesPer100kTiles = new FloatRange(30f, 50f);

		// Token: 0x04005887 RID: 22663
		private static readonly IntRange RoadDistanceFromSettlement = new IntRange(-4, 4);

		// Token: 0x04005888 RID: 22664
		private const float ChanceExtraNonSpanningTreeLink = 0.015f;

		// Token: 0x04005889 RID: 22665
		private const float ChanceHideSpanningTreeLink = 0.1f;

		// Token: 0x0400588A RID: 22666
		private const float ChanceWorldObjectReclusive = 0.05f;

		// Token: 0x0400588B RID: 22667
		private const int PotentialSpanningTreeLinksPerSettlement = 8;

		// Token: 0x020029D0 RID: 10704
		private struct Link
		{
			// Token: 0x04009D32 RID: 40242
			public float distance;

			// Token: 0x04009D33 RID: 40243
			public int indexA;

			// Token: 0x04009D34 RID: 40244
			public int indexB;
		}

		// Token: 0x020029D1 RID: 10705
		private class Connectedness
		{
			// Token: 0x0600E307 RID: 58119 RVA: 0x00427DD6 File Offset: 0x00425FD6
			public WorldGenStep_Roads.Connectedness Group()
			{
				if (this.parent == null)
				{
					return this;
				}
				return this.parent.Group();
			}

			// Token: 0x04009D35 RID: 40245
			public WorldGenStep_Roads.Connectedness parent;
		}
	}
}
