using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000915 RID: 2325
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x06003C5D RID: 15453 RVA: 0x0014F5C0 File Offset: 0x0014D7C0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
