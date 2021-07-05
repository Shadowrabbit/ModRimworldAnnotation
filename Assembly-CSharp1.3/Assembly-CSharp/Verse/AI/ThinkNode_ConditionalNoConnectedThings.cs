using System;

namespace Verse.AI
{
	// Token: 0x02000624 RID: 1572
	public class ThinkNode_ConditionalNoConnectedThings : ThinkNode_Conditional
	{
		// Token: 0x06002D36 RID: 11574 RVA: 0x0010F3AC File Offset: 0x0010D5AC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.connections == null || !pawn.connections.ConnectedThings.Any<Thing>();
		}
	}
}
