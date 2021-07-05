using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000927 RID: 2343
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06003C84 RID: 15492 RVA: 0x0014F95C File Offset: 0x0014DB5C
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
