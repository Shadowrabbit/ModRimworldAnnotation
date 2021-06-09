using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001370 RID: 4976
	public class CompProperties_AbilityFarskip : CompProperties_AbilityEffect
	{
		// Token: 0x06006C38 RID: 27704 RVA: 0x00049A0D File Offset: 0x00047C0D
		public CompProperties_AbilityFarskip()
		{
			this.compClass = typeof(CompAbilityEffect_Farskip);
		}

		// Token: 0x040047D6 RID: 18390
		public IntRange stunTicks;
	}
}
