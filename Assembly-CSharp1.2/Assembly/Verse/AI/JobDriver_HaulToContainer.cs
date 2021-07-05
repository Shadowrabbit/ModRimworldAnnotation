using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000997 RID: 2455
	public class JobDriver_HaulToContainer : JobDriver
	{
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003BF9 RID: 15353 RVA: 0x0002DC75 File Offset: 0x0002BE75
		public Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003BFA RID: 15354 RVA: 0x0002DC88 File Offset: 0x0002BE88
		public Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003BFB RID: 15355 RVA: 0x0002DC9B File Offset: 0x0002BE9B
		private int Duration
		{
			get
			{
				if (this.Container == null || !(this.Container is Building))
				{
					return 0;
				}
				return this.Container.def.building.haulToContainerDuration;
			}
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00170128 File Offset: 0x0016E328
		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			if (thing == null || !this.job.targetB.HasThing)
			{
				return "ReportHaulingUnknown".Translate();
			}
			return ((this.job.GetTarget(TargetIndex.B).Thing is Building_Grave) ? "ReportHaulingToGrave" : "ReportHaulingTo").Translate(thing.Label, this.job.targetB.Thing.LabelShort.Named("DESTINATION"), thing.Named("THING"));
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00170204 File Offset: 0x0016E404
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0002DCC9 File Offset: 0x0002BEC9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(delegate()
			{
				ThingOwner thingOwner = this.Container.TryGetInnerInteractableThingOwner();
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(this.ThingToCarry, true))
				{
					return true;
				}
				IHaulDestination haulDestination = this.Container as IHaulDestination;
				return haulDestination != null && !haulDestination.Accepts(this.ThingToCarry);
			});
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil toil = Toils_General.Wait(this.Duration, TargetIndex.B);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return toil;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		// Token: 0x04002985 RID: 10629
		protected const TargetIndex CarryThingIndex = TargetIndex.A;

		// Token: 0x04002986 RID: 10630
		public const TargetIndex DestIndex = TargetIndex.B;

		// Token: 0x04002987 RID: 10631
		protected const TargetIndex PrimaryDestIndex = TargetIndex.C;
	}
}
