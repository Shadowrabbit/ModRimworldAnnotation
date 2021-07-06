using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC1 RID: 3009
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x060046B8 RID: 18104 RVA: 0x00196014 File Offset: 0x00194214
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x000339AB File Offset: 0x00031BAB
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
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield break;
		}

		// Token: 0x04002F7E RID: 12158
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x04002F7F RID: 12159
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
