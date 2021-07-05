using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072D RID: 1837
	public class JobDriver_Refuel : JobDriver
	{
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x00123FF0 File Offset: 0x001221F0
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060032F8 RID: 13048 RVA: 0x00124011 File Offset: 0x00122211
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x00124020 File Offset: 0x00122220
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x00124044 File Offset: 0x00122244
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x00124095 File Offset: 0x00122295
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

		// Token: 0x04001DE6 RID: 7654
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04001DE7 RID: 7655
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04001DE8 RID: 7656
		public const int RefuelingDuration = 240;
	}
}
