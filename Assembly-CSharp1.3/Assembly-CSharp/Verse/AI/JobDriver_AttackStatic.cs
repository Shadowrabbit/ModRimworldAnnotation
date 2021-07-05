using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000597 RID: 1431
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x060029D6 RID: 10710 RVA: 0x000FCF25 File Offset: 0x000FB125
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000FCF51 File Offset: 0x000FB151
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

		// Token: 0x04001A15 RID: 6677
		private bool startedIncapacitated;

		// Token: 0x04001A16 RID: 6678
		private int numAttacksMade;
	}
}
