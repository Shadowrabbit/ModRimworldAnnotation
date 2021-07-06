using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E70 RID: 3696
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06005318 RID: 21272 RVA: 0x0003A0D2 File Offset: 0x000382D2
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.Spawned)
			{
				return pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
			}
			return false;
		}
	}
}
