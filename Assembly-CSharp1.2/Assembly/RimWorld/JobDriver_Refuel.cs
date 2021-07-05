using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF1 RID: 3057
	public class JobDriver_Refuel : JobDriver
	{
		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060047DE RID: 18398 RVA: 0x0018EA98 File Offset: 0x0018CC98
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060047DF RID: 18399 RVA: 0x00034326 File Offset: 0x00032526
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060047E0 RID: 18400 RVA: 0x00190280 File Offset: 0x0018E480
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00198FAC File Offset: 0x001971AC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x00034333 File Offset: 0x00032533
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
			base.AddFailCondition(() => !this.job.playerForced && !this.RefuelableComp.ShouldAutoRefuelNowIgnoringFuelPct);
			base.AddFailCondition(() => !this.RefuelableComp.allowAutoRefuel && !this.job.playerForced);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.RefuelableComp.GetFuelCountToFullyRefuel();
			});
			Toil reserveFuel = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return reserveFuel;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveFuel, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
			yield break;
		}

		// Token: 0x0400300C RID: 12300
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x0400300D RID: 12301
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x0400300E RID: 12302
		private const int RefuelingDuration = 240;
	}
}
