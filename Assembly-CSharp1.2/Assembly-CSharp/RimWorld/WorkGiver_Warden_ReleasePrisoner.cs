using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D64 RID: 3428
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		// Token: 0x06004E47 RID: 20039 RVA: 0x001B0D1C File Offset: 0x001AEF1C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner_NewTemp(pawn, t, forced))
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
