using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000979 RID: 2425
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x06003B5A RID: 15194 RVA: 0x0002D84D File Offset: 0x0002BA4D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0016DA38 File Offset: 0x0016BC38
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0002D867 File Offset: 0x0002BA67
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
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
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

		// Token: 0x06003B5D RID: 15197 RVA: 0x0016DA84 File Offset: 0x0016BC84
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPath(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
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

		// Token: 0x06003B5E RID: 15198 RVA: 0x0002D573 File Offset: 0x0002B773
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04002933 RID: 10547
		private int numMeleeAttacksMade;
	}
}
