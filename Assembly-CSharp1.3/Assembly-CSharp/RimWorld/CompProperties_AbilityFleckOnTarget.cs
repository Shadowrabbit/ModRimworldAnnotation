using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D37 RID: 3383
	public class CompProperties_AbilityFleckOnTarget : CompProperties_AbilityEffect
	{
		// Token: 0x06004F33 RID: 20275 RVA: 0x001A8FAF File Offset: 0x001A71AF
		public CompProperties_AbilityFleckOnTarget()
		{
			this.compClass = typeof(CompAbilityEffect_FleckOnTarget);
		}

		// Token: 0x04002F8D RID: 12173
		public FleckDef fleckDef;

		// Token: 0x04002F8E RID: 12174
		public List<FleckDef> fleckDefs;

		// Token: 0x04002F8F RID: 12175
		public float scale = 1f;

		// Token: 0x04002F90 RID: 12176
		public int preCastTicks;
	}
}
