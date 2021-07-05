using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E52 RID: 3666
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		// Token: 0x060052D8 RID: 21208 RVA: 0x00039E10 File Offset: 0x00038010
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
