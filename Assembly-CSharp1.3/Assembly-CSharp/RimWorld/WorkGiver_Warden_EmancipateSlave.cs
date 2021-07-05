using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200082C RID: 2092
	public class WorkGiver_Warden_EmancipateSlave : WorkGiver_Warden
	{
		// Token: 0x06003788 RID: 14216 RVA: 0x00139294 File Offset: 0x00137494
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Slave imprisonment"))
			{
				return null;
			}
			Pawn pawn2 = t as Pawn;
			if (!base.ShouldTakeCareOfSlave(pawn, pawn2, false))
			{
				return null;
			}
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Emancipate)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.SlaveEmancipation, pawn2);
			job.count = 1;
			return job;
		}
	}
}
