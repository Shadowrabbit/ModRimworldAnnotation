using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120D RID: 4621
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06006EFB RID: 28411 RVA: 0x002519D3 File Offset: 0x0024FBD3
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x06006EFC RID: 28412 RVA: 0x002519E8 File Offset: 0x0024FBE8
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			SkillDef skill = this.Skill;
			int level = user.skills.GetSkill(skill).Level;
			user.skills.Learn(skill, 50000f, true);
			int level2 = user.skills.GetSkill(skill).Level;
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("SkillNeurotrainerUsed".Translate(user.LabelShort, skill.LabelCap, level, level2, user.Named("USER")), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06006EFD RID: 28413 RVA: 0x00251A90 File Offset: 0x0024FC90
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (p.skills == null)
			{
				failReason = null;
				return false;
			}
			if (p.skills.GetSkill(this.Skill).TotallyDisabled)
			{
				failReason = "SkillDisabled".Translate();
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		// Token: 0x04003D62 RID: 15714
		private const float XPGainAmount = 50000f;
	}
}
