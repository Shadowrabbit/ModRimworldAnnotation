using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F2E RID: 3886
	public class RitualCancellationTrigger_CannotScarify : RitualCancellationTrigger
	{
		// Token: 0x06005C67 RID: 23655 RVA: 0x001FDE44 File Offset: 0x001FC044
		public override IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments)
		{
			yield return new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && GenTicks.TicksGame % 60 == 0 && !JobDriver_Scarify.AvailableOnNow(assignments.FirstAssignedPawn(this.roleId), null));
			yield break;
		}

		// Token: 0x040035C3 RID: 13763
		[NoTranslate]
		public string roleId;
	}
}
