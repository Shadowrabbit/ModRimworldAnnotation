using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D65 RID: 3429
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		// Token: 0x06004E49 RID: 20041 RVA: 0x001B0D84 File Offset: 0x001AEF84
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner_NewTemp(pawn, t, forced))
			{
				return null;
			}
			Pawn prisoner = (Pawn)t;
			Job job = this.TakeDownedToBedJob(prisoner, pawn);
			if (job != null)
			{
				return job;
			}
			Job job2 = this.TakeToPreferredBedJob(prisoner, pawn);
			if (job2 != null)
			{
				return job2;
			}
			return null;
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x001B0DC4 File Offset: 0x001AEFC4
		private Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			if (prisoner.Downed || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			if (RestUtility.FindBedFor(prisoner, prisoner, true, true, false) != null)
			{
				return null;
			}
			Room room = prisoner.GetRoom(RegionType.Set_Passable);
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, false);
			if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_Passable) != room)
			{
				Job job = JobMaker.MakeJob(JobDefOf.EscortPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x001B0E3C File Offset: 0x001AF03C
		private Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) || prisoner.InBed() || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, true, false);
			if (building_Bed != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeWoundedPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}
	}
}
