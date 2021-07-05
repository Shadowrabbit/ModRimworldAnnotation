using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200090E RID: 2318
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x06003C4F RID: 15439 RVA: 0x0014F504 File Offset: 0x0014D704
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
