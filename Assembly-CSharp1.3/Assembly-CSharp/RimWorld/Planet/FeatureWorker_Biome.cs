using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001738 RID: 5944
	public class FeatureWorker_Biome : FeatureWorker_FloodFill
	{
		// Token: 0x06008920 RID: 35104 RVA: 0x00314A46 File Offset: 0x00312C46
		protected override bool IsRoot(int tile)
		{
			return this.def.rootBiomes.Contains(Find.WorldGrid[tile].biome);
		}

		// Token: 0x06008921 RID: 35105 RVA: 0x00314A68 File Offset: 0x00312C68
		protected override bool IsPossiblyAllowed(int tile)
		{
			return this.def.acceptableBiomes.Contains(Find.WorldGrid[tile].biome);
		}
	}
}
