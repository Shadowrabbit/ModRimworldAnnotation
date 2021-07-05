using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000851 RID: 2129
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x0600384D RID: 14413 RVA: 0x0013CBBC File Offset: 0x0013ADBC
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04001F31 RID: 7985
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
