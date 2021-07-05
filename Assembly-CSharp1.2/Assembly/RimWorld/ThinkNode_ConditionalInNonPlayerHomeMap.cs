using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6D RID: 3693
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06005312 RID: 21266 RVA: 0x0003A0AD File Offset: 0x000382AD
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
