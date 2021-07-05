using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D81 RID: 3457
	public abstract class CompAbilityEffect_WithDuration : CompAbilityEffect
	{
		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x001AC688 File Offset: 0x001AA888
		public new CompProperties_AbilityEffectWithDuration Props
		{
			get
			{
				return (CompProperties_AbilityEffectWithDuration)this.props;
			}
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x001AC698 File Offset: 0x001AA898
		public float GetDurationSeconds(Pawn target)
		{
			float num = this.parent.def.GetStatValueAbstract(StatDefOf.Ability_Duration, this.parent.pawn);
			if (this.Props.durationMultiplier != null)
			{
				num *= target.GetStatValue(this.Props.durationMultiplier, true);
			}
			return num;
		}
	}
}
