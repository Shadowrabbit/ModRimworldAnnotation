using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001380 RID: 4992
	public class CompProperties_AbilityMoteOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06006C77 RID: 27767 RVA: 0x00049C1A File Offset: 0x00047E1A
		public CompProperties_AbilityMoteOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_MoteOnTarget);
		}

		// Token: 0x040047F2 RID: 18418
		public ThingDef moteDef;

		// Token: 0x040047F3 RID: 18419
		public List<ThingDef> moteDefs;

		// Token: 0x040047F4 RID: 18420
		public float scale = 1f;

		// Token: 0x040047F5 RID: 18421
		public int preCastTicks;
	}
}
