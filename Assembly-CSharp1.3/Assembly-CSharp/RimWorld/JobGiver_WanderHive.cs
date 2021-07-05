using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000762 RID: 1890
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x0600344E RID: 13390 RVA: 0x0012888B File Offset: 0x00126A8B
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x001288B0 File Offset: 0x00126AB0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive == null || !hive.Spawned)
			{
				return pawn.Position;
			}
			return hive.Position;
		}
	}
}
