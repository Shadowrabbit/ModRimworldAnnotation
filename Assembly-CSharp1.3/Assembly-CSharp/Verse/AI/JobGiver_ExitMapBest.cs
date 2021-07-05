using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000630 RID: 1584
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06002D59 RID: 11609 RVA: 0x0010FC40 File Offset: 0x0010DE40
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
