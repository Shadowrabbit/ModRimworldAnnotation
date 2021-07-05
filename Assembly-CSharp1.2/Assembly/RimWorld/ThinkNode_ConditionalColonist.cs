using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E49 RID: 3657
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x060052C5 RID: 21189 RVA: 0x0001DF7C File Offset: 0x0001C17C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
