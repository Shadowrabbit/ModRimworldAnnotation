using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E56 RID: 3670
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x060052E1 RID: 21217 RVA: 0x00039E2B File Offset: 0x0003802B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
