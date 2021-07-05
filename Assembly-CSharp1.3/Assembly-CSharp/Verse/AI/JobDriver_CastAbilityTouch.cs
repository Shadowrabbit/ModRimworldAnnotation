using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000599 RID: 1433
	public class JobDriver_CastAbilityTouch : JobDriver_CastVerbOnce
	{
		// Token: 0x060029E0 RID: 10720 RVA: 0x000FD06E File Offset: 0x000FB26E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}
	}
}
