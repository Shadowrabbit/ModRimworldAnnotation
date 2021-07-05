using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCE RID: 3534
	public class PawnsArrivalModeWorker_ClusterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06005204 RID: 20996 RVA: 0x0000313F File Offset: 0x0000133F
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x001BAB9C File Offset: 0x001B8D9C
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map, false);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x001BABBC File Offset: 0x001B8DBC
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = MechClusterUtility.FindClusterPosition(map, parms.mechClusterSketch, 100, 0.5f);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
