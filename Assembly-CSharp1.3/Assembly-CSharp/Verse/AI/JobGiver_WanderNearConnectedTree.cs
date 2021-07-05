using System;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x0200064E RID: 1614
	public class JobGiver_WanderNearConnectedTree : JobGiver_Wander
	{
		// Token: 0x06002DB5 RID: 11701 RVA: 0x00110BC5 File Offset: 0x0010EDC5
		public JobGiver_WanderNearConnectedTree()
		{
			this.wanderRadius = 12f;
			this.ticksBetweenWandersRange = new IntRange(130, 250);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x00110BF0 File Offset: 0x0010EDF0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.connections.ConnectedThings.FirstOrDefault((Thing x) => x.Spawned && x.Map == pawn.Map && pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)) == null)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x00110C3B File Offset: 0x0010EE3B
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return pawn.connections.ConnectedThings[0].Position;
		}
	}
}
