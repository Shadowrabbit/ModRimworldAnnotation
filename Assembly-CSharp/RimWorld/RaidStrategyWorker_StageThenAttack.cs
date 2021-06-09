using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E25 RID: 3621
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x0600521F RID: 21023 RVA: 0x001BD788 File Offset: 0x001BB988
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom_NewTemp(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false);
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed);
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x000396F8 File Offset: 0x000378F8
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}
	}
}
