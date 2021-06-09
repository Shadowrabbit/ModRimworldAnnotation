using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A8D RID: 2701
	public class JobGiver_ExitMapRandom : JobGiver_ExitMap
	{
		// Token: 0x06004037 RID: 16439 RVA: 0x0018249C File Offset: 0x0018069C
		protected override bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 spot)
		{
			TraverseMode mode = canDig ? TraverseMode.PassAllDestroyableThings : TraverseMode.ByPawn;
			return RCellFinder.TryFindRandomExitSpot(pawn, out spot, mode);
		}
	}
}
