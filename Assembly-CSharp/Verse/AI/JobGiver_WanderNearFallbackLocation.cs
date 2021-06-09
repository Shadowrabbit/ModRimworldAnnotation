using System;

namespace Verse.AI
{
	// Token: 0x02000AA9 RID: 2729
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06004097 RID: 16535 RVA: 0x000304BF File Offset: 0x0002E6BF
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x00030501 File Offset: 0x0002E701
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
