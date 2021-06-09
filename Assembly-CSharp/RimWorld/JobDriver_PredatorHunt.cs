using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B2D RID: 2861
	public class JobDriver_PredatorHunt : JobDriver
	{
		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06004327 RID: 17191 RVA: 0x0018D368 File Offset: 0x0018B568
		public Pawn Prey
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06004328 RID: 17192 RVA: 0x0018D3A0 File Offset: 0x0018B5A0
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x00031D92 File Offset: 0x0002FF92
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.firstHit, "firstHit", false, false);
			Scribe_Values.Look<bool>(ref this.notifiedPlayerAttacking, "notifiedPlayerAttacking", false, false);
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00031DBE File Offset: 0x0002FFBE
		public override string GetReport()
		{
			if (this.Corpse != null)
			{
				return this.ReportStringProcessed(JobDefOf.Ingest.reportString);
			}
			return base.GetReport();
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00031DDF File Offset: 0x0002FFDF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddFinishAction(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Toil prepareToEatCorpse = new Toil();
			prepareToEatCorpse.initAction = delegate()
			{
				Pawn actor = prepareToEatCorpse.actor;
				Corpse corpse = this.Corpse;
				if (corpse == null)
				{
					Pawn prey = this.Prey;
					if (prey == null)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					corpse = prey.Corpse;
					if (corpse == null || !corpse.Spawned)
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
				}
				if (actor.Faction == Faction.OfPlayer)
				{
					corpse.SetForbidden(false, false);
				}
				else
				{
					corpse.SetForbidden(true, false);
				}
				actor.CurJob.SetTarget(TargetIndex.A, corpse);
			};
			yield return Toils_General.DoAtomic(delegate
			{
				this.Map.attackTargetsCache.UpdateTarget(this.pawn);
			});
			Action hitAction = delegate()
			{
				Pawn prey = this.Prey;
				bool surpriseAttack = this.firstHit && !prey.IsColonist;
				if (this.pawn.meleeVerbs.TryMeleeAttack(prey, this.job.verbToUse, surpriseAttack))
				{
					if (!this.notifiedPlayerAttacked && PawnUtility.ShouldSendNotificationAbout(prey))
					{
						this.notifiedPlayerAttacked = true;
						Messages.Message("MessageAttackedByPredator".Translate(prey.LabelShort, this.pawn.LabelIndefinite(), prey.Named("PREY"), this.pawn.Named("PREDATOR")).CapitalizeFirst(), prey, MessageTypeDefOf.ThreatSmall, true);
					}
					this.Map.attackTargetsCache.UpdateTarget(this.pawn);
					this.firstHit = false;
				}
			};
			Toil toil = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, hitAction).JumpIfDespawnedOrNull(TargetIndex.A, prepareToEatCorpse).JumpIf(() => this.Corpse != null, prepareToEatCorpse).FailOn(() => Find.TickManager.TicksGame > this.startTick + 5000 && (float)(this.job.GetTarget(TargetIndex.A).Cell - this.pawn.Position).LengthHorizontalSquared > 4f);
			toil.AddPreTickAction(new Action(this.CheckWarnPlayer));
			yield return toil;
			yield return prepareToEatCorpse;
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return gotoCorpse;
			float durationMultiplier = 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true);
			yield return Toils_Ingest.ChewIngestible(this.pawn, durationMultiplier, TargetIndex.A, TargetIndex.None).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.A);
			yield return Toils_Jump.JumpIf(gotoCorpse, () => this.pawn.needs.food.CurLevelPercentage < 0.9f);
			yield break;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x0018D3C8 File Offset: 0x0018B5C8
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && dinfo.Def.isRanged && dinfo.Instigator != null && dinfo.Instigator != this.Prey && !this.pawn.InMentalState && !this.pawn.Downed)
			{
				this.pawn.mindState.StartFleeingBecauseOfPawnAction(dinfo.Instigator);
			}
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x0018D448 File Offset: 0x0018B648
		private void CheckWarnPlayer()
		{
			if (this.notifiedPlayerAttacking)
			{
				return;
			}
			Pawn prey = this.Prey;
			if (!prey.Spawned || prey.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame <= this.pawn.mindState.lastPredatorHuntingPlayerNotificationTick + 2500)
			{
				return;
			}
			if (!prey.Position.InHorDistOf(this.pawn.Position, 60f))
			{
				return;
			}
			if (prey.RaceProps.Humanlike)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPredatorHuntingColonist".Translate(this.pawn.LabelShort, prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), "LetterPredatorHuntingColonist".Translate(this.pawn.LabelIndefinite(), prey.LabelDefinite(), this.pawn.Named("PREDATOR"), prey.Named("PREY")).CapitalizeFirst(), LetterDefOf.ThreatBig, this.pawn, null, null, null, null);
			}
			else
			{
				Messages.Message((prey.Name.Numerical ? "LetterPredatorHuntingColonist" : "MessagePredatorHuntingPlayerAnimal").Translate(this.pawn.Named("PREDATOR"), prey.Named("PREY")), this.pawn, MessageTypeDefOf.ThreatBig, true);
			}
			this.pawn.mindState.Notify_PredatorHuntingPlayerNotification();
			this.notifiedPlayerAttacking = true;
		}

		// Token: 0x04002DE8 RID: 11752
		private bool notifiedPlayerAttacked;

		// Token: 0x04002DE9 RID: 11753
		private bool notifiedPlayerAttacking;

		// Token: 0x04002DEA RID: 11754
		private bool firstHit = true;

		// Token: 0x04002DEB RID: 11755
		public const TargetIndex PreyInd = TargetIndex.A;

		// Token: 0x04002DEC RID: 11756
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04002DED RID: 11757
		private const int MaxHuntTicks = 5000;
	}
}
