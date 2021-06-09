using System;

namespace Verse
{
	// Token: 0x020003EE RID: 1006
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x0600188D RID: 6285 RVA: 0x00017488 File Offset: 0x00015688
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04001291 RID: 4753
		public int healIntervalTicksStanding = 50;

		// Token: 0x04001292 RID: 4754
		public float healAmount = 1f;
	}
}
