using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F09 RID: 3849
	public class StageEndTrigger_Instant : StageEndTrigger
	{
		// Token: 0x06005BD0 RID: 23504 RVA: 0x001FBD20 File Offset: 0x001F9F20
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => true);
		}
	}
}
