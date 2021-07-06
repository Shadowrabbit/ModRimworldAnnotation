using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E5 RID: 4581
	public class IncidentWorker_WandererJoinTransportPod : IncidentWorker_WandererJoin
	{
		// Token: 0x06006454 RID: 25684 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CanSpawnJoiner(Map map)
		{
			return true;
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x001F28C4 File Offset: 0x001F0AC4
		public override void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 c = DropCellFinder.RandomDropSpot(map);
			ActiveDropPodInfo activeDropPodInfo = new ActiveDropPodInfo();
			activeDropPodInfo.innerContainer.TryAddOrTransfer(pawn, true);
			activeDropPodInfo.openDelay = 180;
			activeDropPodInfo.leaveSlag = true;
			DropPodUtility.MakeDropPodAt(c, map, activeDropPodInfo);
		}
	}
}
