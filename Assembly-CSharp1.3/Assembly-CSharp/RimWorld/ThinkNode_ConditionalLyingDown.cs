using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091F RID: 2335
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06003C73 RID: 15475 RVA: 0x0014F78E File Offset: 0x0014D98E
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
