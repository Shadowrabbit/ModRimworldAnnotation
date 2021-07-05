using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001789 RID: 6025
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x06008AF3 RID: 35571 RVA: 0x0031E060 File Offset: 0x0031C260
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x06008AF4 RID: 35572 RVA: 0x0031E067 File Offset: 0x0031C267
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x06008AF5 RID: 35573 RVA: 0x0031E070 File Offset: 0x0031C270
		private void GenerateLakes()
		{
			WorldGrid grid = Find.WorldGrid;
			bool[] touched = new bool[grid.TilesCount];
			List<int> oceanChunk = new List<int>();
			Predicate<int> <>9__0;
			Action<int> <>9__1;
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (!touched[i] && grid[i].biome == BiomeDefOf.Ocean)
				{
					WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
					int rootTile = i;
					Predicate<int> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((int tid) => grid[tid].biome == BiomeDefOf.Ocean));
					}
					Action<int> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(int tid)
						{
							oceanChunk.Add(tid);
							touched[tid] = true;
						});
					}
					worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, null);
					if (oceanChunk.Count <= 15)
					{
						for (int j = 0; j < oceanChunk.Count; j++)
						{
							grid[oceanChunk[j]].biome = BiomeDefOf.Lake;
						}
					}
					oceanChunk.Clear();
				}
			}
		}

		// Token: 0x0400587E RID: 22654
		private const int LakeMaxSize = 15;
	}
}
