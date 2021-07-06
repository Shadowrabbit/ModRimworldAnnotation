using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000988 RID: 2440
	public class JobDriver_CastVerbOnceStatic : JobDriver_CastVerbOnce
	{
		// Token: 0x06003BAE RID: 15278 RVA: 0x0002DA98 File Offset: 0x0002BC98
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.StopDead();
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
			yield break;
		}
	}
}
