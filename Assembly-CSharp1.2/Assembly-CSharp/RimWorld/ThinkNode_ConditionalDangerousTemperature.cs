using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E5D RID: 3677
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x060052EF RID: 21231 RVA: 0x001BFC80 File Offset: 0x001BDE80
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
