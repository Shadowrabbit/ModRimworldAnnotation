using System;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x06001945 RID: 6469 RVA: 0x00017D49 File Offset: 0x00015F49
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (cause.Severity < this.minSeverity)
			{
				return;
			}
			if (Rand.MTBEventOccurs(this.baseMtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040012EA RID: 4842
		public float baseMtbDays;

		// Token: 0x040012EB RID: 4843
		public float minSeverity;
	}
}
