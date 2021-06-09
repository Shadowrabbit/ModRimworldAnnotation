using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A45 RID: 2629
	public class MentalStateWorker_TargetedTantrum : MentalStateWorker
	{
		// Token: 0x06003E91 RID: 16017 RVA: 0x00178A80 File Offset: 0x00176C80
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

		// Token: 0x04002B0B RID: 11019
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
