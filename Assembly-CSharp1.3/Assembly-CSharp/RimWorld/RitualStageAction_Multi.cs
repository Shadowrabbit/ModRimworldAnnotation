using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA0 RID: 4000
	public class RitualStageAction_Multi : RitualStageAction
	{
		// Token: 0x06005E93 RID: 24211 RVA: 0x00206A44 File Offset: 0x00204C44
		public override void Apply(LordJob_Ritual ritual)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ritual);
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.Apply(ritual);
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00206AB0 File Offset: 0x00204CB0
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			if (this.subActions == null)
			{
				Log.Error("RitualStageAction_Multi without any sub actions on ritual " + ritual);
				return;
			}
			foreach (RitualStageAction ritualStageAction in this.subActions)
			{
				ritualStageAction.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00206B1C File Offset: 0x00204D1C
		public override void ExposeData()
		{
			Scribe_Collections.Look<RitualStageAction>(ref this.subActions, "subActions", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04003689 RID: 13961
		public List<RitualStageAction> subActions;
	}
}
