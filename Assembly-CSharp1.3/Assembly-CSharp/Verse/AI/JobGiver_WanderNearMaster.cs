using System;

namespace Verse.AI
{
	// Token: 0x0200064A RID: 1610
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06002DAA RID: 11690 RVA: 0x00110A80 File Offset: 0x0010EC80
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x00110AB7 File Offset: 0x0010ECB7
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x00110ACF File Offset: 0x0010ECCF
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
