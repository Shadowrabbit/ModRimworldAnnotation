using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000FA6 RID: 4006
	public abstract class RitualStageTickActionMaker
	{
		// Token: 0x06005EA7 RID: 24231
		public abstract IEnumerable<ActionOnTick> GenerateTimedActions(LordJob_Ritual ritual, RitualStage stage);
	}
}
