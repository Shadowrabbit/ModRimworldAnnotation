using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C73 RID: 3187
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x06004AA8 RID: 19112 RVA: 0x0003574B File Offset: 0x0003394B
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x001A2678 File Offset: 0x001A0878
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
