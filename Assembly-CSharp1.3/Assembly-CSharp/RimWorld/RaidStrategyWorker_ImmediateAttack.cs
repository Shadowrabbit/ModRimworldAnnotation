using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008D8 RID: 2264
	public class RaidStrategyWorker_ImmediateAttack : RaidStrategyWorker
	{
		// Token: 0x06003B7B RID: 15227 RVA: 0x0014C104 File Offset: 0x0014A304
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 originCell = parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld;
			if (parms.attackTargets != null && parms.attackTargets.Count > 0)
			{
				return new LordJob_AssaultThings(parms.faction, parms.attackTargets, 1f, false);
			}
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				return new LordJob_AssaultColony(parms.faction, true, parms.canTimeoutOrFlee, false, false, true, false, false);
			}
			IntVec3 fallbackLocation;
			RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out fallbackLocation);
			return new LordJob_AssistColony(parms.faction, fallbackLocation);
		}
	}
}
