using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FE RID: 2302
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x06003C2D RID: 15405 RVA: 0x0014F2A3 File Offset: 0x0014D4A3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
