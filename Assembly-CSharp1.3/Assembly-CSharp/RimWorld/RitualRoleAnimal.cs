using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F94 RID: 3988
	public class RitualRoleAnimal : RitualRole
	{
		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool Animal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005E6D RID: 24173 RVA: 0x002062FC File Offset: 0x002044FC
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null)
		{
			if (!p.RaceProps.Animal)
			{
				reason = "MessageRitualRoleMustBeAnimal".Translate(base.LabelCap);
				return false;
			}
			if (p.BodySize < this.minBodySize)
			{
				reason = "MessageRitualRoleMustHaveLargerBodySize".Translate(base.LabelCap, this.minBodySize.ToString("0.00"));
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

		// Token: 0x06005E6E RID: 24174 RVA: 0x00206218 File Offset: 0x00204418
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null)
		{
			reason = null;
			return false;
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x002063A4 File Offset: 0x002045A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minBodySize, "minBodySize", 0f, false);
		}

		// Token: 0x04003682 RID: 13954
		private float minBodySize;
	}
}
