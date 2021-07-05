using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F96 RID: 3990
	public class RitualRoleWarden : RitualRole
	{
		// Token: 0x06005E75 RID: 24181 RVA: 0x002064F8 File Offset: 0x002046F8
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.Faction.IsPlayerSafe())
			{
				reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				return false;
			}
			if (!p.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				reason = "MessageRitualRoleMustBeCapableOfWardening".Translate(p);
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}
	}
}
