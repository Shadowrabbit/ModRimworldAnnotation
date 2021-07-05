using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x06003569 RID: 13673 RVA: 0x0012E0DC File Offset: 0x0012C2DC
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

		// Token: 0x04001E96 RID: 7830
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x04001E97 RID: 7831
		private const float VictimSearchRadiusOngoing = 18f;
	}
}
