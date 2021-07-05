using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E68 RID: 3688
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		// Token: 0x06005307 RID: 21255 RVA: 0x0003A03A File Offset: 0x0003823A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
