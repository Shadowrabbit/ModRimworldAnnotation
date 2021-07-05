using System;

namespace Verse.AI
{
	// Token: 0x020005FC RID: 1532
	public class PathFinderCostTuning
	{
		// Token: 0x04001AAF RID: 6831
		private const int Cost_BlockedWallBase = 70;

		// Token: 0x04001AB0 RID: 6832
		private const float Cost_BlockedWallExtraPerHitPoint = 0.2f;

		// Token: 0x04001AB1 RID: 6833
		private const int Cost_BlockedDoor = 50;

		// Token: 0x04001AB2 RID: 6834
		private const float Cost_BlockedDoorPerHitPoint = 0.2f;

		// Token: 0x04001AB3 RID: 6835
		private const int Cost_OffLordWalkGrid = 70;

		// Token: 0x04001AB4 RID: 6836
		public int costBlockedWallBase = 70;

		// Token: 0x04001AB5 RID: 6837
		public float costBlockedWallExtraPerHitPoint = 0.2f;

		// Token: 0x04001AB6 RID: 6838
		public int costBlockedDoor = 50;

		// Token: 0x04001AB7 RID: 6839
		public float costBlockedDoorPerHitPoint = 0.2f;

		// Token: 0x04001AB8 RID: 6840
		public int costOffLordWalkGrid = 70;

		// Token: 0x04001AB9 RID: 6841
		public PathFinderCostTuning.ICustomizer custom;

		// Token: 0x04001ABA RID: 6842
		public static readonly PathFinderCostTuning DefaultTuning = new PathFinderCostTuning();

		// Token: 0x02001DAD RID: 7597
		public interface ICustomizer
		{
			// Token: 0x0600AB60 RID: 43872
			int CostOffset(IntVec3 from, IntVec3 to);
		}
	}
}
