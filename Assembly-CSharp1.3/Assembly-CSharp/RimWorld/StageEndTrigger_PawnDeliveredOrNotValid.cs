using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0B RID: 3851
	public class StageEndTrigger_PawnDeliveredOrNotValid : StageEndTrigger
	{
		// Token: 0x06005BD5 RID: 23509 RVA: 0x001FBD88 File Offset: 0x001F9F88
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (TargetInfo targetInfo in foci)
				{
					Pawn pawn = targetInfo.Thing as Pawn;
					IntVec3 c = (spot.Thing != null) ? spot.Thing.OccupiedRect().CenterCell : spot.Cell;
					if (!pawn.CanReachImmediate(c, PathEndMode.Touch) && !pawn.Dead)
					{
						return false;
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x06005BD6 RID: 23510 RVA: 0x0000313F File Offset: 0x0000133F
		public override void ExposeData()
		{
		}
	}
}
