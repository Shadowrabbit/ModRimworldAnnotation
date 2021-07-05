using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCD RID: 3533
	public class PawnsArrivalModeWorker_CenterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06005200 RID: 20992 RVA: 0x001BAA79 File Offset: 0x001B8C79
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x001BAA84 File Offset: 0x001B8C84
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near;
			if (!DropCellFinder.TryFindRaidDropCenterClose(out near, map, true, true, true, -1))
			{
				near = DropCellFinder.FindRaidDropCenterDistant(map, false);
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001BAAB0 File Offset: 0x001B8CB0
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			if (!parms.spawnCenter.IsValid)
			{
				bool flag = parms.faction != null && parms.faction == Faction.OfMechanoids;
				bool flag2 = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
				if (Rand.Chance(0.4f) && !flag && map.listerBuildings.ColonistsHaveBuildingWithPowerOn(ThingDefOf.OrbitalTradeBeacon))
				{
					parms.spawnCenter = DropCellFinder.TradeDropSpot(map);
				}
				else if (!DropCellFinder.TryFindRaidDropCenterClose(out parms.spawnCenter, map, !flag && flag2, !flag, true, -1))
				{
					parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
					return parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms);
				}
			}
			return true;
		}

		// Token: 0x04003076 RID: 12406
		public const int PodOpenDelay = 520;
	}
}
