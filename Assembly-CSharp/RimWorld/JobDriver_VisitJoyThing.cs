using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA3 RID: 2979
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x060045F9 RID: 17913 RVA: 0x0002DA01 File Offset: 0x0002BC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x00033477 File Offset: 0x00031677
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(this.job.def.joyDuration, TargetIndex.None);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate()
			{
				this.WaitTickAction();
			};
			toil.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return toil;
			yield break;
		}

		// Token: 0x060045FB RID: 17915
		protected abstract void WaitTickAction();

		// Token: 0x04002F24 RID: 12068
		protected const TargetIndex TargetThingIndex = TargetIndex.A;
	}
}
