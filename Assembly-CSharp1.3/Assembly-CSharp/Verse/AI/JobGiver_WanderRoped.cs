using System;

namespace Verse.AI
{
	// Token: 0x0200064D RID: 1613
	public class JobGiver_WanderRoped : JobGiver_Wander
	{
		// Token: 0x06002DB2 RID: 11698 RVA: 0x00110B5B File Offset: 0x0010ED5B
		public JobGiver_WanderRoped()
		{
			this.wanderRadius = 6f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00110B80 File Offset: 0x0010ED80
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.roping.RopedTo.Cell;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x00110BA0 File Offset: 0x0010EDA0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.roping.IsRoped || pawn.roping.IsRopedByPawn)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}
	}
}
