using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C70 RID: 3184
	public class JobGiver_HiveDefense : JobGiver_AIFightEnemies
	{
		// Token: 0x06004A9E RID: 19102 RVA: 0x001A2480 File Offset: 0x001A0680
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive != null && hive.Spawned)
			{
				return hive.Position;
			}
			return pawn.Position;
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x000356F5 File Offset: 0x000338F5
		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x00035707 File Offset: 0x00033907
		protected override Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = base.MeleeAttackJob(enemyTarget);
			job.attackDoorIfTargetLost = true;
			return job;
		}
	}
}
