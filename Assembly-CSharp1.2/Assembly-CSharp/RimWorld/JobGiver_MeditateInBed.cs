using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD9 RID: 3289
	public class JobGiver_MeditateInBed : JobGiver_Meditate
	{
		// Token: 0x06004BDF RID: 19423 RVA: 0x00036063 File Offset: 0x00034263
		protected override bool ValidatePawnState(Pawn pawn)
		{
			return pawn.CurrentBed() != null && pawn.Awake() && MeditationUtility.ShouldMeditateInBed(pawn);
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x001A7554 File Offset: 0x001A5754
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
