using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E76 RID: 3702
	public class ThinkNode_ConditionalAnyEnemyInHostileMap : ThinkNode_Conditional
	{
		// Token: 0x06005326 RID: 21286 RVA: 0x001BFE5C File Offset: 0x001BE05C
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			Map map = pawn.Map;
			return !map.IsPlayerHome && map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer) && GenHostility.AnyHostileActiveThreatToPlayer(map, true);
		}
	}
}
