using System;

namespace Verse.AI
{
	// Token: 0x02000AA0 RID: 2720
	public class JobGiver_WanderCurrentRoom : JobGiver_Wander
	{
		// Token: 0x06004084 RID: 16516 RVA: 0x00182D9C File Offset: 0x00180F9C
		public JobGiver_WanderCurrentRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x0003044E File Offset: 0x0002E64E
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
