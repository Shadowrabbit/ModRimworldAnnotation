using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F91 RID: 3985
	public class RitualRolePrisonerOrSlave : RitualRole
	{
		// Token: 0x06005E62 RID: 24162 RVA: 0x00206220 File Offset: 0x00204420
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.IsPrisonerOfColony && !p.IsSlaveOfColony)
			{
				reason = "MessageRitualRoleMustBePrisonerOrSlave".Translate(base.LabelCap);
				return false;
			}
			if (this.mustBeCapableToFight && (p.WorkTagIsDisabled(WorkTags.Violent) || !p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)))
			{
				reason = "MessageRitualRoleMustBeCapableOfFighting".Translate(p);
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005E64 RID: 24164 RVA: 0x002062A0 File Offset: 0x002044A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.mustBeCapableToFight, "mustBeCapableToFight", false, false);
		}

		// Token: 0x04003681 RID: 13953
		public bool mustBeCapableToFight;
	}
}
