using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDC RID: 4060
	public class RoleRequirement_MinSkillAny : RoleRequirement
	{
		// Token: 0x06005F92 RID: 24466 RVA: 0x0020AE0C File Offset: 0x0020900C
		public override string GetLabel(Precept_Role role)
		{
			if (this.labelCached == null)
			{
				if (this.skills.Count == 1)
				{
					this.labelCached = "RoleRequirementSkill".Translate() + ": " + RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(this.skills[0]);
				}
				else
				{
					this.labelCached = "RoleRequirementSkillAny".Translate() + ": " + (from s in this.skills
					select RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(s)).ToCommaList(false, false);
				}
			}
			return this.labelCached;
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x0020AEC4 File Offset: 0x002090C4
		public override bool Met(Pawn p, Precept_Role role)
		{
			foreach (SkillRequirement skillRequirement in this.skills)
			{
				if (p.skills.GetSkill(skillRequirement.skill).Level >= skillRequirement.minLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x0020AF38 File Offset: 0x00209138
		[CompilerGenerated]
		internal static string <GetLabel>g__GetSkillStr|2_0(SkillRequirement requirement)
		{
			return requirement.skill.LabelCap + " " + requirement.minLevel;
		}

		// Token: 0x040036E5 RID: 14053
		public List<SkillRequirement> skills;

		// Token: 0x040036E6 RID: 14054
		[NoTranslate]
		private string labelCached;
	}
}
