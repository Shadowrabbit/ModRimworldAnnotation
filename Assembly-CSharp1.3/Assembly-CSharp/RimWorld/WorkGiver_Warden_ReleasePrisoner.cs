using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000832 RID: 2098
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		// Token: 0x06003795 RID: 14229 RVA: 0x001395E8 File Offset: 0x001377E8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.interactionMode != PrisonerInteractionModeDefOf.Release || pawn2.Downed)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.ReleasePrisoner, pawn2, c);
			job.count = 1;
			return job;
		}
	}
}
