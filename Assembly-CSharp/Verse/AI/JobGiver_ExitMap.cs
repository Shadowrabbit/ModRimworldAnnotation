using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A8C RID: 2700
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x06004033 RID: 16435 RVA: 0x001822EC File Offset: 0x001804EC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ExitMap jobGiver_ExitMap = (JobGiver_ExitMap)base.DeepCopy(resolve);
			jobGiver_ExitMap.defaultLocomotion = this.defaultLocomotion;
			jobGiver_ExitMap.jobMaxDuration = this.jobMaxDuration;
			jobGiver_ExitMap.canBash = this.canBash;
			jobGiver_ExitMap.forceCanDig = this.forceCanDig;
			jobGiver_ExitMap.forceCanDigIfAnyHostileActiveThreat = this.forceCanDigIfAnyHostileActiveThreat;
			jobGiver_ExitMap.forceCanDigIfCantReachMapEdge = this.forceCanDigIfCantReachMapEdge;
			jobGiver_ExitMap.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			return jobGiver_ExitMap;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0018235C File Offset: 0x0018055C
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, true));
			IntVec3 c;
			if (!this.TryFindGoodExitDest(pawn, flag, out c))
			{
				return null;
			}
			if (flag)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false), PathEndMode.OnCell))
				{
					IntVec3 cellBeforeBlocker;
					Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
					if (thing != null)
					{
						Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
						if (job != null)
						{
							return job;
						}
					}
				}
			}
			Job job2 = JobMaker.MakeJob(JobDefOf.Goto, c);
			job2.exitMapOnArrival = true;
			job2.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			job2.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.defaultLocomotion, LocomotionUrgency.Jog);
			job2.expiryInterval = this.jobMaxDuration;
			job2.canBash = this.canBash;
			return job2;
		}

		// Token: 0x06004035 RID: 16437
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 dest);

		// Token: 0x04002C42 RID: 11330
		protected LocomotionUrgency defaultLocomotion;

		// Token: 0x04002C43 RID: 11331
		protected int jobMaxDuration = 999999;

		// Token: 0x04002C44 RID: 11332
		protected bool canBash;

		// Token: 0x04002C45 RID: 11333
		protected bool forceCanDig;

		// Token: 0x04002C46 RID: 11334
		protected bool forceCanDigIfAnyHostileActiveThreat;

		// Token: 0x04002C47 RID: 11335
		protected bool forceCanDigIfCantReachMapEdge;

		// Token: 0x04002C48 RID: 11336
		protected bool failIfCantJoinOrCreateCaravan;
	}
}
