using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200077C RID: 1916
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		// Token: 0x060034BE RID: 13502
		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest);

		// Token: 0x060034BF RID: 13503 RVA: 0x0012AC49 File Offset: 0x00128E49
		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00116BFF File Offset: 0x00114DFF
		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0012AC50 File Offset: 0x00128E50
		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			Pawn victim;
			return (victim = (target as Pawn)) == null || !GenHostility.IsOfFriendlyFactionAndNonThreatAnymore(pawn, victim);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x0012AC73 File Offset: 0x00128E73
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			return jobGiver_AIFightEnemy;
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x0012ACB4 File Offset: 0x00128EB4
		protected override Job TryGiveJob(Pawn pawn)
		{
			this.UpdateEnemyTarget(pawn);
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget == null)
			{
				return null;
			}
			Pawn pawn2 = enemyTarget as Pawn;
			if (pawn2 != null && pawn2.IsInvisible())
			{
				return null;
			}
			bool allowManualCastWeapons = !pawn.IsColonist;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
			if (verb == null)
			{
				return null;
			}
			if (verb.verbProps.IsMeleeAttack)
			{
				return this.MeleeAttackJob(enemyTarget);
			}
			bool flag = CoverUtility.CalculateOverallBlockChance(pawn, enemyTarget.Position, pawn.Map) > 0.01f;
			bool flag2 = pawn.Position.Standable(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted);
			bool flag3 = verb.CanHitTarget(enemyTarget);
			bool flag4 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
			if ((flag && flag2 && flag3) || (flag4 && flag3))
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
			}
			IntVec3 intVec;
			if (!this.TryFindShootingPosition(pawn, out intVec))
			{
				return null;
			}
			if (intVec == pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
			}
			Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
			job.checkOverrideOnExpire = true;
			return job;
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x0012AE28 File Offset: 0x00129028
		protected virtual Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, enemyTarget);
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_Melee.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
			job.endIfAllyNotAThreatAnymore = true;
			return job;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x0012AE70 File Offset: 0x00129070
		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null && (thing.Destroyed || Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > 400 || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || (float)(pawn.Position - thing.Position).LengthHorizontalSquared > this.targetKeepRadius * this.targetKeepRadius || ((IAttackTarget)thing).ThreatDisabled(pawn)))
			{
				thing = null;
			}
			if (thing == null)
			{
				thing = this.FindAttackTargetIfPossible(pawn);
				if (thing != null)
				{
					pawn.mindState.Notify_EngagedTarget();
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnAcquiredTarget(pawn, thing);
					}
				}
			}
			else
			{
				Thing thing2 = this.FindAttackTargetIfPossible(pawn);
				if (thing2 == null && !this.chaseTarget)
				{
					thing = null;
				}
				else if (thing2 != null && thing2 != thing)
				{
					pawn.mindState.Notify_EngagedTarget();
					thing = thing2;
				}
			}
			pawn.mindState.enemyTarget = thing;
			if (thing is Pawn && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x0012AFA0 File Offset: 0x001291A0
		private Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, !pawn.IsColonist) == null)
			{
				return null;
			}
			return this.FindAttackTarget(pawn);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x0012AFC0 File Offset: 0x001291C0
		protected virtual Thing FindAttackTarget(Pawn pawn)
		{
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (this.needLOSToAcquireNonPawnTargets)
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToNonPawns;
			}
			if (this.PrimaryVerbIsIncendiary(pawn))
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Thing x) => this.ExtraTargetValidator(pawn, x), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false, true, false);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x0012B04C File Offset: 0x0012924C
		private bool PrimaryVerbIsIncendiary(Pawn pawn)
		{
			if (pawn.equipment != null && pawn.equipment.Primary != null)
			{
				List<Verb> allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.isPrimary)
					{
						return allVerbs[i].IsIncendiary();
					}
				}
			}
			return false;
		}

		// Token: 0x04001E61 RID: 7777
		private float targetAcquireRadius = 56f;

		// Token: 0x04001E62 RID: 7778
		private float targetKeepRadius = 65f;

		// Token: 0x04001E63 RID: 7779
		private bool needLOSToAcquireNonPawnTargets;

		// Token: 0x04001E64 RID: 7780
		private bool chaseTarget;

		// Token: 0x04001E65 RID: 7781
		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		// Token: 0x04001E66 RID: 7782
		public static readonly IntRange ExpiryInterval_Melee = new IntRange(360, 480);

		// Token: 0x04001E67 RID: 7783
		private const int MinTargetDistanceToMove = 5;

		// Token: 0x04001E68 RID: 7784
		private const int TicksSinceEngageToLoseTarget = 400;
	}
}
