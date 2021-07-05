using System;

namespace Verse.AI
{
	// Token: 0x02000640 RID: 1600
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06002D92 RID: 11666 RVA: 0x00110578 File Offset: 0x0010E778
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0011056E File Offset: 0x0010E76E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
