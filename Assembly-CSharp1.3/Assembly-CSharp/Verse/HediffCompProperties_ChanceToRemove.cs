using System;

namespace Verse
{
	// Token: 0x0200027E RID: 638
	public class HediffCompProperties_ChanceToRemove : HediffCompProperties
	{
		// Token: 0x06001224 RID: 4644 RVA: 0x000694F0 File Offset: 0x000676F0
		public HediffCompProperties_ChanceToRemove()
		{
			this.compClass = typeof(HediffComp_ChanceToRemove);
		}

		// Token: 0x04000DCB RID: 3531
		public int intervalTicks;

		// Token: 0x04000DCC RID: 3532
		public float chance;
	}
}
