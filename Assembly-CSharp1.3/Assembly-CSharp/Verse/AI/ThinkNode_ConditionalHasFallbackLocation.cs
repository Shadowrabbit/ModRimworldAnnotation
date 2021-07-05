using System;

namespace Verse.AI
{
	// Token: 0x02000621 RID: 1569
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06002D2F RID: 11567 RVA: 0x0010F324 File Offset: 0x0010D524
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
