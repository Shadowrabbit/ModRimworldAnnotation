using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000ACC RID: 2764
	public static class LordMaker
	{
		/// <summary>
		/// 创建集群AI 集群AI会存在地图的集群AI管理器中
		/// </summary>
		/// <param name="faction"></param>
		/// <param name="lordJob"></param>
		/// <param name="map"></param>
		/// <param name="startingPawns"></param>
		/// <returns></returns>
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.", false);
				return null;
			}
			Lord lord = new Lord();
			lord.loadID = Find.UniqueIDsManager.GetNextLordID();
			lord.faction = faction;
			map.lordManager.AddLord(lord);
			lord.SetJob(lordJob);
			lord.GotoToil(lord.Graph.StartingToil);
			if (startingPawns != null)
			{
				foreach (Pawn p in startingPawns)
				{
					lord.AddPawn(p);
				}
			}
			return lord;
		}
	}
}
