using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005A6 RID: 1446
	public class JobDriver_HaulToContainer : JobDriver
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002A15 RID: 10773 RVA: 0x000FDB76 File Offset: 0x000FBD76
		public Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x000FDB89 File Offset: 0x000FBD89
		public Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002A17 RID: 10775 RVA: 0x000FDB9C File Offset: 0x000FBD9C
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

		// Token: 0x06002A18 RID: 10776 RVA: 0x000FDBCC File Offset: 0x000FBDCC
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

		// Token: 0x06002A19 RID: 10777 RVA: 0x000FDCA8 File Offset: 0x000FBEA8
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

		// Token: 0x06002A1A RID: 10778 RVA: 0x000FDD40 File Offset: 0x000FBF40
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(() => CompBiosculpterPod.WasLoadingCanceled(this.Container));
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
			Toil uninstallIfMinifiable = Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil startCarryingThing = Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			Toil jumpIfAlsoCollectingNextTarget = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return Toils_Jump.JumpIf(jumpIfAlsoCollectingNextTarget, () => this.pawn.IsCarryingThing(this.ThingToCarry));
			yield return getToHaulTarget;
			yield return uninstallIfMinifiable;
			yield return startCarryingThing;
			yield return jumpIfAlsoCollectingNextTarget;
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil toil = Toils_General.Wait(this.Duration, TargetIndex.B);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			Thing destThing = this.job.GetTarget(TargetIndex.B).Thing;
			toil.tickAction = delegate()
			{
				if (this.pawn.IsHashIntervalTick(80) && destThing is Building_Grave && this.graveDigEffect == null)
				{
					this.graveDigEffect = EffecterDefOf.BuryPawn.Spawn();
					this.graveDigEffect.Trigger(destThing, destThing);
				}
				Effecter effecter = this.graveDigEffect;
				if (effecter == null)
				{
					return;
				}
				effecter.EffectTick(destThing, destThing);
			};
			yield return toil;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C, null);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		// Token: 0x04001A23 RID: 6691
		private Effecter graveDigEffect;

		// Token: 0x04001A24 RID: 6692
		protected const TargetIndex CarryThingIndex = TargetIndex.A;

		// Token: 0x04001A25 RID: 6693
		public const TargetIndex DestIndex = TargetIndex.B;

		// Token: 0x04001A26 RID: 6694
		protected const TargetIndex PrimaryDestIndex = TargetIndex.C;

		// Token: 0x04001A27 RID: 6695
		protected const int DiggingEffectInterval = 80;
	}
}
