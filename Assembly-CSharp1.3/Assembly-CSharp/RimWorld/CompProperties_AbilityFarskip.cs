using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D31 RID: 3377
	public class CompProperties_AbilityFarskip : CompProperties_AbilityEffect
	{
		// Token: 0x06004F1A RID: 20250 RVA: 0x001A83AC File Offset: 0x001A65AC
		public CompProperties_AbilityFarskip()
		{
			this.compClass = typeof(CompAbilityEffect_Farskip);
		}

		// Token: 0x04002F8B RID: 12171
		public IntRange stunTicks;
	}
}
