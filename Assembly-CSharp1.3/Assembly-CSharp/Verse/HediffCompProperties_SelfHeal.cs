using System;

namespace Verse
{
	// Token: 0x020002B3 RID: 691
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x060012C8 RID: 4808 RVA: 0x0006B9C0 File Offset: 0x00069BC0
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04000E2A RID: 3626
		public int healIntervalTicksStanding = 50;

		// Token: 0x04000E2B RID: 3627
		public float healAmount = 1f;
	}
}
