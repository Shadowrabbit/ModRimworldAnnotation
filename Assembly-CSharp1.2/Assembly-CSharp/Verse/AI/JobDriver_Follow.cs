using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200096C RID: 2412
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x06003B07 RID: 15111 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0002D563 File Offset: 0x0002B763
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.Position.InHorDistOf(pawn.Position, 4f) && this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
					{
						return;
					}
					if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
					{
						this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0002D573 File Offset: 0x0002B773
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04002918 RID: 10520
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x04002919 RID: 10521
		private const int Distance = 4;
	}
}
