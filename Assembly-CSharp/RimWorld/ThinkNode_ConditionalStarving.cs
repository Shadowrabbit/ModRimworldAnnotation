using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5F RID: 3679
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		// Token: 0x060052F3 RID: 21235 RVA: 0x00039EE1 File Offset: 0x000380E1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && pawn.needs.food.CurCategory >= HungerCategory.Starving;
		}
	}
}
