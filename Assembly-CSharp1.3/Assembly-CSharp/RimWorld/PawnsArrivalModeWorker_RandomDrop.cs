using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD3 RID: 3539
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06005215 RID: 21013 RVA: 0x001BAECC File Offset: 0x001B90CC
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			for (int i = 0; i < pawns.Count; i++)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map, true), map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, canRoofPunch, true);
			}
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001BAF35 File Offset: 0x001B9135
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
