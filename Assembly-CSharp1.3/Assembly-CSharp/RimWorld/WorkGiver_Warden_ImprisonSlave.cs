using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000831 RID: 2097
	public class WorkGiver_Warden_ImprisonSlave : WorkGiver_Warden
	{
		// Token: 0x06003793 RID: 14227 RVA: 0x00139570 File Offset: 0x00137770
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
			if (pawn2.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Imprison)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, pawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Arrest, pawn2, building_Bed);
			job.count = 1;
			return job;
		}
	}
}
