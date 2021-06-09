using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5E RID: 3678
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x060052F1 RID: 21233 RVA: 0x00039ECE File Offset: 0x000380CE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
