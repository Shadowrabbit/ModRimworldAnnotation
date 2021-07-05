using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F98 RID: 3992
	public class RitualRoleColonist : RitualRole
	{
		// Token: 0x06005E7C RID: 24188 RVA: 0x00206700 File Offset: 0x00204900
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.RaceProps.Humanlike)
			{
				reason = "MessageRitualRoleMustBeHumanlike".Translate(base.Label);
				return false;
			}
			if (this.requiredWorkType != null && p.WorkTypeIsDisabled(this.requiredWorkType))
			{
				reason = "MessageRitualRoleMustBeCapableOfGeneric".Translate(base.LabelCap, this.requiredWorkType.gerundLabel);
				return false;
			}
			if (!p.Faction.IsPlayerSafe())
			{
				reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06005E7D RID: 24189 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005E7E RID: 24190 RVA: 0x002067AB File Offset: 0x002049AB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WorkTypeDef>(ref this.requiredWorkType, "requiredWorkType");
		}

		// Token: 0x04003683 RID: 13955
		public WorkTypeDef requiredWorkType;
	}
}
