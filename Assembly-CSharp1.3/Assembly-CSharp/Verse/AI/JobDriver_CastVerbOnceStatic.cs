using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005A1 RID: 1441
	public class JobDriver_CastVerbOnceStatic : JobDriver_CastVerbOnce
	{
		// Token: 0x060029F9 RID: 10745 RVA: 0x000FD1AD File Offset: 0x000FB3AD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.StopDead();
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
