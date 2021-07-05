using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200090F RID: 2319
	public class ThinkNode_ConditionalHasFaction : ThinkNode_Conditional
	{
		// Token: 0x06003C51 RID: 15441 RVA: 0x0014F52E File Offset: 0x0014D72E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null;
		}
	}
}
