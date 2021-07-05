using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E2 RID: 4578
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x06006E6F RID: 28271 RVA: 0x002502CB File Offset: 0x0024E4CB
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x04003D3F RID: 15679
		public float brainDamageChance = 0.3f;
	}
}
