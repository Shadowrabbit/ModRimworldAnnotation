using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200090D RID: 2317
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x0014F4F7 File Offset: 0x0014D6F7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
