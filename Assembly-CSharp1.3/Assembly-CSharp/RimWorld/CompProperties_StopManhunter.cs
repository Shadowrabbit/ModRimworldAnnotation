using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6E RID: 3438
	public class CompProperties_StopManhunter : CompProperties_AbilityEffect
	{
		// Token: 0x06004FC5 RID: 20421 RVA: 0x001AB0E0 File Offset: 0x001A92E0
		public CompProperties_StopManhunter()
		{
			this.compClass = typeof(CompAbilityEffect_StopManhunter);
		}

		// Token: 0x04002FC8 RID: 12232
		[MustTranslate]
		public string successMessage;
	}
}
