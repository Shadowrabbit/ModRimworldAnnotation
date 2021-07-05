using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000714 RID: 1812
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x06003248 RID: 12872 RVA: 0x001224B4 File Offset: 0x001206B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x001224F7 File Offset: 0x001206F7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					if (this.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				}
			};
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			toil.AddPreTickAction(delegate
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.ExitMap();
				}
			});
			yield return toil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
					{
						this.ExitMap();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x00122508 File Offset: 0x00120708
		private void ExitMap()
		{
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}

		// Token: 0x04001DBD RID: 7613
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x04001DBE RID: 7614
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
