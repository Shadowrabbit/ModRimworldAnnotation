using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200058F RID: 1423
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x060029A6 RID: 10662 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000FC208 File Offset: 0x000FA408
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
					if (this.pawn.Position.InHorDistOf(pawn.Position, 4f) && this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), RegionType.Set_Passable))
					{
						return;
					}
					if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
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

		// Token: 0x060029A8 RID: 10664 RVA: 0x000FA4B3 File Offset: 0x000F86B3
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04001A0F RID: 6671
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x04001A10 RID: 6672
		private const int Distance = 4;
	}
}
