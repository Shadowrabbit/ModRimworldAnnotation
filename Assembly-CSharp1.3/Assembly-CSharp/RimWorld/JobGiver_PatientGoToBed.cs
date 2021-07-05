using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CA RID: 1994
	public class JobGiver_PatientGoToBed : ThinkNode
	{
		// Token: 0x060035C2 RID: 13762 RVA: 0x00130704 File Offset: 0x0012E904
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return ThinkResult.NoJob;
			}
			if (this.respectTimetable && RestUtility.TimetablePreventsLayDown(pawn) && !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) && !HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
			{
				return ThinkResult.NoJob;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return ThinkResult.NoJob;
			}
			Thing thing = RestUtility.FindPatientBedFor(pawn);
			if (thing == null)
			{
				return ThinkResult.NoJob;
			}
			return new ThinkResult(JobMaker.MakeJob(JobDefOf.LayDown, thing), this, null, false);
		}

		// Token: 0x04001EBF RID: 7871
		public bool respectTimetable = true;
	}
}
