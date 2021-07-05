using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class JobDriver_Maintain : JobDriver
	{
		// Token: 0x060032D1 RID: 13009 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x00123AEB File Offset: 0x00121CEB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(180, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			Toil maintain = new Toil();
			maintain.initAction = delegate()
			{
				maintain.actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>().Maintained();
			};
			maintain.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return maintain;
			yield break;
		}

		// Token: 0x04001DD8 RID: 7640
		private const int MaintainTicks = 180;
	}
}
