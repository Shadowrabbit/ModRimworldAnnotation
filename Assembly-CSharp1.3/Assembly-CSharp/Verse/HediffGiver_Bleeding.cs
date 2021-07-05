using System;

namespace Verse
{
	// Token: 0x020002D4 RID: 724
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x0006F09C File Offset: 0x0006D29C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.1f)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
				return;
			}
			HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
		}
	}
}
