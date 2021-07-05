using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000645 RID: 1605
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x06002D9F RID: 11679 RVA: 0x001108F4 File Offset: 0x0010EAF4
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00110918 File Offset: 0x0010EB18
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			IntVec3 result;
			if (RCellFinder.TryFindBestExitSpot(pawn, out result, TraverseMode.ByPawn))
			{
				return result;
			}
			return pawn.Position;
		}
	}
}
