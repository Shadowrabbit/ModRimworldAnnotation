using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FA RID: 2298
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06003C25 RID: 15397 RVA: 0x0014F226 File Offset: 0x0014D426
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
