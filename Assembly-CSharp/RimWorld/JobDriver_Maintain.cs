using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BDA RID: 3034
	public class JobDriver_Maintain : JobDriver
	{
		// Token: 0x0600476C RID: 18284 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x00034046 File Offset: 0x00032246
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

		// Token: 0x04002FCE RID: 12238
		private const int MaintainTicks = 180;
	}
}
