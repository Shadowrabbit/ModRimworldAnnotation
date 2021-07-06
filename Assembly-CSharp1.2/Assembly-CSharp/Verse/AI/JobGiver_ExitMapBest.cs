using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A8E RID: 2702
	public class JobGiver_ExitMapBest : JobGiver_ExitMap
	{
		// Token: 0x06004039 RID: 16441 RVA: 0x001824BC File Offset: 0x001806BC
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindBestExitSpot(pawn, out spot, mode);
		}
	}
}
