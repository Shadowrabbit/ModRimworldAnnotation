using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200076B RID: 1899
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		// Token: 0x06003467 RID: 13415 RVA: 0x001293EC File Offset: 0x001275EC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (pawn.mindState.knownExploder == null)
			{
				return null;
			}
			if (!pawn.mindState.knownExploder.Spawned)
			{
				pawn.mindState.knownExploder = null;
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			Thing knownExploder = pawn.mindState.knownExploder;
			if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81f)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}

		// Token: 0x04001E4E RID: 7758
		public const float FleeDist = 9f;
	}
}
