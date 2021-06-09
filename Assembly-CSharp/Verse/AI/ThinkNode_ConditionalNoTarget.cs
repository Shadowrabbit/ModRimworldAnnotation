using System;

namespace Verse.AI
{
	// Token: 0x02000A7D RID: 2685
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x06004008 RID: 16392 RVA: 0x0002FF72 File Offset: 0x0002E172
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
