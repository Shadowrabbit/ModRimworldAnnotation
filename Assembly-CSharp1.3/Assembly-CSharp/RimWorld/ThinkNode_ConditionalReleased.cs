using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FF RID: 2303
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06003C2F RID: 15407 RVA: 0x0014F2B8 File Offset: 0x0014D4B8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
