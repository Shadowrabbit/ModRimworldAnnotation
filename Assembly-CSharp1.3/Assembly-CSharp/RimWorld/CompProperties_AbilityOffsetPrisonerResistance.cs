using System;

namespace RimWorld
{
	// Token: 0x02000D4C RID: 3404
	public class CompProperties_AbilityOffsetPrisonerResistance : CompProperties_AbilityEffect
	{
		// Token: 0x06004F70 RID: 20336 RVA: 0x001AA1C4 File Offset: 0x001A83C4
		public CompProperties_AbilityOffsetPrisonerResistance()
		{
			this.compClass = typeof(CompAbilityEffect_OffsetPrisonerResistance);
		}

		// Token: 0x04002FB0 RID: 12208
		public float offset;
	}
}
