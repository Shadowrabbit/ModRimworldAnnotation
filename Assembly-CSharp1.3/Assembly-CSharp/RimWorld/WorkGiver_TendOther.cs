using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000864 RID: 2148
	public class WorkGiver_TendOther : WorkGiver_Tend
	{
		// Token: 0x060038B7 RID: 14519 RVA: 0x0013DCB7 File Offset: 0x0013BEB7
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && pawn != t;
		}
	}
}
