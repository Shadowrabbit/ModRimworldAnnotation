using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200062E RID: 1582
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x06002D53 RID: 11603 RVA: 0x0010FA50 File Offset: 0x0010DC50
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

		// Token: 0x06002D54 RID: 11604 RVA: 0x0010FAC0 File Offset: 0x0010DCC0
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig && !pawn.CanReachMapEdge()) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction, true, true));
			IntVec3 c;
			if (!this.TryFindGoodExitDest(pawn, flag, out c))
			{
				return null;
			}
			if (flag)
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false, false, false), PathEndMode.OnCell, null))
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
			job2.canBashDoors = this.canBash;
			return job2;
		}

		// Token: 0x06002D55 RID: 11605
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 dest);

		// Token: 0x04001BCA RID: 7114
		protected LocomotionUrgency defaultLocomotion;

		// Token: 0x04001BCB RID: 7115
		protected int jobMaxDuration = 999999;

		// Token: 0x04001BCC RID: 7116
		protected bool canBash;

		// Token: 0x04001BCD RID: 7117
		protected bool forceCanDig;

		// Token: 0x04001BCE RID: 7118
		protected bool forceCanDigIfAnyHostileActiveThreat;

		// Token: 0x04001BCF RID: 7119
		protected bool forceCanDigIfCantReachMapEdge;

		// Token: 0x04001BD0 RID: 7120
		protected bool failIfCantJoinOrCreateCaravan;
	}
}
