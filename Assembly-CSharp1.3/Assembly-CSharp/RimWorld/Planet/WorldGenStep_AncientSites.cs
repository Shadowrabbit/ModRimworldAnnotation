using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001785 RID: 6021
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x06008AE3 RID: 35555 RVA: 0x0031DEBE File Offset: 0x0031C0BE
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x06008AE4 RID: 35556 RVA: 0x0031DEC5 File Offset: 0x0031C0C5
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x06008AE5 RID: 35557 RVA: 0x0031DED0 File Offset: 0x0031C0D0
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomSettlementTileFor(null, false, null));
			}
		}

		// Token: 0x0400587D RID: 22653
		public FloatRange ancientSitesPer100kTiles;
	}
}
