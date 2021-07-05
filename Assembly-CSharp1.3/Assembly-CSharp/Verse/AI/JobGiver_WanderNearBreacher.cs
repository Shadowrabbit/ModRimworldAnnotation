using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200064B RID: 1611
	public class JobGiver_WanderNearBreacher : JobGiver_Wander
	{
		// Token: 0x06002DAE RID: 11694 RVA: 0x00110A21 File Offset: 0x0010EC21
		public JobGiver_WanderNearBreacher()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00110B10 File Offset: 0x0010ED10
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Pawn pawn2 = BreachingUtility.FindPawnToEscort(pawn);
			if (pawn2 == null)
			{
				return IntVec3.Invalid;
			}
			return pawn2.Position;
		}
	}
}
