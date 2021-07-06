using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E6A RID: 3690
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x0600530B RID: 21259 RVA: 0x0003A047 File Offset: 0x00038247
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.Downed && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
