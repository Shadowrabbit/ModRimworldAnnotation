using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5C RID: 3676
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x060052ED RID: 21229 RVA: 0x00039EB5 File Offset: 0x000380B5
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
