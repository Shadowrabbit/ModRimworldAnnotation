using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DC RID: 6364
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06008CFC RID: 36092 RVA: 0x0005E81A File Offset: 0x0005CA1A
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x06008CFD RID: 36093 RVA: 0x0028E6B4 File Offset: 0x0028C8B4
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

		// Token: 0x06008CFE RID: 36094 RVA: 0x0028E75C File Offset: 0x0028C95C
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

		// Token: 0x04005A09 RID: 23049
		private const float XPGainAmount = 50000f;
	}
}
