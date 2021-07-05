using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000798 RID: 1944
	public class JobGiver_Duel : JobGiver_AIFightEnemies
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x0012CDB0 File Offset: 0x0012AFB0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Duel lordJob_Ritual_Duel;
			if ((lordJob_Ritual_Duel = (((lord != null) ? lord.LordJob : null) as LordJob_Ritual_Duel)) == null)
			{
				return null;
			}
			lordJob_Ritual_Duel.StartDuelIfNotStartedYet();
			if (lordJob_Ritual_Duel.CurrentStage == DuelBehaviorStage.Attack)
			{
				return base.TryGiveJob(pawn);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, this.GetMoveTarget(pawn, lordJob_Ritual_Duel), lordJob_Ritual_Duel.Opponent(pawn));
			job.checkOverrideOnExpire = true;
			job.expiryInterval = 40;
			job.collideWithPawns = true;
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x0012CE2C File Offset: 0x0012B02C
		private LocalTargetInfo GetMoveTarget(Pawn pawn, LordJob_Ritual_Duel duel)
		{
			Pawn opponent = duel.Opponent(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, duel.selectedTarget.Cell, 3.1f, delegate(Pawn p, IntVec3 c, IntVec3 r)
			{
				if (c == pawn.Position || !c.Standable(p.Map) || !p.CanReserveAndReach(c, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) || c.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
				{
					return false;
				}
				Job curJob = opponent.CurJob;
				IntVec3 a = (((curJob != null) ? curJob.def : null) == JobDefOf.Goto) ? opponent.CurJob.targetA.Cell : IntVec3.Invalid;
				if (c.DistanceTo(opponent.Position) < 1.9f || (a != IntVec3.Invalid && a.DistanceTo(c) < 1.9f))
				{
					return false;
				}
				PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, c, pawn, PathEndMode.OnCell, null);
				try
				{
					foreach (IntVec3 a2 in pawnPath.NodesReversed)
					{
						if (a2.DistanceTo(opponent.Position) < 1.9f || a2.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
						{
							return false;
						}
					}
					if (a != IntVec3.Invalid && opponent.pather.curPath != null)
					{
						foreach (IntVec3 a3 in opponent.pather.curPath.NodesReversed)
						{
							if (a3.DistanceTo(pawn.Position) < 1.9f || a3.DistanceTo(duel.selectedTarget.Cell) > 3.1f)
							{
								return false;
							}
							foreach (IntVec3 b in pawnPath.NodesReversed)
							{
								if (a3.DistanceTo(b) < 1.9f)
								{
									return false;
								}
							}
						}
					}
				}
				finally
				{
					pawnPath.ReleaseToPool();
				}
				return true;
			}, Danger.Deadly);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0012CE98 File Offset: 0x0012B098
		protected override void UpdateEnemyTarget(Pawn pawn)
		{
			Pawn pawn2 = ((LordJob_Ritual_Duel)pawn.GetLord().LordJob).Opponent(pawn);
			if (pawn2 == null || pawn2.Dead)
			{
				pawn.mindState.enemyTarget = null;
				return;
			}
			pawn.mindState.enemyTarget = pawn2;
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x0012CEE0 File Offset: 0x0012B0E0
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.killIncappedTarget = true;
			job.endIfAllyNotAThreatAnymore = false;
			return job;
		}

		// Token: 0x04001E7B RID: 7803
		public const float MinDistOpponentWhenMoving = 1.9f;

		// Token: 0x04001E7C RID: 7804
		public const float MaxFightMoveDist = 3.1f;
	}
}
