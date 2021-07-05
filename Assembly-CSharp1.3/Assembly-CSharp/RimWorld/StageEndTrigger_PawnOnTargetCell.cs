using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F11 RID: 3857
	public class StageEndTrigger_PawnOnTargetCell : StageEndTrigger
	{
		// Token: 0x06005BE7 RID: 23527 RVA: 0x001FC018 File Offset: 0x001FA218
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			if (ritual.Ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == this.roleId) == null)
			{
				return null;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn(this.roleId);
			if (pawn == null)
			{
				return null;
			}
			PawnStagePosition dest = ritual.PawnPositionForStage(pawn, stage);
			return new Trigger_TicksPassedAfterConditionMet(this.waitTicks, () => pawn.CanReachImmediate(dest.cell, PathEndMode.ClosestTouch) || pawn.Dead, 1);
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x001FC0A9 File Offset: 0x001FA2A9
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Values.Look<int>(ref this.waitTicks, "waitTicks", 0, false);
		}

		// Token: 0x04003585 RID: 13701
		public string roleId;

		// Token: 0x04003586 RID: 13702
		public int waitTicks = 50;
	}
}
