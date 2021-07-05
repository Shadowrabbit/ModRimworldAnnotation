using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000582 RID: 1410
	public class JobDriver_ActivateArchonexusCore : JobDriver_Goto
	{
		// Token: 0x0600295D RID: 10589 RVA: 0x000FA56A File Offset: 0x000F876A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Activate archonexus core"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				((Building_ArchonexusCore)this.job.targetA.Thing).Activate();
			};
			toil.handlingFacing = true;
			toil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 120;
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return toil;
			yield break;
		}

		// Token: 0x040019A3 RID: 6563
		private const int DefaultDuration = 120;
	}
}
