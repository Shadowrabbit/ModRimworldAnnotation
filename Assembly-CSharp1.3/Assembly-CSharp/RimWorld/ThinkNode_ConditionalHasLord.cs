using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000909 RID: 2313
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		// Token: 0x06003C44 RID: 15428 RVA: 0x0014F3F6 File Offset: 0x0014D5F6
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
