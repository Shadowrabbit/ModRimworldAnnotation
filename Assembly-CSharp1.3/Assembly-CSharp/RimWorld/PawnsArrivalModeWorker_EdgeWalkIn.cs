using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD1 RID: 3537
	public class PawnsArrivalModeWorker_EdgeWalkIn : PawnsArrivalModeWorker
	{
		// Token: 0x0600520F RID: 21007 RVA: 0x001BAD04 File Offset: 0x001B8F04
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false);
			}
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x001BAD54 File Offset: 0x001B8F54
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			PawnsArrivalModeWorker_EdgeWalkIn.<>c__DisplayClass1_0 CS$<>8__locals1 = new PawnsArrivalModeWorker_EdgeWalkIn.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = (Map)parms.target;
			if (parms.attackTargets != null && parms.attackTargets.Count > 0 && !RCellFinder.TryFindEdgeCellFromThingAvoidingColony(parms.attackTargets[0], CS$<>8__locals1.map, new Func<IntVec3, IntVec3, bool>(CS$<>8__locals1.<TryResolveRaidSpawnCenter>g__predicate|0), out parms.spawnCenter))
			{
				CellFinder.TryFindRandomEdgeCellWith((IntVec3 p) => !CS$<>8__locals1.map.roofGrid.Roofed(p) && p.Walkable(CS$<>8__locals1.map), CS$<>8__locals1.map, CellFinder.EdgeRoadChance_Hostile, out parms.spawnCenter);
			}
			if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, CS$<>8__locals1.map, CellFinder.EdgeRoadChance_Hostile, false, null))
			{
				return false;
			}
			parms.spawnRotation = Rot4.FromAngleFlat((CS$<>8__locals1.map.Center - parms.spawnCenter).AngleFlat);
			return true;
		}
	}
}
