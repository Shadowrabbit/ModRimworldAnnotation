using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E57 RID: 3671
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x060052E3 RID: 21219 RVA: 0x00039E38 File Offset: 0x00038038
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.Animal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def);
		}
	}
}
