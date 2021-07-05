using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD2 RID: 4050
	public class RoleEffect_NoRangedWeapons : RoleEffect
	{
		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06005F78 RID: 24440 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x0020AB86 File Offset: 0x00208D86
		public RoleEffect_NoRangedWeapons()
		{
			this.labelKey = "RoleEffectWontUseRangedWeapons";
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x0020AB99 File Offset: 0x00208D99
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsRangedWeapon;
		}
	}
}
