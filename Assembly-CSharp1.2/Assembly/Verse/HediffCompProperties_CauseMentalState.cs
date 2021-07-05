using System;

namespace Verse
{
	// Token: 0x020003BC RID: 956
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		// Token: 0x060017D9 RID: 6105 RVA: 0x00016B63 File Offset: 0x00014D63
		public HediffCompProperties_CauseMentalState()
		{
			this.compClass = typeof(HediffComp_CauseMentalState);
		}

		// Token: 0x04001222 RID: 4642
		public MentalStateDef animalMentalState;

		// Token: 0x04001223 RID: 4643
		public MentalStateDef animalMentalStateAlias;

		// Token: 0x04001224 RID: 4644
		public MentalStateDef humanMentalState;

		// Token: 0x04001225 RID: 4645
		public LetterDef letterDef;

		// Token: 0x04001226 RID: 4646
		public float mtbDaysToCauseMentalState;

		// Token: 0x04001227 RID: 4647
		public bool endMentalStateOnCure = true;
	}
}
