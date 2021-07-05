using System;

namespace Verse
{
	// Token: 0x02000287 RID: 647
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x0600123E RID: 4670 RVA: 0x00069992 File Offset: 0x00067B92
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x04000DDA RID: 3546
		public IntRange disappearsAfterTicks;

		// Token: 0x04000DDB RID: 3547
		public bool showRemainingTime;

		// Token: 0x04000DDC RID: 3548
		public MentalStateDef requiredMentalState;
	}
}
