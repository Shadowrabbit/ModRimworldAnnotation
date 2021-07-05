using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200209E RID: 8350
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600B0F3 RID: 45299 RVA: 0x00072FCB File Offset: 0x000711CB
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x0600B0F4 RID: 45300 RVA: 0x00072FD2 File Offset: 0x000711D2
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x0600B0F5 RID: 45301 RVA: 0x00335BC8 File Offset: 0x00333DC8
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

		// Token: 0x040079D3 RID: 31187
		private const int LakeMaxSize = 15;
	}
}
