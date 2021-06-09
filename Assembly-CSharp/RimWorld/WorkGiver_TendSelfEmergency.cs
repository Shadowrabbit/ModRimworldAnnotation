using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DA9 RID: 3497
	public class WorkGiver_TendSelfEmergency : WorkGiver_TendSelf
	{
		// Token: 0x06004FB6 RID: 20406 RVA: 0x001B5230 File Offset: 0x001B3430
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

		// Token: 0x04003393 RID: 13203
		private static JobGiver_SelfTend jgp = new JobGiver_SelfTend();
	}
}
