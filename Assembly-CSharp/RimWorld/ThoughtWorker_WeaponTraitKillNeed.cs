using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDE RID: 3806
	public class ThoughtWorker_WeaponTraitKillNeed : ThoughtWorker_WeaponTrait
	{
		// Token: 0x06005436 RID: 21558 RVA: 0x001C2FB4 File Offset: 0x001C11B4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			CompBladelinkWeapon compBladelinkWeapon = p.equipment.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
			List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (traitsListForReading[i] == WeaponTraitDefOf.NeedKill)
				{
					return compBladelinkWeapon.TicksSinceLastKill > 1200000;
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x04003525 RID: 13605
		public const int TicksToGiveMemory = 1200000;
	}
}
