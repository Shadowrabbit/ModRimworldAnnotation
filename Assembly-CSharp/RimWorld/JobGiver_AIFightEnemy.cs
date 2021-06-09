using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C96 RID: 3222
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		// Token: 0x06004B14 RID: 19220
		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest);

		// Token: 0x06004B15 RID: 19221 RVA: 0x00035943 File Offset: 0x00033B43
		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x00030CD3 File Offset: 0x0002EED3
		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			return true;
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0003594A File Offset: 0x00033B4A
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			return jobGiver_AIFightEnemy;
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x001A40EC File Offset: 0x001A22EC
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

		// Token: 0x06004B1A RID: 19226 RVA: 0x001A4260 File Offset: 0x001A2460
		protected virtual Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, enemyTarget);
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_Melee.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
			return job;
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x001A42A0 File Offset: 0x001A24A0
		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null && (thing.Destroyed || Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > 400 || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (float)(pawn.Position - thing.Position).LengthHorizontalSquared > this.targetKeepRadius * this.targetKeepRadius || ((IAttackTarget)thing).ThreatDisabled(pawn)))
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

		// Token: 0x06004B1C RID: 19228 RVA: 0x00035988 File Offset: 0x00033B88
		private Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			if (pawn.TryGetAttackVerb(null, !pawn.IsColonist) == null)
			{
				return null;
			}
			return this.FindAttackTarget(pawn);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x001A43D0 File Offset: 0x001A25D0
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
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Thing x) => this.ExtraTargetValidator(pawn, x), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false, true);
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x001A4458 File Offset: 0x001A2658
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

		// Token: 0x040031B3 RID: 12723
		private float targetAcquireRadius = 56f;

		// Token: 0x040031B4 RID: 12724
		private float targetKeepRadius = 65f;

		// Token: 0x040031B5 RID: 12725
		private bool needLOSToAcquireNonPawnTargets;

		// Token: 0x040031B6 RID: 12726
		private bool chaseTarget;

		// Token: 0x040031B7 RID: 12727
		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		// Token: 0x040031B8 RID: 12728
		private static readonly IntRange ExpiryInterval_Melee = new IntRange(360, 480);

		// Token: 0x040031B9 RID: 12729
		private const int MinTargetDistanceToMove = 5;

		// Token: 0x040031BA RID: 12730
		private const int TicksSinceEngageToLoseTarget = 400;
	}
}
