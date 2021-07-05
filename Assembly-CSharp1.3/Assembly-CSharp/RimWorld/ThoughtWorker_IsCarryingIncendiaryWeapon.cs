using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x06003E0A RID: 15882 RVA: 0x001540D4 File Offset: 0x001522D4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsIncendiary())
				{
					return true;
				}
			}
			return false;
		}
	}
}
