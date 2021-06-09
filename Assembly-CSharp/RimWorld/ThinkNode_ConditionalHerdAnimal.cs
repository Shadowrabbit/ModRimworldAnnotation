using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E59 RID: 3673
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x060052E7 RID: 21223 RVA: 0x00039E6D File Offset: 0x0003806D
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
