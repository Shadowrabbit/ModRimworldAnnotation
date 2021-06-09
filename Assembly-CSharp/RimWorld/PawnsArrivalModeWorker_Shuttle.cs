using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143F RID: 5183
	public class PawnsArrivalModeWorker_Shuttle : PawnsArrivalModeWorker
	{
		// Token: 0x06006FCA RID: 28618 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x06006FCB RID: 28619 RVA: 0x00223914 File Offset: 0x00221B14
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			if (dropPods.Count > 1)
			{
				TransportPodsArrivalActionUtility.DropShuttle_NewTemp(dropPods, map, IntVec3.Invalid, null);
				return;
			}
			ActiveDropPodInfo activeDropPodInfo = dropPods[0];
			List<Pawn> requiredPawns = (from t in activeDropPodInfo.innerContainer
			where t is Pawn
			select t).Cast<Pawn>().ToList<Pawn>();
			Thing thing = TransportPodsArrivalActionUtility.DropShuttle_NewTemp(dropPods, map, IntVec3.Invalid, null);
			thing.questTags = activeDropPodInfo.questTags;
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (compShuttle != null)
			{
				compShuttle.sendAwayIfQuestFinished = activeDropPodInfo.sendAwayIfQuestFinished;
				if (activeDropPodInfo.missionShuttleHome != null || activeDropPodInfo.missionShuttleTarget != null)
				{
					compShuttle.missionShuttleTarget = activeDropPodInfo.missionShuttleHome;
					compShuttle.missionShuttleHome = null;
					compShuttle.stayAfterDroppedEverythingOnArrival = true;
					compShuttle.requiredPawns = requiredPawns;
					compShuttle.hideControls = false;
					if (compShuttle.missionShuttleTarget == null)
					{
						using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)compShuttle.Transporter.innerContainer).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn pawn;
								if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsColonist)
								{
									pawn.inventory.UnloadEverything = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006FCC RID: 28620 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			return true;
		}
	}
}
