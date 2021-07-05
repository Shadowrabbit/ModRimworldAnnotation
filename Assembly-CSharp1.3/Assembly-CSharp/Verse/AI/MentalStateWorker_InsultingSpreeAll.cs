using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005CC RID: 1484
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x06002B2C RID: 11052 RVA: 0x00102B67 File Offset: 0x00100D67
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(pawn, MentalStateWorker_InsultingSpreeAll.candidates, true);
			bool result = MentalStateWorker_InsultingSpreeAll.candidates.Count >= 2;
			MentalStateWorker_InsultingSpreeAll.candidates.Clear();
			return result;
		}

		// Token: 0x04001A6D RID: 6765
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
