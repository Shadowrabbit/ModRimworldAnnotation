using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005D3 RID: 1491
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06002B40 RID: 11072 RVA: 0x00102DB4 File Offset: 0x00100FB4
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
