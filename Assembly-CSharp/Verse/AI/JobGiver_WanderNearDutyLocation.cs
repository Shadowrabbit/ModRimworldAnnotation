using System;

namespace Verse.AI
{
	// Token: 0x02000AA8 RID: 2728
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06004095 RID: 16533 RVA: 0x000304BF File Offset: 0x0002E6BF
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x000304E4 File Offset: 0x0002E6E4
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
