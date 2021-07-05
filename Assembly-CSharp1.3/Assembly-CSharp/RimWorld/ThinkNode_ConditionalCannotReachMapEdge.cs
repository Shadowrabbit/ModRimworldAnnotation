using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000918 RID: 2328
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06003C63 RID: 15459 RVA: 0x0014F602 File Offset: 0x0014D802
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
