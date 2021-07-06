using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001439 RID: 5177
	public class PawnsArrivalModeWorker_ClusterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06006FB6 RID: 28598 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x00223624 File Offset: 0x00221824
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x00223644 File Offset: 0x00221844
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
