using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06003E08 RID: 15880 RVA: 0x001540A5 File Offset: 0x001522A5
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
