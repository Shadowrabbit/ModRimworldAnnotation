using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000737 RID: 1847
	public class JobDriver_TakeToBed : JobDriver
	{
		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x00124804 File Offset: 0x00122A04
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x0012482C File Offset: 0x00122A2C
		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x00124852 File Offset: 0x00122A52
		private bool TakeeRescued
		{
			get
			{
				return this.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !this.Takee.IsPrisonerOfColony;
			}
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x00124888 File Offset: 0x00122A88
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.DropBed, this.job, this.DropBed.SleepingSlotsCount, 0, null, errorOnFailed);
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x001248E3 File Offset: 0x00122AE3
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
			Toil goToTakee = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn(() => this.job.def == JobDefOf.Arrest && !this.Takee.CanBeArrestedBy(this.pawn)).FailOn(() => !this.pawn.CanReach(this.DropBed, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOn(() => (this.job.def == JobDefOf.Rescue || this.job.def == JobDefOf.Capture) && !this.Takee.Downed).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil checkArrestResistance = new Toil();
			checkArrestResistance.initAction = delegate()
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
					if (!pawn.IsPrisoner && !pawn.IsSlave)
					{
						QuestUtility.SendQuestTargetSignals(pawn.questTags, "Arrested", pawn.Named("SUBJECT"));
					}
					if (this.job.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(this.pawn))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			};
			yield return Toils_Jump.JumpIf(checkArrestResistance, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return goToTakee;
			yield return checkArrestResistance;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOnNonMedicalBedNotOwned(TargetIndex.B, TargetIndex.A);
			startCarrying.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			Toil goToBed = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOn(() => !this.pawn.IsCarryingPawn(this.Takee));
			yield return Toils_Jump.JumpIf(goToBed, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return goToBed;
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
			yield return Toils_Bed.TuckIntoBed(this.DropBed, this.pawn, this.Takee, this.TakeeRescued);
			yield break;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x001248F4 File Offset: 0x00122AF4
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

		// Token: 0x0600333F RID: 13119 RVA: 0x00124980 File Offset: 0x00122B80
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
				QuestUtility.SendQuestTargetSignals(this.Takee.questTags, "Rescued", this.Takee.Named("SUBJECT"));
			}
		}

		// Token: 0x04001DFB RID: 7675
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04001DFC RID: 7676
		private const TargetIndex BedIndex = TargetIndex.B;
	}
}
