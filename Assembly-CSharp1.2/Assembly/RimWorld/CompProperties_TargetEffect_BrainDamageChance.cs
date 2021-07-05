using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C4 RID: 6340
	public class CompProperties_TargetEffect_BrainDamageChance : CompProperties
	{
		// Token: 0x06008CAD RID: 36013 RVA: 0x0005E48B File Offset: 0x0005C68B
		public CompProperties_TargetEffect_BrainDamageChance()
		{
			this.compClass = typeof(CompTargetEffect_BrainDamageChance);
		}

		// Token: 0x040059FA RID: 23034
		public float brainDamageChance = 0.3f;
	}
}
