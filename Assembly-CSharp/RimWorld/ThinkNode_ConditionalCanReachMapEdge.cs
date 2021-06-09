using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E60 RID: 3680
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x060052F5 RID: 21237 RVA: 0x00039F08 File Offset: 0x00038108
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge();
		}
	}
}
