using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D18 RID: 3352
	public class Ability_RandomEffect : Ability
	{
		// Token: 0x06004EB6 RID: 20150 RVA: 0x001A6002 File Offset: 0x001A4202
		public Ability_RandomEffect(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06004EB7 RID: 20151 RVA: 0x001A600B File Offset: 0x001A420B
		public Ability_RandomEffect(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001A6018 File Offset: 0x001A4218
		public override bool CanApplyOn(LocalTargetInfo target)
		{
			using (List<CompAbilityEffect>.Enumerator enumerator = base.EffectComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanApplyOn(target, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001A6078 File Offset: 0x001A4278
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			(from x in effects
			where x.Props.weight > 0f && x.CanApplyOn(target, dest)
			select x).RandomElementByWeight((CompAbilityEffect x) => x.Props.weight).Apply(target, dest);
		}
	}
}
