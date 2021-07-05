using System;

namespace Verse
{
	// Token: 0x020003BE RID: 958
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x060017DF RID: 6111 RVA: 0x00016B97 File Offset: 0x00014D97
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x04001228 RID: 4648
		public int intervalTicks;

		// Token: 0x04001229 RID: 4649
		public float chance;
	}
}
