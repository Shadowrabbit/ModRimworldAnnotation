using System;

namespace Verse.AI
{
	// Token: 0x0200059B RID: 1435
	public class JobDriver_CastJump : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x060029E5 RID: 10725 RVA: 0x000FC6D2 File Offset: 0x000FA8D2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}
	}
}
