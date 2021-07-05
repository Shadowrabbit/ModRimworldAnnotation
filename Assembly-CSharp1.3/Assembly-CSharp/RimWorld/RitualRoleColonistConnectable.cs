using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F99 RID: 3993
	public class RitualRoleColonistConnectable : RitualRoleColonist
	{
		// Token: 0x06005E80 RID: 24192 RVA: 0x002067C4 File Offset: 0x002049C4
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!base.AppliesToPawn(p, out reason, ritual, assignments, precept))
			{
				return false;
			}
			if (((precept != null) ? precept.ideo : null) != null && precept.ritualOnlyForIdeoMembers && p.Ideo != precept.ideo)
			{
				reason = "CantStartRitualSelectedPawnMustBeMember".Translate(p.Named("PAWN"), precept.ideo.Named("IDEO"));
				return false;
			}
			Pawn_NeedsTracker needs = p.needs;
			bool flag;
			if (needs == null)
			{
				flag = (null != null);
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					flag = (null != null);
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					flag = (((thoughts != null) ? thoughts.memories : null) != null);
				}
			}
			return !flag || p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.ConnectedTreeDied) == null;
		}
	}
}
