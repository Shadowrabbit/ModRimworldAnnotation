using System;

namespace Verse
{
	// Token: 0x0200027C RID: 636
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x00069157 File Offset: 0x00067357
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x04000DC5 RID: 3525
		public MentalStateDef animalMentalState;

		// Token: 0x04000DC6 RID: 3526
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x04000DC7 RID: 3527
		public MentalStateDef humanMentalState;

		// Token: 0x04000DC8 RID: 3528
		public LetterDef letterDef;

		// Token: 0x04000DC9 RID: 3529
		public float mtbDaysToCauseMentalState;

		// Token: 0x04000DCA RID: 3530
		public bool endMentalStateOnCure = true;
	}
}
