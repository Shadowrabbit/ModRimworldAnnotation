using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF3 RID: 3059
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060047F0 RID: 18416 RVA: 0x0018EA98 File Offset: 0x0018CC98
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060047F1 RID: 18417 RVA: 0x000343D5 File Offset: 0x000325D5
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060047F2 RID: 18418 RVA: 0x00190280 File Offset: 0x0018E480
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x00199210 File Offset: 0x00197410
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x000343E2 File Offset: 0x000325E2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (!this.RefuelableComp.IsFull)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			base.AddFailCondition(() => (!this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct) || !this.RefuelableComp.allowAutoRefuel);
			base.AddFailCondition(() => !this.RefuelableComp.allowAutoRefuel && !this.job.playerForced);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
			yield return findPlaceTarget;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false, false);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			findPlaceTarget = null;
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.None);
			yield break;
		}

		// Token: 0x04003014 RID: 12308
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04003015 RID: 12309
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04003016 RID: 12310
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x04003017 RID: 12311
		private const int RefuelingDuration = 240;
	}
}
