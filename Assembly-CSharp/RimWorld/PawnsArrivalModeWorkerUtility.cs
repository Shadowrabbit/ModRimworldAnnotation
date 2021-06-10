using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x02001442 RID: 5186
    public static class PawnsArrivalModeWorkerUtility
    {
        // Token: 0x06006FD6 RID: 28630 RVA: 0x00223C38 File Offset: 0x00221E38
        public static void DropInDropPodsNearSpawnCenter(IncidentParms parms, List<Pawn> pawns)
        {
            Map map = (Map)parms.target;
            bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
            DropPodUtility.DropThingsNear(parms.spawnCenter, map, pawns.Cast<Thing>(), parms.podOpenDelay, false, true,
                flag || parms.raidArrivalModeForQuickMilitaryAid, true);
        }

        /// <summary>
        /// 分割成随机组 在地图边缘
        /// </summary>
        /// <param name="pawns"></param>
        /// <param name="map"></param>
        /// <param name="arriveInPods"></param>
        /// <returns></returns>
        public static List<Pair<List<Pawn>, IntVec3>> SplitIntoRandomGroupsNearMapEdge(List<Pawn> pawns, Map map, bool arriveInPods)
        {
            List<Pair<List<Pawn>, IntVec3>> list = new List<Pair<List<Pawn>, IntVec3>>();
            //列表中不存在角色
            if (!pawns.Any<Pawn>())
            {
                return list;
            }
            //最大组数量
            var maxGroupsCount = GetMaxGroupsCount(pawns.Count);
            //随机生成组的数量
            var num = (maxGroupsCount == 1) ? 1 : Rand.RangeInclusive(2, maxGroupsCount);
            //与组索引相同的角色分成给不同组
            for (var i = 0; i < num; i++)
            {
                //寻找中心
                IntVec3 second = FindNewMapEdgeGroupCenter(map, list, arriveInPods);
                var item = new Pair<List<Pawn>, IntVec3>(new List<Pawn>(), second);
                item.First.Add(pawns[i]);
                list.Add(item);
            }
            //剩余的角色随机分组
            for (int j = num; j < pawns.Count; j++)
            {
                list.RandomElement().First.Add(pawns[j]);
            }
            return list;
        }

        /// <summary>
        /// 查找新地图边缘组中心
        /// </summary>
        /// <param name="map"></param>
        /// <param name="groups"></param>
        /// <param name="arriveInPods"></param>
        /// <returns></returns>
        private static IntVec3 FindNewMapEdgeGroupCenter(Map map, List<Pair<List<Pawn>, IntVec3>> groups, bool arriveInPods)
        {
            IntVec3 result = IntVec3.Invalid;
            float num = 0f;
            for (int i = 0; i < 4; i++)
            {
                IntVec3 intVec;
                if (arriveInPods)
                {
                    intVec = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
                }
                else if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Hostile, false, null))
                {
                    intVec = DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
                }
                if (!groups.Any())
                {
                    result = intVec;
                    break;
                }
                float num2 = float.MaxValue;
                for (int j = 0; j < groups.Count; j++)
                {
                    float num3 = (float)intVec.DistanceToSquared(groups[j].Second);
                    if (num3 < num2)
                    {
                        num2 = num3;
                    }
                }
                if (!result.IsValid || num2 > num)
                {
                    num = num2;
                    result = intVec;
                }
            }
            return result;
        }

        /// <summary>
        /// 固定分成2~3组
        /// </summary>
        /// <param name="pawnsCount"></param>
        /// <returns></returns>
        private static int GetMaxGroupsCount(int pawnsCount)
        {
            if (pawnsCount <= 1)
            {
                return 1;
            }
            return Mathf.Clamp(pawnsCount / 2, 2, 3);
        }

        // Token: 0x06006FDA RID: 28634 RVA: 0x00223DF4 File Offset: 0x00221FF4
        public static void SetPawnGroupsInfo(IncidentParms parms, List<Pair<List<Pawn>, IntVec3>> groups)
        {
            parms.pawnGroups = new Dictionary<Pawn, int>();
            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < groups[i].First.Count; j++)
                {
                    parms.pawnGroups.Add(groups[i].First[j], i);
                }
            }
        }

        // Token: 0x040049DD RID: 18909
        private const int MaxGroupsCount = 3;
    }
}
