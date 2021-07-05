using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000834 RID: 2100
	public class WorkGiver_Warden_TakeToBed : WorkGiver_Warden
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x001396F1 File Offset: 0x001378F1
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			return WorkGiver_Warden_TakeToBed.TryMakeJob(pawn, t, forced);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00139708 File Offset: 0x00137908
		public static Job TryMakeJob(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn prisoner = (Pawn)t;
			Job job = WorkGiver_Warden_TakeToBed.TakeDownedToBedJob(prisoner, pawn);
			if (job != null)
			{
				return job;
			}
			Job job2 = WorkGiver_Warden_TakeToBed.TakeToPreferredBedJob(prisoner, pawn);
			if (job2 != null)
			{
				return job2;
			}
			return null;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x00139738 File Offset: 0x00137938
		private static Job TakeToPreferredBedJob(Pawn prisoner, Pawn warden)
		{
			if (prisoner.Downed || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			if (RestUtility.FindBedFor(prisoner, prisoner, true, false, new GuestStatus?(GuestStatus.Prisoner)) != null)
			{
				return null;
			}
			Room room = prisoner.GetRoom(RegionType.Set_All);
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, false, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed != null && building_Bed.GetRoom(RegionType.Set_All) != room)
			{
				Job job = JobMaker.MakeJob(JobDefOf.EscortPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001397BC File Offset: 0x001379BC
		private static Job TakeDownedToBedJob(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Downed || !HealthAIUtility.ShouldSeekMedicalRestUrgent(prisoner) || prisoner.InBed() || !warden.CanReserve(prisoner, 1, -1, null, false))
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(prisoner, warden, true, false, new GuestStatus?(GuestStatus.Prisoner));
			if (building_Bed != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeWoundedPrisonerToBed, prisoner, building_Bed);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00139828 File Offset: 0x00137A28
		public static void TryTakePrisonerToBed(Pawn prisoner, Pawn warden)
		{
			if (!prisoner.Spawned || prisoner.InAggroMentalState || prisoner.IsForbidden(warden) || prisoner.IsFormingCaravan() || !warden.CanReserveAndReach(prisoner, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, true))
			{
				return;
			}
			Job job = WorkGiver_Warden_TakeToBed.TryMakeJob(warden, prisoner, true);
			if (job != null)
			{
				warden.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
		}
	}
}
