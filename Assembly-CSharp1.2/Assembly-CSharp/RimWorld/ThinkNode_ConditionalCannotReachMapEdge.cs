using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E61 RID: 3681
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x060052F7 RID: 21239 RVA: 0x00039F10 File Offset: 0x00038110
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
