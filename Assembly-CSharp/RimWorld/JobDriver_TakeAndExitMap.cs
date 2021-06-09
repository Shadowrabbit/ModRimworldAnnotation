using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C04 RID: 3076
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004861 RID: 18529 RVA: 0x0018EA98 File Offset: 0x0018CC98
		protected Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x00034725 File Offset: 0x00032925
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x00034747 File Offset: 0x00032947
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

		// Token: 0x0400304E RID: 12366
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x0400304F RID: 12367
		private const TargetIndex ExitCellInd = TargetIndex.B;
	}
}
