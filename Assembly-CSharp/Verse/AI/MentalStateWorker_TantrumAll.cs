using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A43 RID: 2627
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06003E8B RID: 16011 RVA: 0x001789B8 File Offset: 0x00176BB8
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TantrumAll.tmpThings, null, 0, 40);
			bool result = MentalStateWorker_TantrumAll.tmpThings.Count >= 2;
			MentalStateWorker_TantrumAll.tmpThings.Clear();
			return result;
		}

		// Token: 0x04002B09 RID: 11017
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
