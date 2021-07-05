using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F12 RID: 3858
	public class StageEndTrigger_PawnDead : StageEndTrigger
	{
		// Token: 0x06005BEA RID: 23530 RVA: 0x001FC0E0 File Offset: 0x001FA2E0
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
			return new Trigger_TickCondition(() => pawn.Dead, 1);
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x001FC157 File Offset: 0x001FA357
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x04003587 RID: 13703
		[NoTranslate]
		public string roleId;
	}
}
