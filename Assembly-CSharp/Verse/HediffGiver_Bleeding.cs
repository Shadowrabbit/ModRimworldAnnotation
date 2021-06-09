using System;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06001933 RID: 6451 RVA: 0x000E1724 File Offset: 0x000DF924
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
