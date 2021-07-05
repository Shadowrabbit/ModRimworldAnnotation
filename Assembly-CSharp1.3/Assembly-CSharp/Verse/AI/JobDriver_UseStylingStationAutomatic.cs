using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200059F RID: 1439
	public class JobDriver_UseStylingStationAutomatic : JobDriver
	{
		// Token: 0x060029F1 RID: 10737 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000FD112 File Offset: 0x000FB312
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			this.FailOn(() => !this.pawn.style.LookChangeDesired);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_StyleChange.SetupLookChangeData();
			yield return Toils_StyleChange.DoLookChange(TargetIndex.A, this.pawn);
			yield return Toils_StyleChange.FinalizeLookChange();
			yield break;
		}
	}
}
