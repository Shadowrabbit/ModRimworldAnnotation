using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017FD RID: 6141
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x060087DF RID: 34783 RVA: 0x0005B24F File Offset: 0x0005944F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
			Scribe_Defs.Look<AbilityDef>(ref this.ability, "ability");
		}

		// Token: 0x060087E0 RID: 34784 RVA: 0x0027CC50 File Offset: 0x0027AE50
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			CompProperties_Neurotrainer compProperties_Neurotrainer = (CompProperties_Neurotrainer)props;
			this.ability = compProperties_Neurotrainer.ability;
			this.skill = compProperties_Neurotrainer.skill;
		}

		// Token: 0x060087E1 RID: 34785 RVA: 0x0005B277 File Offset: 0x00059477
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			return string.Format(base.Props.useLabel, (this.skill != null) ? this.skill.skillLabel : this.ability.label);
		}

		// Token: 0x060087E2 RID: 34786 RVA: 0x0027CC84 File Offset: 0x0027AE84
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompNeurotrainer compNeurotrainer = other.TryGetComp<CompNeurotrainer>();
			return compNeurotrainer != null && compNeurotrainer.skill == this.skill && compNeurotrainer.ability == this.ability;
		}

		// Token: 0x060087E3 RID: 34787 RVA: 0x0027CCC8 File Offset: 0x0027AEC8
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompNeurotrainer compNeurotrainer = piece.TryGetComp<CompNeurotrainer>();
			if (compNeurotrainer != null)
			{
				compNeurotrainer.skill = this.skill;
				compNeurotrainer.ability = this.ability;
			}
		}

		// Token: 0x04005724 RID: 22308
		public SkillDef skill;

		// Token: 0x04005725 RID: 22309
		public AbilityDef ability;
	}
}
