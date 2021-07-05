using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x06003444 RID: 13380 RVA: 0x0012863C File Offset: 0x0012683C
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x0012867C File Offset: 0x0012687C
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x0012868E File Offset: 0x0012688E
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
