using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A49 RID: 2633
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06003E9B RID: 16027 RVA: 0x0002F071 File Offset: 0x0002D271
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
