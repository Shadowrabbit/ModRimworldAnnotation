using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000591 RID: 1425
	public class JobDriver_Goto : JobDriver
	{
		// Token: 0x060029B6 RID: 10678 RVA: 0x000FC6D2 File Offset: 0x000FA8D2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000FC706 File Offset: 0x000FA906
		protected override IEnumerable<Toil> MakeNewToils()
		{
			LocalTargetInfo lookAtTarget = this.job.GetTarget(TargetIndex.B);
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			toil.AddPreTickAction(delegate
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.TryExitMap();
				}
			});
			toil.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			if (lookAtTarget.IsValid)
			{
				Toil toil2 = toil;
				toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
				{
					this.pawn.rotationTracker.FaceCell(lookAtTarget.Cell);
				}));
				toil.handlingFacing = true;
			}
			yield return toil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == this.TargetA.Cell)
					{
						this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
					if (!this.job.ritualTag.NullOrEmpty())
					{
						Lord lord = this.pawn.GetLord();
						LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
						if (lordJob_Ritual != null)
						{
							lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
						}
					}
					if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
					{
						this.TryExitMap();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000FC718 File Offset: 0x000FA918
		private void TryExitMap()
		{
			if (this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				return;
			}
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}
	}
}
