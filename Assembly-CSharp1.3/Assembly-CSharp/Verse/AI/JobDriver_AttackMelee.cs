using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000596 RID: 1430
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x060029CE RID: 10702 RVA: 0x000FCC50 File Offset: 0x000FAE50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000FCC6C File Offset: 0x000FAE6C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000FCCB5 File Offset: 0x000FAEB5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.job.targetA.Thing as Pawn;
				if (pawn != null && pawn.Downed && this.pawn.mindState.duty != null && this.pawn.mindState.duty.attackDownedIfStarving && this.pawn.Starving())
				{
					this.job.killIncappedTarget = true;
				}
			});
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				Pawn p;
				if (this.job.reactingToMeleeThreat && (p = (thing as Pawn)) != null && !p.Awake())
				{
					base.EndJobWith(JobCondition.InterruptForced);
					return;
				}
				if (this.pawn.meleeVerbs.TryMeleeAttack(thing, this.job.verbToUse, false))
				{
					if (this.pawn.CurJob == null || this.pawn.jobs.curDriver != this)
					{
						return;
					}
					Lord lord = this.pawn.GetLord();
					LordJob_Ritual_Duel lordJob_Ritual_Duel;
					if (((lord != null) ? lord.LordJob : null) != null && (lordJob_Ritual_Duel = (lord.LordJob as LordJob_Ritual_Duel)) != null)
					{
						lordJob_Ritual_Duel.Notify_MeleeAttack(this.pawn, thing);
					}
					this.numMeleeAttacksMade++;
					if (this.numMeleeAttacksMade >= this.job.maxNumMeleeAttacks)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
			}).FailOnDespawnedOrNull(TargetIndex.A);
			yield break;
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000FCCC8 File Offset: 0x000FAEC8
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPath(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false, false, false), PathEndMode.OnCell, null))
				{
					if (!pawnPath.Found)
					{
						return;
					}
					IntVec3 position;
					thing = pawnPath.FirstBlockingBuilding(out position, this.pawn);
				}
				if (thing != null)
				{
					IntVec3 position = thing.Position;
					if (position.InHorDistOf(this.pawn.Position, 6f))
					{
						this.job.targetA = thing;
						this.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
						this.job.expiryInterval = Rand.Range(2000, 4000);
						return;
					}
				}
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000FA4B3 File Offset: 0x000F86B3
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04001A14 RID: 6676
		private int numMeleeAttacksMade;
	}
}
