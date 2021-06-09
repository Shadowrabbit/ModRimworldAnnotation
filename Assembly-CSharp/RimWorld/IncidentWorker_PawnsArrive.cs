using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
    /// <summary>
    /// 小人到达
    /// </summary>
    public abstract class IncidentWorker_PawnsArrive : IncidentWorker
    {
        /// <summary>
        /// 获取候选派系
        /// </summary>
        /// <param name="map"></param>
        /// <param name="desperate"></param>
        /// <returns></returns>
        protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
        {
            return from f in Find.FactionManager.AllFactions
                where this.FactionCanBeGroupSource(f, map, desperate)
                select f;
        }

        /// <summary>
        /// 派系是否能是组资源
        /// </summary>
        /// <param name="f"></param>
        /// <param name="map"></param>
        /// <param name="desperate"></param>
        /// <returns></returns>
        protected virtual bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
        {
            //派系不是玩家 没有被击败 不是临时（绝望 或派系允许温度中包含室外与季节）
            return !f.IsPlayer && !f.defeated && !f.temporary && (desperate ||
                                                                  (f.def.allowedArrivalTemperatureRange.Includes(
                                                                       map.mapTemperature.OutdoorTemp) &&
                                                                   f.def.allowedArrivalTemperatureRange.Includes(
                                                                       map.mapTemperature.SeasonalTemp)));
        }

        /// <summary>
        /// 当前是否可以生成该事件
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map) parms.target;
            
            //事件指定了派系 或地图上存在可使用的派系
            return parms.faction != null || this.CandidateFactions(map, false).Any<Faction>();
        }

        /// <summary>
        /// 测试组资源
        /// </summary>
        /// <returns></returns>
        public string DebugListingOfGroupSources()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Faction faction in Find.FactionManager.AllFactions)
            {
                stringBuilder.Append(faction.Name);
                if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, false))
                {
                    stringBuilder.Append("    YES");
                }
                else if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, true))
                {
                    stringBuilder.Append("    YES-DESPERATE");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}