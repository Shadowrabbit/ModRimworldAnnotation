using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200095D RID: 2397
	public class JobDriver_GotoMindControlled : JobDriver_Goto
	{
		// Token: 0x06003AAE RID: 15022 RVA: 0x0002D2AF File Offset: 0x0002B4AF
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
