using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AA5 RID: 2725
	public class JobGiver_WanderMapEdge : JobGiver_Wander
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x00030491 File Offset: 0x0002E691
		public JobGiver_WanderMapEdge()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(50, 125);
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x00182F8C File Offset: 0x0018118C
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
