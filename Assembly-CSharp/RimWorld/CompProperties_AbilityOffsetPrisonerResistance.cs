using System;

namespace RimWorld
{
	// Token: 0x02001385 RID: 4997
	public class CompProperties_AbilityOffsetPrisonerResistance : CompProperties_AbilityEffect
	{
		// Token: 0x06006C8E RID: 27790 RVA: 0x00049D64 File Offset: 0x00047F64
		public CompProperties_AbilityOffsetPrisonerResistance()
		{
			this.compClass = typeof(CompAbilityEffect_OffsetPrisonerResistance);
		}

		// Token: 0x040047FF RID: 18431
		public float offset;
	}
}
