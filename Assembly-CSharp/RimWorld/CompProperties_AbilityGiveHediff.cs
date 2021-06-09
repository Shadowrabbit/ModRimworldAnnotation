using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137A RID: 4986
	public class CompProperties_AbilityGiveHediff : CompProperties_AbilityEffectWithDuration
	{
		// Token: 0x040047EA RID: 18410
		public HediffDef hediffDef;

		// Token: 0x040047EB RID: 18411
		public bool onlyBrain;

		// Token: 0x040047EC RID: 18412
		public bool applyToSelf;

		// Token: 0x040047ED RID: 18413
		public bool onlyApplyToSelf;

		// Token: 0x040047EE RID: 18414
		public bool replaceExisting;
	}
}
