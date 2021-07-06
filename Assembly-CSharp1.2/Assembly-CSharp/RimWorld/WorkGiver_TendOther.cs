using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA5 RID: 3493
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x06004FA6 RID: 20390 RVA: 0x00037F7E File Offset: 0x0003617E
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
