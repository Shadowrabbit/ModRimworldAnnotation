using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000913 RID: 2323
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x06003C59 RID: 15449 RVA: 0x0014F581 File Offset: 0x0014D781
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
