using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000592 RID: 1426
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x060029BA RID: 10682 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000FC78C File Offset: 0x000FA98C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return toil;
			if (this.job.takeInventoryDelay > 0)
			{
				Toil toil2 = Toils_General.Wait(this.job.takeInventoryDelay, TargetIndex.None);
				toil2.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
				toil2.tickAction = delegate()
				{
					this.pawn.rotationTracker.FaceTarget(base.TargetThingA);
				};
				toil2.handlingFacing = true;
				yield return toil2;
			}
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, this.job.count);
			yield break;
		}
	}
}
