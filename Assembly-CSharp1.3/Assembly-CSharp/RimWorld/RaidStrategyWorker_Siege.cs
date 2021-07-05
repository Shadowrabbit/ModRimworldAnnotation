using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008DE RID: 2270
	public class RaidStrategyWorker_Siege : RaidStrategyWorker
	{
		// Token: 0x06003B8D RID: 15245 RVA: 0x0014C360 File Offset: 0x0014A560
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 siegeSpot = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false);
			float num = parms.points * Rand.Range(0.2f, 0.3f);
			if (num < 60f)
			{
				num = 60f;
			}
			return new LordJob_Siege(parms.faction, siegeSpot, num);
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0014C3C8 File Offset: 0x0014A5C8
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canSiege;
		}
	}
}
