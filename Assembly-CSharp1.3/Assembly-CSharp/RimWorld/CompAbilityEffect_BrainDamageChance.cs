using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D23 RID: 3363
	public class CompAbilityEffect_BrainDamageChance : CompAbilityEffect
	{
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x001A6D5E File Offset: 0x001A4F5E
		public new CompProperties_AbilityBrainDamageChance Props
		{
			get
			{
				return (CompProperties_AbilityBrainDamageChance)this.props;
			}
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x001A6D6C File Offset: 0x001A4F6C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn;
			if ((pawn = (target.Thing as Pawn)) != null)
			{
				if (pawn.Dead)
				{
					return;
				}
				if (Rand.Value <= this.Props.brainDamageChance)
				{
					BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
					if (brain == null)
					{
						return;
					}
					int num = Rand.RangeInclusive(1, 5);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.parent.pawn, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				}
			}
		}
	}
}
