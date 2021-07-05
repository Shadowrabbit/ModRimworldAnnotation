using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000586 RID: 1414
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x0600296F RID: 10607 RVA: 0x000FA71C File Offset: 0x000F891C
		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x000FA742 File Offset: 0x000F8942
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x000FA764 File Offset: 0x000F8964
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn(() => ((Pawn)((Thing)this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.interactionMode != PrisonerInteractionModeDefOf.Release);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false, false);
			Toil setReleased = new Toil();
			setReleased.initAction = delegate()
			{
				Pawn pawn = setReleased.actor.jobs.curJob.targetA.Thing as Pawn;
				GenGuest.PrisonerRelease(pawn);
				pawn.guest.ClearLastRecruiterData();
				if (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Map.Tile))
				{
					GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "Released", pawn.Named("SUBJECT"));
			};
			yield return setReleased;
			yield break;
		}

		// Token: 0x040019A9 RID: 6569
		private const TargetIndex PrisonerInd = TargetIndex.A;

		// Token: 0x040019AA RID: 6570
		private const TargetIndex ReleaseCellInd = TargetIndex.B;
	}
}
