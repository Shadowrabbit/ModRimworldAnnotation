using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000912 RID: 2322
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x06003C57 RID: 15447 RVA: 0x0014F56F File Offset: 0x0014D76F
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
