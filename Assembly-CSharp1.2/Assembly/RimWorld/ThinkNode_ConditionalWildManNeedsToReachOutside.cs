using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E74 RID: 3700
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x06005322 RID: 21282 RVA: 0x0003A153 File Offset: 0x00038353
		protected override bool Satisfied(Pawn pawn)
		{
			return WildManUtility.WildManShouldReachOutsideNow(pawn);
		}
	}
}
