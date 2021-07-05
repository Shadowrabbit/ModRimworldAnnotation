using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD7 RID: 4055
	public class RoleEffect_HuntingRevengeChanceFactor : RoleEffect
	{
		// Token: 0x06005F82 RID: 24450 RVA: 0x0020AC56 File Offset: 0x00208E56
		public RoleEffect_HuntingRevengeChanceFactor()
		{
			this.labelKey = "RoleEffectHuntingRevengeChance";
		}

		// Token: 0x06005F83 RID: 24451 RVA: 0x0020AC69 File Offset: 0x00208E69
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate("x" + this.factor.ToStringPercent());
		}

		// Token: 0x040036E2 RID: 14050
		public float factor;
	}
}
