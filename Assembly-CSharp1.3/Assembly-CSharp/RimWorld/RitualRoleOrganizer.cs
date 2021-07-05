using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F92 RID: 3986
	public class RitualRoleOrganizer : RitualRole
	{
		// Token: 0x06005E66 RID: 24166 RVA: 0x002062BA File Offset: 0x002044BA
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			reason = null;
			if (ritual == null)
			{
				return assignments != null && assignments.Required(p);
			}
			return p == ritual.Organizer;
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}
	}
}
