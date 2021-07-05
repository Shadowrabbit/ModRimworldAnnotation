using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F93 RID: 3987
	public class RitualRoleForced : RitualRole
	{
		// Token: 0x06005E69 RID: 24169 RVA: 0x002062DA File Offset: 0x002044DA
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			reason = null;
			return ritual != null && ritual.assignments.ForcedRole(p) == this.id;
		}

		// Token: 0x06005E6A RID: 24170 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}
	}
}
