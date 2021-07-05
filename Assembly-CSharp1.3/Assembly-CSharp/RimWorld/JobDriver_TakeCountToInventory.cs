using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000736 RID: 1846
	public class JobDriver_TakeCountToInventory : JobDriver
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x001247C4 File Offset: 0x001229C4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 10, this.job.count, null, true);
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x001247F2 File Offset: 0x001229F2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, this.job.count);
			yield break;
		}
	}
}
