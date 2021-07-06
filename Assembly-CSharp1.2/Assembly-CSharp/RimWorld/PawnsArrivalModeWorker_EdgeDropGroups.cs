using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143B RID: 5179
	public class PawnsArrivalModeWorker_EdgeDropGroups : PawnsArrivalModeWorker
	{
		/// <summary>
		/// 登场
		/// </summary>
		/// <param name="pawns"></param>
		/// <param name="parms"></param>
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			var map = (Map)parms.target;
			//是否可以从屋顶进入
			var canRoofPunch = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			//分组信息
			var list = PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(pawns, map, true);
			//设置分组信息
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			//空投到战场
			for (int i = 0; i < list.Count; i++)
			{
				DropPodUtility.DropThingsNear(list[i].Second, map, list[i].First.Cast<Thing>(), parms.podOpenDelay, false, true, canRoofPunch, true);
			}
		}

		// Token: 0x06006FBF RID: 28607 RVA: 0x0004B7B9 File Offset: 0x000499B9
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
