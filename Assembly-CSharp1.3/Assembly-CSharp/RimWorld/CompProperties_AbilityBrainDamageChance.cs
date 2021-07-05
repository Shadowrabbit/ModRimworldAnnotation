using System;

namespace RimWorld
{
	// Token: 0x02000D22 RID: 3362
	public class CompProperties_AbilityBrainDamageChance : CompProperties_AbilityEffect
	{
		// Token: 0x06004EE8 RID: 20200 RVA: 0x001A6D3B File Offset: 0x001A4F3B
		public CompProperties_AbilityBrainDamageChance()
		{
			this.compClass = typeof(CompAbilityEffect_BrainDamageChance);
		}

		// Token: 0x04002F72 RID: 12146
		public float brainDamageChance = 0.3f;
	}
}
