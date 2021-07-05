using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001051 RID: 4177
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x060062D4 RID: 25300 RVA: 0x002177B8 File Offset: 0x002159B8
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
