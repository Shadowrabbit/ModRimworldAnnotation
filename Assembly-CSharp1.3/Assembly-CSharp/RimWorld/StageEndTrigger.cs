using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F08 RID: 3848
	public abstract class StageEndTrigger : IExposable
	{
		// Token: 0x06005BCD RID: 23501
		public abstract Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage);

		// Token: 0x06005BCE RID: 23502 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}
	}
}
