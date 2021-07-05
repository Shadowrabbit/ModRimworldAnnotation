using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F90 RID: 3984
	public class RitualRolePrisoner : RitualRole
	{
		// Token: 0x06005E5F RID: 24159 RVA: 0x002061EC File Offset: 0x002043EC
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (p.IsPrisonerOfColony)
			{
				reason = null;
				return true;
			}
			reason = "MessageRitualRoleMustBePrisoner".Translate(base.LabelCap);
			return false;
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}
	}
}
