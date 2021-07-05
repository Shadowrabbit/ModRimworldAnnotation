using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000703 RID: 1795
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x060031DE RID: 12766 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x001215D6 File Offset: 0x0011F7D6
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

		// Token: 0x060031E0 RID: 12768
		protected abstract void WaitTickAction();

		// Token: 0x04001D9D RID: 7581
		protected const TargetIndex TargetThingIndex = TargetIndex.A;
	}
}
