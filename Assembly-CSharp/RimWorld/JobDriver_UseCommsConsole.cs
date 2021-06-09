using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C11 RID: 3089
	public class JobDriver_UseCommsConsole : JobDriver
	{
		// Token: 0x060048C1 RID: 18625 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00034A4C File Offset: 0x00032C4C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn((Toil to) => !((Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).CanUseCommsNow);
			Toil openComms = new Toil();
			openComms.initAction = delegate()
			{
				Pawn actor = openComms.actor;
				if (((Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).CanUseCommsNow)
				{
					actor.jobs.curJob.commTarget.TryOpenComms(actor);
				}
			};
			yield return openComms;
			yield break;
		}
	}
}
