using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E63 RID: 3683
	public class ThinkNode_ConditionalExhausted : ThinkNode_Conditional
	{
		// Token: 0x060052FC RID: 21244 RVA: 0x00039F61 File Offset: 0x00038161
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.rest != null && pawn.needs.rest.CurCategory >= RestCategory.Exhausted;
		}
	}
}
