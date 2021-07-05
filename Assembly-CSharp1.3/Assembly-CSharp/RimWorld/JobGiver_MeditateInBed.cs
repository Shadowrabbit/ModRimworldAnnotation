using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C7 RID: 1991
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		// Token: 0x060035B4 RID: 13748 RVA: 0x0012F8A7 File Offset: 0x0012DAA7
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake() && MeditationUtility.ShouldMeditateInBed(pawn);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0012F8C4 File Offset: 0x0012DAC4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!MeditationUtility.CanMeditateNow(pawn))
			{
				return null;
			}
			LocalTargetInfo targetC = ModsConfig.RoyaltyActive ? MeditationUtility.BestFocusAt(pawn.Position, pawn) : LocalTargetInfo.Invalid;
			Job job = JobMaker.MakeJob(JobDefOf.Meditate, pawn.Position, pawn.InBed() ? pawn.CurrentBed() : new LocalTargetInfo(pawn.Position), targetC);
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
