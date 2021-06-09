using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167D RID: 5757
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x06007DA4 RID: 32164 RVA: 0x000545C7 File Offset: 0x000527C7
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
