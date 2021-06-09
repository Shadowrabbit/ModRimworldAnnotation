using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E54 RID: 3668
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x060052DC RID: 21212 RVA: 0x00039E1B File Offset: 0x0003801B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
