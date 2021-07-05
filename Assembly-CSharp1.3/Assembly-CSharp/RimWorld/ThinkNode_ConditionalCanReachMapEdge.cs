using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000917 RID: 2327
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06003C61 RID: 15457 RVA: 0x0014F5FA File Offset: 0x0014D7FA
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge();
		}
	}
}
