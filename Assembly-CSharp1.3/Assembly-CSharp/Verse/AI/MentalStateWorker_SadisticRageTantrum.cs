using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005D1 RID: 1489
	public class MentalStateWorker_SadisticRageTantrum : MentalStateWorker
	{
		// Token: 0x06002B3B RID: 11067 RVA: 0x00102D20 File Offset: 0x00100F20
		public override bool StateCanOccur(Pawn pawn)
		{
			if (!base.StateCanOccur(pawn))
			{
				return false;
			}
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			TantrumMentalStateUtility.GetSmashableThingsNear(pawn, pawn.Position, MentalStateWorker_SadisticRageTantrum.tmpThings, (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(pawn, x), 0, 40);
			bool result = MentalStateWorker_SadisticRageTantrum.tmpThings.Any<Thing>();
			MentalStateWorker_SadisticRageTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x04001A72 RID: 6770
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
