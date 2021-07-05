using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C0 RID: 1728
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x0011D130 File Offset: 0x0011B330
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x0011D156 File Offset: 0x0011B356
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
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
				if (this.Female.IsHashIntervalTick(100))
				{
					FleckMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			};
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}

		// Token: 0x04001D33 RID: 7475
		private const int MateDuration = 500;

		// Token: 0x04001D34 RID: 7476
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x04001D35 RID: 7477
		private const int TicksBetweenHeartMotes = 100;
	}
}
