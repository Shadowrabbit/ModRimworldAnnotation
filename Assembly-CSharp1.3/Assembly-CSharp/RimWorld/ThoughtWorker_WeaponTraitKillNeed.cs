using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public class ThoughtWorker_WeaponTraitKillNeed : ThoughtWorker_WeaponTrait
	{
		// Token: 0x06003E48 RID: 15944 RVA: 0x00154DF4 File Offset: 0x00152FF4
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

		// Token: 0x040020E9 RID: 8425
		public const int TicksToGiveMemory = 1200000;
	}
}
