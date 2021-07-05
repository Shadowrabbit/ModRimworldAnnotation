using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3B RID: 3387
	public class CompProperties_AbilityGiveHediff : CompProperties_AbilityEffectWithDuration
	{
		// Token: 0x04002F93 RID: 12179
		public HediffDef hediffDef;

		// Token: 0x04002F94 RID: 12180
		public bool onlyBrain;

		// Token: 0x04002F95 RID: 12181
		public bool applyToSelf;

		// Token: 0x04002F96 RID: 12182
		public bool onlyApplyToSelf;

		// Token: 0x04002F97 RID: 12183
		public bool applyToTarget = true;

		// Token: 0x04002F98 RID: 12184
		public bool replaceExisting;
	}
}
