using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DD RID: 6365
	public class CompProperties_UseEffectPlaySound : CompProperties_Usable
	{
		// Token: 0x06008D00 RID: 36096 RVA: 0x0005E82C File Offset: 0x0005CA2C
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		// Token: 0x04005A0A RID: 23050
		public SoundDef soundOnUsed;
	}
}
