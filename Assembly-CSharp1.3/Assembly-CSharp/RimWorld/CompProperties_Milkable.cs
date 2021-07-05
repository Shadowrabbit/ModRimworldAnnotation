using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A06 RID: 2566
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x06003EF2 RID: 16114 RVA: 0x00157D01 File Offset: 0x00155F01
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x040021E3 RID: 8675
		public int milkIntervalDays;

		// Token: 0x040021E4 RID: 8676
		public int milkAmount = 1;

		// Token: 0x040021E5 RID: 8677
		public ThingDef milkDef;

		// Token: 0x040021E6 RID: 8678
		public bool milkFemaleOnly = true;
	}
}
