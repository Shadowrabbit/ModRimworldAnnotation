using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDD RID: 3805
	public class ThoughtWorker_WeaponTraitBonded : ThoughtWorker_WeaponTrait
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x001C2F48 File Offset: 0x001C1148
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
