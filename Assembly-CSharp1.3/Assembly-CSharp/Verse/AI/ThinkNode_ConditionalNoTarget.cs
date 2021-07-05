using System;

namespace Verse.AI
{
	// Token: 0x0200061E RID: 1566
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06002D27 RID: 11559 RVA: 0x0010F253 File Offset: 0x0010D453
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
