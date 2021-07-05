using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000631 RID: 1585
	public class JobGiver_ExitMapNearDutyTarget : JobGiver_ExitMap
	{
		// Token: 0x06002D5B RID: 11611 RVA: 0x0010FC60 File Offset: 0x0010DE60
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			IntVec3 near = pawn.DutyLocation();
			float num = pawn.mindState.duty.radius;
			if (num <= 0f)
			{
				num = 12f;
			}
			return RCellFinder.TryFindExitSpotNear(pawn, near, num, out spot, mode);
		}
	}
}
