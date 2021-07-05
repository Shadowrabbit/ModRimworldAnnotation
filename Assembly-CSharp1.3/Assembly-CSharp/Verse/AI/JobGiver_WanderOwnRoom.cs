using System;

namespace Verse.AI
{
	// Token: 0x02000647 RID: 1607
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x06002DA4 RID: 11684 RVA: 0x00110998 File Offset: 0x0010EB98
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x001109F8 File Offset: 0x0010EBF8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			MentalState_WanderOwnRoom mentalState_WanderOwnRoom = pawn.MentalState as MentalState_WanderOwnRoom;
			if (mentalState_WanderOwnRoom != null)
			{
				return mentalState_WanderOwnRoom.target;
			}
			return pawn.Position;
		}
	}
}
