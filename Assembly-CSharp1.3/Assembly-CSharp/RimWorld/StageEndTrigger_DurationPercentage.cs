using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0A RID: 3850
	public class StageEndTrigger_DurationPercentage : StageEndTrigger
	{
		// Token: 0x06005BD2 RID: 23506 RVA: 0x001FBD4E File Offset: 0x001F9F4E
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TicksPassedRitual(Mathf.RoundToInt((float)ritual.DurationTicks * this.percentage), stage);
		}

		// Token: 0x06005BD3 RID: 23507 RVA: 0x001FBD6A File Offset: 0x001F9F6A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.percentage, "percentage", 0f, false);
		}

		// Token: 0x0400357E RID: 13694
		public float percentage;
	}
}
