using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D48 RID: 3400
	public class CompProperties_AbilityMustBeCapableOf : CompProperties_AbilityEffect
	{
		// Token: 0x06004F63 RID: 20323 RVA: 0x001A9A2E File Offset: 0x001A7C2E
		public CompProperties_AbilityMustBeCapableOf()
		{
			this.compClass = typeof(CompAbilityEffect_MustBeCapableOf);
		}

		// Token: 0x04002FA6 RID: 12198
		public WorkTags workTags;
	}
}
