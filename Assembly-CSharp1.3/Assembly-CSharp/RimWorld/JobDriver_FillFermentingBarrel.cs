using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x001223C8 File Offset: 0x001205C8
		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x001223F0 File Offset: 0x001205F0
		protected Thing Wort
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x00122414 File Offset: 0x00120614
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Wort, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x00122465 File Offset: 0x00120665
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

		// Token: 0x04001DBA RID: 7610
		private const TargetIndex BarrelInd = TargetIndex.A;

		// Token: 0x04001DBB RID: 7611
		private const TargetIndex WortInd = TargetIndex.B;

		// Token: 0x04001DBC RID: 7612
		private const int Duration = 200;
	}
}
