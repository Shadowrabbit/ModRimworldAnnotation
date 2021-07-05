using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A42 RID: 2626
	public class MentalStateWorker_TargetedInsultingSpree : MentalStateWorker
	{
		// Token: 0x06003E88 RID: 16008 RVA: 0x0002EFE4 File Offset: 0x0002D1E4
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

		// Token: 0x04002B08 RID: 11016
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
