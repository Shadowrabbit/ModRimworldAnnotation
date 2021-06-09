using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200097B RID: 2427
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x06003B6A RID: 15210 RVA: 0x0002D8A1 File Offset: 0x0002BAA1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0002D8CD File Offset: 0x0002BACD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			Toil init = new Toil();
			init.initAction = delegate()
			{
				Pawn pawn = this.TargetThingA as Pawn;
				if (pawn != null)
				{
					this.startedIncapacitated = pawn.Downed;
				}
				this.pawn.pather.StopDead();
			};
			init.tickAction = delegate()
			{
				if (!this.TargetA.IsValid)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (this.TargetA.HasThing)
				{
					Pawn pawn = this.TargetA.Thing as Pawn;
					if (this.TargetA.Thing.Destroyed || (pawn != null && !this.startedIncapacitated && pawn.Downed) || (pawn != null && pawn.IsInvisible()))
					{
						this.EndJobWith(JobCondition.Succeeded);
						return;
					}
				}
				if (this.numAttacksMade >= this.job.maxNumStaticAttacks && !this.pawn.stances.FullBodyBusy)
				{
					this.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (this.pawn.TryStartAttack(this.TargetA))
				{
					this.numAttacksMade++;
					return;
				}
				if (!this.pawn.stances.FullBodyBusy)
				{
					Verb verb = this.pawn.TryGetAttackVerb(this.TargetA.Thing, !this.pawn.IsColonist);
					if (this.job.endIfCantShootTargetFromCurPos && (verb == null || !verb.CanHitTargetFrom(this.pawn.Position, this.TargetA)))
					{
						this.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (this.job.endIfCantShootInMelee)
					{
						if (verb == null)
						{
							this.EndJobWith(JobCondition.Incompletable);
							return;
						}
						float num = verb.verbProps.EffectiveMinRange(this.TargetA, this.pawn);
						if ((float)this.pawn.Position.DistanceToSquared(this.TargetA.Cell) < num * num && this.pawn.Position.AdjacentTo8WayOrInside(this.TargetA.Cell))
						{
							this.EndJobWith(JobCondition.Incompletable);
							return;
						}
					}
				}
			};
			init.defaultCompleteMode = ToilCompleteMode.Never;
			init.activeSkill = (() => Toils_Combat.GetActiveSkillForToil(init));
			yield return init;
			yield break;
		}

		// Token: 0x04002938 RID: 10552
		private bool startedIncapacitated;

		// Token: 0x04002939 RID: 10553
		private int numAttacksMade;
	}
}
