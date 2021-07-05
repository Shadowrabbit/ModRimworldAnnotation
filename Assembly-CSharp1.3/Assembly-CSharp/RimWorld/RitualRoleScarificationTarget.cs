using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F95 RID: 3989
	public class RitualRoleScarificationTarget : RitualRole
	{
		// Token: 0x06005E71 RID: 24177 RVA: 0x002063C4 File Offset: 0x002045C4
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.RaceProps.Humanlike)
			{
				reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				return false;
			}
			if (p.Ideo == null || p.Ideo.RequiredScars <= p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification))
			{
				reason = "MessageRitualRoleMustRequireScarification".Translate(p);
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

		// Token: 0x06005E72 RID: 24178 RVA: 0x0020646E File Offset: 0x0020466E
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return true;
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x00206474 File Offset: 0x00204674
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null)
			{
				if (pawn.Ideo == null || pawn.Ideo.RequiredScars <= 0)
				{
					return "RitualScarificationWarnNonScarificationIdeo".Translate(pawn.Named("PAWN"));
				}
				if (pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
				{
					return "RitualScarificationWarnWimp".Translate(pawn.Named("PAWN"));
				}
			}
			return null;
		}
	}
}
