using System;

namespace RimWorld
{
	// Token: 0x02000D6C RID: 3436
	public class CompProperties_AbilityStartTrial : CompProperties_AbilityStartRitualOnPawn
	{
		// Token: 0x06004FC0 RID: 20416 RVA: 0x001AAFD6 File Offset: 0x001A91D6
		public CompProperties_AbilityStartTrial()
		{
			this.compClass = typeof(CompAbilityEffect_StartTrial);
		}

		// Token: 0x04002FC6 RID: 12230
		public PreceptDef ritualDefForPrisoner;

		// Token: 0x04002FC7 RID: 12231
		public PreceptDef ritualDefForMentalState;
	}
}
