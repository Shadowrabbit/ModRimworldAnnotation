using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114C RID: 4428
	public class CompProperties_Maintainable : CompProperties
	{
		// Token: 0x06006A73 RID: 27251 RVA: 0x0023CECF File Offset: 0x0023B0CF
		public CompProperties_Maintainable()
		{
			this.compClass = typeof(CompMaintainable);
		}

		// Token: 0x04003B51 RID: 15185
		public int ticksHealthy = 1000;

		// Token: 0x04003B52 RID: 15186
		public int ticksNeedsMaintenance = 1000;

		// Token: 0x04003B53 RID: 15187
		public int damagePerTickRare = 10;
	}
}
