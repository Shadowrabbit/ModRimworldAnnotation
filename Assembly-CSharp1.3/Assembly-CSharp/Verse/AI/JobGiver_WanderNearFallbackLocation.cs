using System;

namespace Verse.AI
{
	// Token: 0x02000649 RID: 1609
	public class JobGiver_WanderNearFallbackLocation : JobGiver_Wander
	{
		// Token: 0x06002DA8 RID: 11688 RVA: 0x00110A21 File Offset: 0x0010EC21
		public JobGiver_WanderNearFallbackLocation()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x00110A63 File Offset: 0x0010EC63
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.mindState.duty.focusSecond.Cell, pawn);
		}
	}
}
