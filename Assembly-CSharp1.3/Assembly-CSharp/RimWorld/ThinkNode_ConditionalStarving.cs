using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000916 RID: 2326
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x06003C5F RID: 15455 RVA: 0x0014F5D3 File Offset: 0x0014D7D3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
