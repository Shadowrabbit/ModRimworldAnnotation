using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007FF RID: 2047
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x0600369D RID: 13981 RVA: 0x0013584C File Offset: 0x00133A4C
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x001358A1 File Offset: 0x00133AA1
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.GetColonyWanderRoot(pawn);
		}
	}
}
