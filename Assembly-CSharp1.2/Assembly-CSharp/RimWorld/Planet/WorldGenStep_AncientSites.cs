using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002099 RID: 8345
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x0600B0DF RID: 45279 RVA: 0x00072F61 File Offset: 0x00071161
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x0600B0E0 RID: 45280 RVA: 0x00072F68 File Offset: 0x00071168
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x0600B0E1 RID: 45281 RVA: 0x00335A8C File Offset: 0x00333C8C
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
		}

		// Token: 0x040079CF RID: 31183
		public FloatRange ancientSitesPer100kTiles;
	}
}
