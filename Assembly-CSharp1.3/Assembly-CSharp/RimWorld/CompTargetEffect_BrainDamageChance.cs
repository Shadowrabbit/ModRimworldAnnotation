using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E3 RID: 4579
	public class CompTargetEffect_BrainDamageChance : CompTargetEffect
	{
		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06006E70 RID: 28272 RVA: 0x002502EE File Offset: 0x0024E4EE
		protected CompProperties_TargetEffect_BrainDamageChance PropsBrainDamageChance
		{
			get
			{
				return (CompProperties_TargetEffect_BrainDamageChance)this.props;
			}
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x002502FC File Offset: 0x0024E4FC
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
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, user, brain, this.parent.def, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}
	}
}
