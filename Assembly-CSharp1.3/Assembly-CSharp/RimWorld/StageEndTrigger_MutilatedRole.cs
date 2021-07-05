using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0D RID: 3853
	public class StageEndTrigger_MutilatedRole : StageEndTrigger
	{
		// Token: 0x06005BDB RID: 23515 RVA: 0x001FBE00 File Offset: 0x001FA000
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			LordJob_Ritual_Mutilation r;
			if ((r = (ritual as LordJob_Ritual_Mutilation)) != null)
			{
				return new Trigger_Custom(delegate(TriggerSignal signal)
				{
					foreach (Pawn p in r.mutilatedPawns)
					{
						RitualRole ritualRole = r.RoleFor(p, true);
						if (ritualRole != null && ritualRole.id == this.roleId)
						{
							return true;
						}
					}
					return false;
				});
			}
			Log.Error("Used StageEndTrigger_MutilatedRole on non LordJob_Ritual_Mutilation ritual job");
			return null;
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x001FBE48 File Offset: 0x001FA048
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
		}

		// Token: 0x04003582 RID: 13698
		[NoTranslate]
		public string roleId;
	}
}
