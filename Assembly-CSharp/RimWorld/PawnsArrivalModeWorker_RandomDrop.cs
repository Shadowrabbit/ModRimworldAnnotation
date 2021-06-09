using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143E RID: 5182
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06006FC7 RID: 28615 RVA: 0x002238AC File Offset: 0x00221AAC
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			for (int i = 0; i < pawns.Count; i++)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map), map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, canRoofPunch, true);
			}
		}

		// Token: 0x06006FC8 RID: 28616 RVA: 0x0004B7C7 File Offset: 0x000499C7
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
