using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F1 RID: 1777
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x06003180 RID: 12672 RVA: 0x001201C8 File Offset: 0x0011E3C8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.GetTarget(TargetIndex.A).Cell, this.job, errorOnFailed);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001201FB File Offset: 0x0011E3FB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.GetTarget(TargetIndex.C).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.C, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				tickAction = delegate()
				{
					this.pawn.rotationTracker.FaceCell(this.job.GetTarget(TargetIndex.B).Cell);
					this.pawn.GainComfortFromCellIfPossible(false);
					if (this.pawn.IsHashIntervalTick(100))
					{
						this.pawn.jobs.CheckForJobOverride();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x04001D84 RID: 7556
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x04001D85 RID: 7557
		private const TargetIndex WatchTargetInd = TargetIndex.B;

		// Token: 0x04001D86 RID: 7558
		private const TargetIndex ChairInd = TargetIndex.C;
	}
}
