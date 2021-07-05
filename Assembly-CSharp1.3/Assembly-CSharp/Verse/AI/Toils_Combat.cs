using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020005AF RID: 1455
	public static class Toils_Combat
	{
		// Token: 0x06002A92 RID: 10898 RVA: 0x000FFD14 File Offset: 0x000FDF14
		public static Toil TrySetJobToUseAttackVerb(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				bool allowManualCastWeapons = !actor.IsColonist;
				Verb verb = actor.TryGetAttackVerb(curJob.GetTarget(targetInd).Thing, allowManualCastWeapons);
				if (verb == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				curJob.verbToUse = verb;
			};
			return toil;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000FFD58 File Offset: 0x000FDF58
		public static Toil GotoCastPosition(TargetIndex targetInd, TargetIndex castPositionInd = TargetIndex.None, bool closeIfDowned = false, float maxRangeFactor = 1f)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				if (actor == thing)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (thing == null)
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				CastPositionRequest newReq = default(CastPositionRequest);
				newReq.caster = toil.actor;
				newReq.target = thing;
				newReq.verb = curJob.verbToUse;
				newReq.maxRangeFromTarget = ((!closeIfDowned || pawn == null || !pawn.Downed) ? Mathf.Max(curJob.verbToUse.verbProps.range * maxRangeFactor, 1.42f) : Mathf.Min(curJob.verbToUse.verbProps.range, (float)pawn.RaceProps.executionRange));
				newReq.wantCoverFromTarget = false;
				if (castPositionInd != TargetIndex.None)
				{
					newReq.preferredCastPosition = new IntVec3?(curJob.GetTarget(castPositionInd).Cell);
				}
				IntVec3 intVec;
				if (!CastPositionFinder.TryFindCastPosition(newReq, out intVec))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				toil.actor.pather.StartPath(intVec, PathEndMode.OnCell);
				actor.Map.pawnDestinationReservationManager.Reserve(actor, curJob, intVec);
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000FFDCD File Offset: 0x000FDFCD
		public static Toil CastVerb(TargetIndex targetInd, bool canHitNonTargetPawns = true)
		{
			return Toils_Combat.CastVerb(targetInd, TargetIndex.None, canHitNonTargetPawns);
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000FFDD8 File Offset: 0x000FDFD8
		public static Toil CastVerb(TargetIndex targetInd, TargetIndex destInd, bool canHitNonTargetPawns = true)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(targetInd);
				LocalTargetInfo destTarg = (destInd != TargetIndex.None) ? toil.actor.jobs.curJob.GetTarget(destInd) : LocalTargetInfo.Invalid;
				toil.actor.jobs.curJob.verbToUse.TryStartCastOn(target, destTarg, false, canHitNonTargetPawns, toil.actor.jobs.curJob.preventFriendlyFire);
			};
			toil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
			toil.activeSkill = (() => Toils_Combat.GetActiveSkillForToil(toil));
			return toil;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000FFE4C File Offset: 0x000FE04C
		public static SkillDef GetActiveSkillForToil(Toil toil)
		{
			Verb verbToUse = toil.actor.jobs.curJob.verbToUse;
			if (verbToUse != null && verbToUse.EquipmentSource != null)
			{
				if (verbToUse.EquipmentSource.def.IsMeleeWeapon)
				{
					return SkillDefOf.Melee;
				}
				if (verbToUse.EquipmentSource.def.IsRangedWeapon)
				{
					return SkillDefOf.Shooting;
				}
			}
			return null;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000FFEAB File Offset: 0x000FE0AB
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, Action hitAction)
		{
			return Toils_Combat.FollowAndMeleeAttack(targetInd, TargetIndex.None, hitAction);
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000FFEB8 File Offset: 0x000FE0B8
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, TargetIndex standPositionInd, Action hitAction)
		{
			Toil followAndAttack = new Toil();
			followAndAttack.tickAction = delegate()
			{
				Pawn actor = followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				LocalTargetInfo target = curJob.GetTarget(targetInd);
				Thing thing = target.Thing;
				Pawn pawn = thing as Pawn;
				if (!thing.Spawned || (pawn != null && (pawn.IsInvisible() || (curJob.endIfAllyNotAThreatAnymore && GenHostility.IsOfFriendlyFactionAndNonThreatAnymore(actor, pawn)))))
				{
					curDriver.ReadyForNextToil();
					return;
				}
				LocalTargetInfo localTargetInfo = target;
				PathEndMode peMode = PathEndMode.Touch;
				if (standPositionInd != TargetIndex.None)
				{
					LocalTargetInfo target2 = curJob.GetTarget(standPositionInd);
					if (target2.IsValid)
					{
						localTargetInfo = target2;
						peMode = PathEndMode.OnCell;
					}
				}
				if (localTargetInfo != actor.pather.Destination || (!actor.pather.Moving && !actor.CanReachImmediate(target, PathEndMode.Touch)))
				{
					actor.pather.StartPath(localTargetInfo, peMode);
					return;
				}
				if (actor.CanReachImmediate(target, PathEndMode.Touch))
				{
					if (pawn != null && pawn.Downed && !curJob.killIncappedTarget)
					{
						curDriver.ReadyForNextToil();
						return;
					}
					hitAction();
				}
			};
			followAndAttack.activeSkill = (() => SkillDefOf.Melee);
			followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
			return followAndAttack;
		}
	}
}
