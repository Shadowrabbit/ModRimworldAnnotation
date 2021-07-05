using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017DA RID: 6106
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x0600872B RID: 34603 RVA: 0x0005AC68 File Offset: 0x00058E68
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x040056DD RID: 22237
		public int ticksHealthy = 1000;

		// Token: 0x040056DE RID: 22238
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x040056DF RID: 22239
		public int damagePerTickRare = 10;
	}
}
