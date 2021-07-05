using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000914 RID: 2324
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x06003C5B RID: 15451 RVA: 0x0014F59C File Offset: 0x0014D79C
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
