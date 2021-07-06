using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E48 RID: 3656
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x060052C3 RID: 21187 RVA: 0x00039CF8 File Offset: 0x00037EF8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
