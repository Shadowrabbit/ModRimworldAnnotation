using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C08 RID: 3080
	public class JobDriver_TakeToBed : JobDriver
	{
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x0016B214 File Offset: 0x00169414
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x0019A7A8 File Offset: 0x001989A8
		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0019A7D0 File Offset: 0x001989D0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.DropBed, this.job, this.DropBed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x000347ED File Offset: 0x000329ED
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			this.FailOn(delegate()
			{
				if (this.job.def.makeTargetPrisoner)
				{
					if (!this.DropBed.ForPrisoners)
					{
						return true;
					}
				}
				else if (this.DropBed.ForPrisoners != this.Takee.IsPrisoner)
				{
					return true;
				}
				return false;
			});
			yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
			base.AddFinishAction(delegate
			{
				if (this.job.def.makeTargetPrisoner && this.Takee.ownership.OwnedBed == this.DropBed && this.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(this.Takee, this.DropBed))
				{
					this.Takee.ownership.UnclaimBed();
				}
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.job.def == JobDefOf.Arrest && !this.Takee.CanBeArrestedBy(this.pawn)).FailOn(() => !this.pawn.CanReach(this.DropBed, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)).FailOn(() => (this.job.def == JobDefOf.Rescue || this.job.def == JobDefOf.Capture) && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.def.makeTargetPrisoner)
					{
						Pawn pawn = (Pawn)this.job.targetA.Thing;
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnAttemptArrested(pawn);
						}
						GenClamor.DoClamor(pawn, 10f, ClamorDefOf.Harm);
						if (!pawn.IsPrisoner)
						{
							QuestUtility.SendQuestTargetSignals(pawn.questTags, "Arrested", pawn.Named("SUBJECT"));
						}
						if (this.job.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(this.pawn))
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						}
					}
				}
			};
			Toil toil = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOnNonMedicalBedNotOwned(TargetIndex.B, TargetIndex.A);
			toil.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			yield return toil;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.CheckMakeTakeePrisoner();
					if (this.Takee.playerSettings == null)
					{
						this.Takee.playerSettings = new Pawn_PlayerSettings(this.Takee);
					}
				}
			};
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return new Toil
			{
				initAction = delegate()
				{
					IntVec3 position = this.DropBed.Position;
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(position, ThingPlaceMode.Direct, out thing, null);
					if (!this.DropBed.Destroyed && (this.DropBed.OwnersForReading.Contains(this.Takee) || (this.DropBed.Medical && this.DropBed.AnyUnoccupiedSleepingSlot) || this.Takee.ownership == null))
					{
						this.Takee.jobs.Notify_TuckedIntoBed(this.DropBed);
						if (this.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !this.Takee.IsPrisonerOfColony)
						{
							this.Takee.relations.Notify_RescuedBy(this.pawn);
						}
						this.Takee.mindState.Notify_TuckedIntoBed();
					}
					if (this.Takee.IsPrisonerOfColony)
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.PrisonerTab, this.Takee, OpportunityType.GoodToKnow);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0019A82C File Offset: 0x00198A2C
		private void CheckMakeTakeePrisoner()
		{
			if (this.job.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.Released)
				{
					this.Takee.guest.Released = false;
					this.Takee.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
					GenGuest.RemoveHealthyPrisonerReleasedThoughts(this.Takee);
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					this.Takee.guest.CapturedBy(Faction.OfPlayer, this.pawn);
				}
			}
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x0019A8B8 File Offset: 0x00198AB8
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, false);
			}
		}

		// Token: 0x0400305D RID: 12381
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x0400305E RID: 12382
		private const TargetIndex BedIndex = TargetIndex.B;
	}
}
