using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D1 RID: 1745
	public class JobDriver_Unrope : JobDriver
	{
		// Token: 0x060030A3 RID: 12451 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x0011E24C File Offset: 0x0011C44C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Rope.GotoRopeAttachmentInteractionCell(TargetIndex.A);
			yield return Toils_Rope.UnropeFromSpot(TargetIndex.A);
			yield break;
		}

		// Token: 0x04001D52 RID: 7506
		private const TargetIndex AnimalInd = TargetIndex.A;
	}
}
