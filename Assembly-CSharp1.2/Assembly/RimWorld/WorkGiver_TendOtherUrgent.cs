using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA6 RID: 3494
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x06004FA8 RID: 20392 RVA: 0x00037F9C File Offset: 0x0003619C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
