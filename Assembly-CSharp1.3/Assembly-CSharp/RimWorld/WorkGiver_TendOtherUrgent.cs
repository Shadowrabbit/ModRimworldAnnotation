using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000865 RID: 2149
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		// Token: 0x060038B9 RID: 14521 RVA: 0x0013DCD5 File Offset: 0x0013BED5
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
