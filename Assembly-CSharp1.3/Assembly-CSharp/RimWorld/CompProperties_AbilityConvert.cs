using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D26 RID: 3366
	public class CompProperties_AbilityConvert : CompProperties_AbilityEffect
	{
		// Token: 0x06004EFA RID: 20218 RVA: 0x001A737D File Offset: 0x001A557D
		public CompProperties_AbilityConvert()
		{
			this.compClass = typeof(CompAbilityEffect_Convert);
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x001A73A0 File Offset: 0x001A55A0
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			if (this.convertPowerFactor < 0f)
			{
				yield return "convertPowerFactor not set";
			}
			yield break;
		}

		// Token: 0x04002F78 RID: 12152
		[MustTranslate]
		public string successMessage;

		// Token: 0x04002F79 RID: 12153
		[MustTranslate]
		public string failMessage;

		// Token: 0x04002F7A RID: 12154
		public ThoughtDef failedThoughtInitiator;

		// Token: 0x04002F7B RID: 12155
		public ThoughtDef failedThoughtRecipient;

		// Token: 0x04002F7C RID: 12156
		public float convertPowerFactor = -1f;
	}
}
