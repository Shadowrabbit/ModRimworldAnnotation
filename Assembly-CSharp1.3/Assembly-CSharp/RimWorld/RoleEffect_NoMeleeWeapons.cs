using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FD1 RID: 4049
	public class RoleEffect_NoMeleeWeapons : RoleEffect
	{
		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06005F75 RID: 24437 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsBad
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x0020AB63 File Offset: 0x00208D63
		public RoleEffect_NoMeleeWeapons()
		{
			this.labelKey = "RoleEffectWontUseMeleeWeapons";
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x0020AB76 File Offset: 0x00208D76
		public override bool CanEquip(Pawn pawn, Thing thing)
		{
			return !thing.def.IsMeleeWeapon;
		}
	}
}
