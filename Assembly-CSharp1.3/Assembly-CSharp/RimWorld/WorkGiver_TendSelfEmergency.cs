using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000867 RID: 2151
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		// Token: 0x060038BF RID: 14527 RVA: 0x0013DD64 File Offset: 0x0013BF64
		public override Job NonScanJob(Pawn pawn)
		{
			if (!this.HasJobOnThing(pawn, pawn, false) || !HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				return null;
			}
			ThinkResult thinkResult = WorkGiver_TendSelfEmergency.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			if (thinkResult.IsValid)
			{
				return thinkResult.Job;
			}
			return null;
		}

		// Token: 0x04001F34 RID: 7988
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
