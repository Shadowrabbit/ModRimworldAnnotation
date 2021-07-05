using System;

namespace Verse.AI
{
	// Token: 0x02000A9F RID: 2719
	public class JobGiver_WanderAnywhere : JobGiver_Wander
	{
		// Token: 0x06004082 RID: 16514 RVA: 0x00030422 File Offset: 0x0002E622
		public JobGiver_WanderAnywhere()
		{
			this.wanderRadius = 7f;
			this.locomotionUrgency = LocomotionUrgency.Walk;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0003044E File Offset: 0x0002E64E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
