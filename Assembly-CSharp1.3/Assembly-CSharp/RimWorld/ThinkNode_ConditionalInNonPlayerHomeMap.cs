using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000924 RID: 2340
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06003C7E RID: 15486 RVA: 0x0014F8C0 File Offset: 0x0014DAC0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
