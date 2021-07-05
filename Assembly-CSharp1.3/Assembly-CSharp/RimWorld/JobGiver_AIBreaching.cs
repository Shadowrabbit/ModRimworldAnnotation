using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000776 RID: 1910
	public class JobGiver_AIBreaching : ThinkNode_JobGiver
	{
		// Token: 0x060034A8 RID: 13480 RVA: 0x0012A604 File Offset: 0x00128804
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			if (cell.IsValid && (float)cell.DistanceToSquared(pawn.Position) < 25f && cell.GetRoom(pawn.Map) == pawn.GetRoom(RegionType.Set_All) && cell.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
			{
				pawn.GetLord().Notify_ReachedDutyLocation(pawn);
				return null;
			}
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			if (verb == null)
			{
				return null;
			}
			this.UpdateBreachingTarget(pawn, verb);
			BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
			if (breachingTarget == null)
			{
				if (cell.IsValid && pawn.CanReach(cell, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					Job job = JobMaker.MakeJob(JobDefOf.Goto, cell, 500, true);
					BreachingUtility.FinalizeTrashJob(job);
					return job;
				}
				return null;
			}
			else
			{
				if (!breachingTarget.firingPosition.IsValid)
				{
					return null;
				}
				Thing target = breachingTarget.target;
				IntVec3 firingPosition = breachingTarget.firingPosition;
				if (verb.IsMeleeAttack)
				{
					Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, target, firingPosition);
					job2.verbToUse = verb;
					BreachingUtility.FinalizeTrashJob(job2);
					return job2;
				}
				bool flag = firingPosition.Standable(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(firingPosition, pawn, false);
				Job job3 = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, target, flag ? firingPosition : IntVec3.Invalid);
				job3.verbToUse = verb;
				job3.preventFriendlyFire = true;
				BreachingUtility.FinalizeTrashJob(job3);
				return job3;
			}
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x0012A790 File Offset: 0x00128990
		private void UpdateBreachingTarget(Pawn pawn, Verb verb)
		{
			try
			{
				LordToil_AssaultColonyBreaching lordToil_AssaultColonyBreaching = BreachingUtility.LordToilOf(pawn);
				if (lordToil_AssaultColonyBreaching != null)
				{
					lordToil_AssaultColonyBreaching.UpdateCurrentBreachTarget();
					LordToilData_AssaultColonyBreaching data = lordToil_AssaultColonyBreaching.Data;
					BreachingGrid breachingGrid = lordToil_AssaultColonyBreaching.Data.breachingGrid;
					BreachingTargetData breachingTargetData = pawn.mindState.breachingTarget;
					bool flag = false;
					if (breachingTargetData != null && (breachingTargetData.target.Destroyed || data.currentTarget != breachingTargetData.target || breachingGrid.MarkerGrid[pawn.Position] == 10 || (breachingTargetData.firingPosition.IsValid && !verb.IsMeleeAttack && !verb.CanHitTargetFrom(breachingTargetData.firingPosition, breachingTargetData.target)) || (breachingTargetData.firingPosition.IsValid && !pawn.CanReach(breachingTargetData.firingPosition, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)) || (data.soloAttacker != null && pawn != data.soloAttacker)))
					{
						breachingTargetData = null;
						flag = true;
					}
					if (breachingTargetData == null && data.currentTarget != null)
					{
						flag = true;
						breachingTargetData = new BreachingTargetData(data.currentTarget, IntVec3.Invalid);
					}
					bool flag2 = BreachingUtility.IsSoloAttackVerb(verb) ? (data.soloAttacker == pawn) : (data.soloAttacker == null);
					if (breachingTargetData != null && !breachingTargetData.firingPosition.IsValid && BreachingUtility.CanDamageTarget(verb, breachingTargetData.target) && flag2 && BreachingUtility.TryFindCastPosition(pawn, verb, breachingTargetData.target, out breachingTargetData.firingPosition))
					{
						flag = true;
					}
					if (flag)
					{
						pawn.mindState.breachingTarget = breachingTargetData;
						breachingGrid.Notify_PawnStateChanged(pawn);
					}
				}
			}
			finally
			{
			}
		}

		// Token: 0x04001E5B RID: 7771
		private const float ReachDestDist = 5f;

		// Token: 0x04001E5C RID: 7772
		private const int CheckOverrideInterval = 500;

		// Token: 0x04001E5D RID: 7773
		private const float WanderDuringBusyJobChance = 0.3f;

		// Token: 0x04001E5E RID: 7774
		private static IntRange WanderTicks = new IntRange(30, 80);
	}
}
