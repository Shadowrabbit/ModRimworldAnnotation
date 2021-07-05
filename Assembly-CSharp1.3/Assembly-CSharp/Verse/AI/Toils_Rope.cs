using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005B8 RID: 1464
	public static class Toils_Rope
	{
		// Token: 0x06002AC1 RID: 10945 RVA: 0x00100A58 File Offset: 0x000FEC58
		public static Toil RopePawn(TargetIndex ropeeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ropeeInd).Thing as Pawn;
				if (pawn != null)
				{
					toil.actor.roping.RopePawn(pawn);
					Pawn_CallTracker caller = pawn.caller;
					if (caller != null)
					{
						caller.DoCall();
					}
					PawnUtility.ForceWait(pawn, 30, actor, false);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 30;
			toil.FailOnDespawnedOrNull(ropeeInd);
			toil.PlaySustainerOrSound(() => SoundDefOf.Roping, 1f);
			return toil;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x00100AF8 File Offset: 0x000FECF8
		public static Toil UnropeFromSpot(TargetIndex ropeeInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = actor.jobs.curJob.GetTarget(ropeeInd).Thing as Pawn;
				if (pawn != null && pawn.roping.IsRopedToSpot)
				{
					pawn.roping.UnropeFromSpot();
					Pawn_CallTracker caller = pawn.caller;
					if (caller != null)
					{
						caller.DoCall();
					}
					PawnUtility.ForceWait(pawn, 30, actor, false);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 30;
			toil.FailOnDespawnedOrNull(ropeeInd);
			return toil;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x00100B68 File Offset: 0x000FED68
		public static Toil GotoRopeAttachmentInteractionCell(TargetIndex ropeeIndex)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn ropee = actor.CurJob.GetTarget(ropeeIndex).Thing as Pawn;
				IntVec3 intVec = AnimalPenUtility.RopeAttachmentInteractionCell(actor, ropee);
				if (!intVec.IsValid)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
				if (actor.Position == intVec)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(intVec, PathEndMode.OnCell);
			};
			toil.tickAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn ropee = actor.CurJob.GetTarget(ropeeIndex).Thing as Pawn;
				if (actor.pather.Moving && !AnimalPenUtility.IsGoodRopeAttachmentInteractionCell(actor, ropee, actor.pather.Destination.Cell))
				{
					IntVec3 c = AnimalPenUtility.RopeAttachmentInteractionCell(actor, ropee);
					if (c.IsValid)
					{
						actor.pather.StartPath(c, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedOrNull(ropeeIndex);
			return toil;
		}

		// Token: 0x04001A4F RID: 6735
		private const int RopeWorkDuration = 30;
	}
}
