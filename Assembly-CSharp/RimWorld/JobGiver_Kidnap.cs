using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CBE RID: 3262
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x06004B93 RID: 19347 RVA: 0x001A60D8 File Offset: 0x001A42D8
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				return null;
			}
			Pawn t;
			if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Kidnap);
				job.targetA = t;
				job.targetB = c;
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x040031E2 RID: 12770
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x040031E3 RID: 12771
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
