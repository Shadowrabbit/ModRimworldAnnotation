using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091A RID: 2330
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x06003C68 RID: 15464 RVA: 0x0014F666 File Offset: 0x0014D866
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
