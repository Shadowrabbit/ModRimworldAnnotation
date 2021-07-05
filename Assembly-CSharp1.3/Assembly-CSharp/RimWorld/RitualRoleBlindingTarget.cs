using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F97 RID: 3991
	public class RitualRoleBlindingTarget : RitualRole
	{
		// Token: 0x06005E78 RID: 24184 RVA: 0x0020658C File Offset: 0x0020478C
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.RaceProps.Humanlike)
			{
				reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				return false;
			}
			if (p.health != null)
			{
				if (p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord part) => part.def == BodyPartDefOf.Eye))
				{
					if (!p.Faction.IsPlayerSafe())
					{
						reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
						return false;
					}
					reason = null;
					return true;
				}
			}
			reason = "MessageRitualRoleMustHaveEyes".Translate(base.LabelCap);
			return false;
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x00206654 File Offset: 0x00204854
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null)
			{
				if (pawn.Ideo != null)
				{
					if (pawn.Ideo.PreceptsListForReading.Any((Precept p) => p.def.issue == IssueDefOf.Blindness))
					{
						if (pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
						{
							return "RitualBlindingWarnWimp".Translate(pawn.Named("PAWN"));
						}
						goto IL_9B;
					}
				}
				return "RitualBlindingWarnNonBlindingIdeo".Translate(pawn.Named("PAWN"));
			}
			IL_9B:
			return null;
		}
	}
}
