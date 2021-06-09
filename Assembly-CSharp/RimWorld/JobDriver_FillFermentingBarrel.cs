using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBF RID: 3007
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x00195D88 File Offset: 0x00193F88
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00190280 File Offset: 0x0018E480
		protected Thing Wort
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x00195DB0 File Offset: 0x00193FB0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Wort, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x00033933 File Offset: 0x00031B33
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (this.Barrel.SpaceLeftForWort > 0)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = this.Barrel.SpaceLeftForWort;
			});
			Toil reserveWort = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return reserveWort;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveWort, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Barrel.AddWort(this.Wort);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04002F76 RID: 12150
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04002F77 RID: 12151
		private const TargetIndex WortInd = TargetIndex.B;

		// Token: 0x04002F78 RID: 12152
		private const int Duration = 200;
	}
}
