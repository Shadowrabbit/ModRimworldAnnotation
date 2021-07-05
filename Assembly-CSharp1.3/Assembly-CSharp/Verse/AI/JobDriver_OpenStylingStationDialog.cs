using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200059D RID: 1437
	public class JobDriver_OpenStylingStationDialog : JobDriver
	{
		// Token: 0x060029EA RID: 10730 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000FD0CB File Offset: 0x000FB2CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Styling station"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				Find.WindowStack.Add(new Dialog_StylingStation(this.pawn, this.job.targetA.Thing));
			});
			yield break;
		}
	}
}
