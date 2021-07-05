using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D0 RID: 2512
	public class ThoughtWorker_WeaponTraitBonded : ThoughtWorker_WeaponTrait
	{
		// Token: 0x06003E46 RID: 15942 RVA: 0x00154D80 File Offset: 0x00152F80
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			List<WeaponTraitDef> traitsListForReading = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>().TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i].bondedThought == this.def)
				{
					return true;
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
