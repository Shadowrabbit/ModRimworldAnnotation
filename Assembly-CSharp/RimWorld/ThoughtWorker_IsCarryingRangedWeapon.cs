using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC6 RID: 3782
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x060053DE RID: 21470 RVA: 0x0003A52B File Offset: 0x0003872B
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
