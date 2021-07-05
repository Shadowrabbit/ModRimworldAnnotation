using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005D0 RID: 1488
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06002B38 RID: 11064 RVA: 0x00102CC4 File Offset: 0x00100EC4
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalStateWorker_TargetedTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04001A71 RID: 6769
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
