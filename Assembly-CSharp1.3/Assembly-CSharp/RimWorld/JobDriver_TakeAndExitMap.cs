using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000734 RID: 1844
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x00124554 File Offset: 0x00122754
		protected Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00124575 File Offset: 0x00122775
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00124597 File Offset: 0x00122797
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil toil = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			toil.AddPreTickAction(delegate
			{
				if (base.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
				}
			});
			toil.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			yield return toil;
			Toil toil2 = new Toil();
			toil2.initAction = delegate()
			{
				if (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
				}
			};
			toil2.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x04001DF5 RID: 7669
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04001DF6 RID: 7670
		private const TargetIndex ExitCellInd = TargetIndex.B;
	}
}
