using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7C RID: 2940
	public class JobDriver_Spectate : JobDriver
	{
		// Token: 0x06004527 RID: 17703 RVA: 0x0002DA01 File Offset: 0x0002BC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00032EE7 File Offset: 0x000310E7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.job.GetTarget(TargetIndex.A).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
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

		// Token: 0x04002EC5 RID: 11973
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		// Token: 0x04002EC6 RID: 11974
		private const TargetIndex WatchTargetInd = TargetIndex.B;
	}
}
