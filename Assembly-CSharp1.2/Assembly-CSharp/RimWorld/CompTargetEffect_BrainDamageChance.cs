using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C5 RID: 6341
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x06008CAE RID: 36014 RVA: 0x0005E4AE File Offset: 0x0005C6AE
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x06008CAF RID: 36015 RVA: 0x0028D7B4 File Offset: 0x0028B9B4
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (Rand.Value <= this.PropsBrainDamageChance.brainDamageChance)
			{
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int num = Rand.RangeInclusive(1, 5);
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}
