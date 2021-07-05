using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000844 RID: 2116
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x0600380C RID: 14348 RVA: 0x0013BC7E File Offset: 0x00139E7E
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
