using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072E RID: 1838
	public class JobDriver_RefuelAtomic : JobDriver
	{
		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x00124110 File Offset: 0x00122310
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x00124131 File Offset: 0x00122331
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x00124140 File Offset: 0x00122340
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x00124164 File Offset: 0x00122364
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x001241B1 File Offset: 0x001223B1
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

		// Token: 0x04001DE9 RID: 7657
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04001DEA RID: 7658
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04001DEB RID: 7659
		private const TargetIndex FuelPlaceCellInd = TargetIndex.C;

		// Token: 0x04001DEC RID: 7660
		private const int RefuelingDuration = 240;
	}
}
