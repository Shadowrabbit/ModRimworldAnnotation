using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200059E RID: 1438
	public class JobDriver_UseStylingStation : JobDriver
	{
		// Token: 0x060029EE RID: 10734 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000FD102 File Offset: 0x000FB302
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_StyleChange.DoLookChange(TargetIndex.A, this.pawn);
			yield return Toils_StyleChange.FinalizeLookChange();
			yield break;
		}
	}
}
