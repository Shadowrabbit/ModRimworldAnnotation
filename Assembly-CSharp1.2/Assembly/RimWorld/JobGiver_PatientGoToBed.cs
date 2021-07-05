using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CDE RID: 3294
	public class JobGiver_PatientGoToBed : ThinkNode
	{
		// Token: 0x06004BF3 RID: 19443 RVA: 0x001A8108 File Offset: 0x001A6308
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

		// Token: 0x0400321B RID: 12827
		public bool respectTimetable = true;
	}
}
