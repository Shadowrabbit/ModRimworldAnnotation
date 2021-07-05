using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E45 RID: 3653
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		// Token: 0x060052BD RID: 21181 RVA: 0x00039CDB File Offset: 0x00037EDB
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
