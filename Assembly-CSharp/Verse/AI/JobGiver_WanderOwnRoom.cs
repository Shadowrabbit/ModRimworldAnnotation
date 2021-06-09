using System;

namespace Verse.AI
{
	// Token: 0x02000AA6 RID: 2726
	public class JobGiver_WanderOwnRoom : JobGiver_Wander
	{
		// Token: 0x06004090 RID: 16528 RVA: 0x00182FAC File Offset: 0x001811AC
		public JobGiver_WanderOwnRoom()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(300, 600);
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => WanderRoomUtility.IsValidWanderDest(pawn, loc, root));
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0018300C File Offset: 0x0018120C
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
