using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D76 RID: 3446
	public class CompAbilityEffect_TrainRandomSkill : CompAbilityEffect
	{
		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x001AB9AA File Offset: 0x001A9BAA
		public new CompProperties_AbilityTrainRandomSkill Props
		{
			get
			{
				return (CompProperties_AbilityTrainRandomSkill)this.props;
			}
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x001AB9B8 File Offset: 0x001A9BB8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (target.Pawn != null)
			{
				SkillDef skillDef = (from x in DefDatabase<SkillDef>.AllDefs
				where !target.Pawn.skills.GetSkill(x).TotallyDisabled
				select x).RandomElement<SkillDef>();
				int level = target.Pawn.skills.GetSkill(skillDef).Level;
				target.Pawn.skills.Learn(skillDef, 50000f, true);
				if (base.SendLetter)
				{
					int value = target.Pawn.skills.GetSkill(skillDef).Level - level;
					Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(skillDef.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, skillDef, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null);
				}
			}
		}

		// Token: 0x04002FCE RID: 12238
		private const float XPGainAmount = 50000f;
	}
}
