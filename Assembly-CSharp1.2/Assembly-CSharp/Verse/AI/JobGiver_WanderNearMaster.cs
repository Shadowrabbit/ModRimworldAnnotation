using System;

namespace Verse.AI
{
	// Token: 0x02000AAA RID: 2730
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06004099 RID: 16537 RVA: 0x0003051E File Offset: 0x0002E71E
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn p, IntVec3 c, IntVec3 root) => !this.MustUseRootRoom(p) || root.GetRoom(p.Map, RegionType.Set_Passable) == null || WanderRoomUtility.IsValidWanderDest(p, c, root));
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x00030555 File Offset: 0x0002E755
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0003056D File Offset: 0x0002E76D
		private bool MustUseRootRoom(Pawn pawn)
		{
			return !pawn.playerSettings.Master.playerSettings.animalsReleased;
		}
	}
}
