using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000585 RID: 1413
	public class JobDriver_GotoMindControlled : JobDriver_Goto
	{
		// Token: 0x0600296D RID: 10605 RVA: 0x000FA70B File Offset: 0x000F890B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return toil;
			if (this.job.def.waitAfterArriving > 0)
			{
				yield return Toils_General.Wait(this.job.def.waitAfterArriving, TargetIndex.None);
			}
			yield break;
		}
	}
}
