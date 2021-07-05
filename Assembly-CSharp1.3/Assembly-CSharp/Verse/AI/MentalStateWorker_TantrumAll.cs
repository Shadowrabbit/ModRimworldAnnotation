using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005CE RID: 1486
	public class MentalStateWorker_TantrumAll : MentalStateWorker
	{
		// Token: 0x06002B32 RID: 11058 RVA: 0x00102BE0 File Offset: 0x00100DE0
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

		// Token: 0x04001A6F RID: 6767
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
