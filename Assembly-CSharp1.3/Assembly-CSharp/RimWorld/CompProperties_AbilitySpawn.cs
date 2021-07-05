using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D62 RID: 3426
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		// Token: 0x06004FA7 RID: 20391 RVA: 0x001AAA34 File Offset: 0x001A8C34
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		// Token: 0x04002FC1 RID: 12225
		public ThingDef thingDef;

		// Token: 0x04002FC2 RID: 12226
		public bool allowOnBuildings = true;
	}
}
