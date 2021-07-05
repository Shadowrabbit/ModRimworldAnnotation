using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC7 RID: 3783
	public class ThoughtWorker_IsCarryingIncendiaryWeapon : ThoughtWorker
	{
		// Token: 0x060053E0 RID: 21472 RVA: 0x001C1E80 File Offset: 0x001C0080
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
