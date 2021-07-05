using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000932 RID: 2354
	public class ThinkNode_ConditionalWantsLookChange : ThinkNode_Conditional
	{
		// Token: 0x06003C9C RID: 15516 RVA: 0x0014FD53 File Offset: 0x0014DF53
		protected override bool Satisfied(Pawn pawn)
		{
			return ModsConfig.IdeologyActive && pawn.style != null && pawn.style.LookChangeDesired;
		}
	}
}
