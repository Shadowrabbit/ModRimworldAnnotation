using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200092F RID: 2351
	public class ThinkNode_ConditionalRoped : ThinkNode_Conditional
	{
		// Token: 0x06003C95 RID: 15509 RVA: 0x0014FCB7 File Offset: 0x0014DEB7
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_RopeTracker roping = pawn.roping;
			return roping != null && roping.IsRoped;
		}
	}
}
