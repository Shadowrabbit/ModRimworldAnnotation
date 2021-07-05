using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006EC RID: 1772
	public class JobDriver_GotoAndStandSociallyActive : JobDriver
	{
		// Token: 0x0600315D RID: 12637 RVA: 0x0011FC7C File Offset: 0x0011DE7C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, true);
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0011FCA8 File Offset: 0x0011DEA8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
					{
						this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return this.StandToil;
			yield break;
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x0011FCB8 File Offset: 0x0011DEB8
		public virtual Toil StandToil
		{
			get
			{
				return new Toil
				{
					tickAction = delegate()
					{
						Pawn pawn = JobDriver_StandAndBeSociallyActive.FindClosePawn(this.pawn);
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
						this.pawn.GainComfortFromCellIfPossible(false);
					},
					socialMode = RandomSocialMode.SuperActive,
					defaultCompleteMode = ToilCompleteMode.Never,
					handlingFacing = true
				};
			}
		}
	}
}
