using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D91 RID: 3473
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x06004F36 RID: 20278 RVA: 0x001B4414 File Offset: 0x001B2614
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04003373 RID: 13171
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
