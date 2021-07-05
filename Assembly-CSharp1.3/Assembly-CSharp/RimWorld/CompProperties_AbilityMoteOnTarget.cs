using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D46 RID: 3398
	public class CompProperties_AbilityMoteOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06004F5B RID: 20315 RVA: 0x001A9888 File Offset: 0x001A7A88
		public CompProperties_AbilityMoteOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_MoteOnTarget);
		}

		// Token: 0x04002FA2 RID: 12194
		public ThingDef moteDef;

		// Token: 0x04002FA3 RID: 12195
		public List<ThingDef> moteDefs;

		// Token: 0x04002FA4 RID: 12196
		public float scale = 1f;

		// Token: 0x04002FA5 RID: 12197
		public int preCastTicks;
	}
}
