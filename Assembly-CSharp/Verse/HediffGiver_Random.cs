using System;

namespace Verse
{
	// Token: 0x02000412 RID: 1042
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06001943 RID: 6467 RVA: 0x00017D1E File Offset: 0x00015F1E
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040012E9 RID: 4841
		public float mtbDays;
	}
}
