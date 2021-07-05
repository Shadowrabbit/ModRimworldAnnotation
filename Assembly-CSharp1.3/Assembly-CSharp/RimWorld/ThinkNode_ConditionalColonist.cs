using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000900 RID: 2304
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06003C31 RID: 15409 RVA: 0x0014F2CF File Offset: 0x0014D4CF
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
