using System;

namespace Verse.AI
{
	// Token: 0x0200063F RID: 1599
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06002D90 RID: 11664 RVA: 0x00110542 File Offset: 0x0010E742
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0011056E File Offset: 0x0010E76E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
