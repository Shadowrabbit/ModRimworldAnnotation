using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001737 RID: 5943
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x0600891C RID: 35100 RVA: 0x003148C0 File Offset: 0x00312AC0
		public override void GenerateWhereAppropriate()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			this.edgeTiles.Clear();
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.edgeTiles.Add(i);
				}
			}
			if (!this.edgeTiles.Any<int>())
			{
				return;
			}
			this.group.Clear();
			Find.WorldFloodFiller.FloodFill(-1, (int x) => this.CanTraverse(x), delegate(int tile, int traversalDist)
			{
				this.group.Add(tile);
				return false;
			}, int.MaxValue, this.edgeTiles);
			this.group.RemoveAll((int x) => worldGrid[x].feature != null);
			if (this.group.Count < this.def.minSize || this.group.Count > this.def.maxSize)
			{
				return;
			}
			base.AddFeature(this.group, this.group);
		}

		// Token: 0x0600891D RID: 35101 RVA: 0x003149C0 File Offset: 0x00312BC0
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		// Token: 0x0600891E RID: 35102 RVA: 0x003149F8 File Offset: 0x00312BF8
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}

		// Token: 0x04005704 RID: 22276
		private List<int> group = new List<int>();

		// Token: 0x04005705 RID: 22277
		private List<int> edgeTiles = new List<int>();
	}
}
