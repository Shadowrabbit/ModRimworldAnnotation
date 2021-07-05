using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200062F RID: 1583
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06002D57 RID: 11607 RVA: 0x0010FC18 File Offset: 0x0010DE18
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
