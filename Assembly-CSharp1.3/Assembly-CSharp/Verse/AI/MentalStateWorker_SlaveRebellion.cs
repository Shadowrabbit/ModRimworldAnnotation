using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005D7 RID: 1495
	public class MentalStateWorker_SlaveRebellion : MentalStateWorker
	{
		// Token: 0x06002B48 RID: 11080 RVA: 0x00102E25 File Offset: 0x00101025
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && SlaveRebellionUtility.FindSlaveForRebellion(pawn) != null;
		}
	}
}
