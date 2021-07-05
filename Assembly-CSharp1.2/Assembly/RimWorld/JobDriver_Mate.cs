using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B29 RID: 2857
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x0600430D RID: 17165 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00031D0B File Offset: 0x0002FF0B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 500, false, false);
			toil.tickAction = delegate()
			{
				if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
				if (this.Female.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			};
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}

		// Token: 0x04002DDC RID: 11740
		private const int MateDuration = 500;

		// Token: 0x04002DDD RID: 11741
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x04002DDE RID: 11742
		private const int TicksBetweenHeartMotes = 100;
	}
}
