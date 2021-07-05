using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5B RID: 3675
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x060052EB RID: 21227 RVA: 0x00039EA3 File Offset: 0x000380A3
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
