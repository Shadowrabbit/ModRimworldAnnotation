using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008DA RID: 2266
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x06003B7F RID: 15231 RVA: 0x0014C1D4 File Offset: 0x0014A3D4
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, parms.canTimeoutOrFlee, false, true, true, false, false);
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0014C1ED File Offset: 0x0014A3ED
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
