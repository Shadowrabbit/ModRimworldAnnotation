using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005CD RID: 1485
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x06002B2F RID: 11055 RVA: 0x00102BA6 File Offset: 0x00100DA6
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_TargetedInsultingSpree.candidates, false);
			bool result = MentalStateWorker_TargetedInsultingSpree.candidates.Any<Pawn>();
			MentalStateWorker_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x04001A6E RID: 6766
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
