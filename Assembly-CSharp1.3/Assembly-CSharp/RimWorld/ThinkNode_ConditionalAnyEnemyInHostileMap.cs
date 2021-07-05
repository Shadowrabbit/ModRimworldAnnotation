using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200092C RID: 2348
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x06003C8F RID: 15503 RVA: 0x0014FA14 File Offset: 0x0014DC14
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map, true, true);
		}
	}
}
