using System;

namespace Verse.AI
{
	// Token: 0x02000A80 RID: 2688
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06004010 RID: 16400 RVA: 0x0002FFC7 File Offset: 0x0002E1C7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
