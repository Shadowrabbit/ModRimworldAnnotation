using System;

namespace Verse.AI
{
	// Token: 0x02000983 RID: 2435
	public class JobDriver_CastJump : JobDriver_CastVerbOnceStatic
	{
		// Token: 0x06003B95 RID: 15253 RVA: 0x0002D627 File Offset: 0x0002B827
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}
	}
}
