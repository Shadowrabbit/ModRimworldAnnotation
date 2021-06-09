using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200201C RID: 8220
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x0600AE1D RID: 44573 RVA: 0x0032AD98 File Offset: 0x00328F98
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

		// Token: 0x0600AE1E RID: 44574 RVA: 0x0032AE98 File Offset: 0x00329098
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		// Token: 0x0600AE1F RID: 44575 RVA: 0x0032AED0 File Offset: 0x003290D0
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}

		// Token: 0x04007789 RID: 30601
		private List<int> group = new List<int>();

		// Token: 0x0400778A RID: 30602
		private List<int> edgeTiles = new List<int>();
	}
}
