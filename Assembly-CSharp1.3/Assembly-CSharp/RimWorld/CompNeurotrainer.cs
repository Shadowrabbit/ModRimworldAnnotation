using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001168 RID: 4456
	public class CompNeurotrainer : CompUsable
	{
		// Token: 0x06006AFD RID: 27389 RVA: 0x0023E992 File Offset: 0x0023CB92
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<SkillDef>(ref this.skill, "skill");
			Scribe_Defs.Look<AbilityDef>(ref this.ability, "ability");
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x0023E9BC File Offset: 0x0023CBBC
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			CompProperties_Neurotrainer compProperties_Neurotrainer = (CompProperties_Neurotrainer)props;
			this.ability = compProperties_Neurotrainer.ability;
			this.skill = compProperties_Neurotrainer.skill;
		}

		// Token: 0x06006AFF RID: 27391 RVA: 0x0023E9EF File Offset: 0x0023CBEF
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			return string.Format(base.Props.useLabel, (this.skill != null) ? this.skill.skillLabel : this.ability.label);
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x0023EA24 File Offset: 0x0023CC24
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompNeurotrainer compNeurotrainer = other.TryGetComp<CompNeurotrainer>();
			return compNeurotrainer != null && compNeurotrainer.skill == this.skill && compNeurotrainer.ability == this.ability;
		}

		// Token: 0x06006B01 RID: 27393 RVA: 0x0023EA68 File Offset: 0x0023CC68
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

		// Token: 0x04003B7B RID: 15227
		public SkillDef skill;

		// Token: 0x04003B7C RID: 15228
		public AbilityDef ability;
	}
}
