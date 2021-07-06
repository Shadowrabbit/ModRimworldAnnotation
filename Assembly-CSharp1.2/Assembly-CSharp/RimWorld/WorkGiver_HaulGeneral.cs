using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D82 RID: 3458
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x06004EE2 RID: 20194 RVA: 0x00037906 File Offset: 0x00035B06
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t is Corpse)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}
	}
}
