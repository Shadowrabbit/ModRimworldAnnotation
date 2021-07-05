using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120E RID: 4622
	public class CompProperties_UseEffectPlaySound : CompProperties_Usable
	{
		// Token: 0x06006EFF RID: 28415 RVA: 0x00251ADD File Offset: 0x0024FCDD
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		// Token: 0x04003D63 RID: 15715
		public SoundDef soundOnUsed;
	}
}
