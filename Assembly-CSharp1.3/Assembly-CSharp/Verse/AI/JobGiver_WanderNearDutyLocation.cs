using System;

namespace Verse.AI
{
	// Token: 0x02000648 RID: 1608
	public class JobGiver_WanderNearDutyLocation : JobGiver_Wander
	{
		// Token: 0x06002DA6 RID: 11686 RVA: 0x00110A21 File Offset: 0x0010EC21
		public JobGiver_WanderNearDutyLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x00110A46 File Offset: 0x0010EC46
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focus.Cell, pawn);
		}
	}
}
