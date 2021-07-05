using System;

namespace Verse
{
	// Token: 0x020002D9 RID: 729
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x0600139E RID: 5022 RVA: 0x0006F63A File Offset: 0x0006D83A
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x04000E71 RID: 3697
		public float mtbDays;
	}
}
