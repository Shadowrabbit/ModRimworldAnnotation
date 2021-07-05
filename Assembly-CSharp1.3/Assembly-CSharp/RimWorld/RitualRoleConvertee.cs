using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9A RID: 3994
	public class RitualRoleConvertee : RitualRole
	{
		// Token: 0x06005E82 RID: 24194 RVA: 0x00206894 File Offset: 0x00204A94
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			reason = null;
			if (p.Ideo == ((assignments != null) ? assignments.Ritual.ideo : null))
			{
				if (p.Ideo != null)
				{
					reason = "MessageRitualPawnIsAlreadyBelievingIdeo".Translate(p, Find.ActiveLanguageWorker.WithIndefiniteArticle(p.Ideo.memberName, p.gender, false, false));
				}
				return false;
			}
			return (assignments != null && assignments.Forced(p)) || p.Ideo != ((assignments != null) ? assignments.Ritual.ideo : null);
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}
	}
}
