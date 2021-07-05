using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200090B RID: 2315
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x06003C48 RID: 15432 RVA: 0x0014F42B File Offset: 0x0014D62B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
