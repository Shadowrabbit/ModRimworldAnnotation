using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008DD RID: 2269
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x06003B8A RID: 15242 RVA: 0x0014C2FC File Offset: 0x0014A4FC
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false);
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0014C340 File Offset: 0x0014A540
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}
	}
}
