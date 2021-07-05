using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073B RID: 1851
	public class JobDriver_UseCommsConsole : JobDriver
	{
		// Token: 0x0600335A RID: 13146 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x00124FFD File Offset: 0x001231FD
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
