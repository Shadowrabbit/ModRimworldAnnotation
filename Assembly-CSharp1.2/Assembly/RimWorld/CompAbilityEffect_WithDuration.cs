using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A6 RID: 5030
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06006D1A RID: 27930 RVA: 0x0004A349 File Offset: 0x00048549
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x002173C0 File Offset: 0x002155C0
		public float GetDurationSeconds(Pawn target)
		{
			float num = this.parent.def.statBases.GetStatValueFromList(StatDefOf.Ability_Duration, 10f);
			if (this.Props.durationMultiplier != null)
			{
				num *= target.GetStatValue(this.Props.durationMultiplier, true);
			}
			return num;
		}
	}
}
