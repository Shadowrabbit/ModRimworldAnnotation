using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D33 RID: 3379
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x06004D44 RID: 19780 RVA: 0x001ADB08 File Offset: 0x001ABD08
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x00036B3C File Offset: 0x00034D3C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.GetColonyWanderRoot(pawn);
		}
	}
}
