using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A8F RID: 2703
	public class JobGiver_ExitMapNearDutyTarget : JobGiver_ExitMap
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x001824DC File Offset: 0x001806DC
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
