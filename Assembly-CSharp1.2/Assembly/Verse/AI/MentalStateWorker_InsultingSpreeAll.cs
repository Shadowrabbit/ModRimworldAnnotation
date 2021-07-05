using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A41 RID: 2625
	public class MentalStateWorker_InsultingSpreeAll : MentalStateWorker
	{
		// Token: 0x06003E85 RID: 16005 RVA: 0x0002EFA5 File Offset: 0x0002D1A5
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

		// Token: 0x04002B07 RID: 11015
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
