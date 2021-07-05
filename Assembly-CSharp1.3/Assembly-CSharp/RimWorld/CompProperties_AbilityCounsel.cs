using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D29 RID: 3369
	public class CompProperties_AbilityCounsel : CompProperties_AbilityEffect
	{
		// Token: 0x06004F03 RID: 20227 RVA: 0x001A7A10 File Offset: 0x001A5C10
		public CompProperties_AbilityCounsel()
		{
			this.compClass = typeof(CompAbilityEffect_Counsel);
		}

		// Token: 0x04002F7D RID: 12157
		public float minMoodOffset = -10f;

		// Token: 0x04002F7E RID: 12158
		[MustTranslate]
		public string successMessage;

		// Token: 0x04002F7F RID: 12159
		[MustTranslate]
		public string successMessageNoNegativeThought;

		// Token: 0x04002F80 RID: 12160
		[MustTranslate]
		public string failMessage;

		// Token: 0x04002F81 RID: 12161
		public ThoughtDef failedThoughtRecipient;
	}
}
