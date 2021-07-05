using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E43 RID: 3651
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x060052B9 RID: 21177 RVA: 0x00039CC0 File Offset: 0x00037EC0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
