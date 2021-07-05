using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FD RID: 2557
	public class CompProperties_EggLayer : CompProperties
	{
		// Token: 0x06003EE5 RID: 16101 RVA: 0x00157A4C File Offset: 0x00155C4C
		public CompProperties_EggLayer()
		{
			this.compClass = typeof(CompEggLayer);
		}

		// Token: 0x040021A9 RID: 8617
		public float eggLayIntervalDays = 1f;

		// Token: 0x040021AA RID: 8618
		public IntRange eggCountRange = IntRange.one;

		// Token: 0x040021AB RID: 8619
		public ThingDef eggUnfertilizedDef;

		// Token: 0x040021AC RID: 8620
		public ThingDef eggFertilizedDef;

		// Token: 0x040021AD RID: 8621
		public int eggFertilizationCountMax = 1;

		// Token: 0x040021AE RID: 8622
		public bool eggLayFemaleOnly = true;

		// Token: 0x040021AF RID: 8623
		public float eggProgressUnfertilizedMax = 1f;
	}
}
