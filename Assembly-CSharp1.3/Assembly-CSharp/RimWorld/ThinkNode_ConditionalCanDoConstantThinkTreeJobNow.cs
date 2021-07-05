using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000921 RID: 2337
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x06003C77 RID: 15479 RVA: 0x0014F813 File Offset: 0x0014DA13
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
