using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
    // Token: 0x020011F3 RID: 4595
    public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
    {
        /// <summary>
        /// 解决袭击派系
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected abstract bool TryResolveRaidFaction(IncidentParms parms);

        /// <summary>
        /// 解决突袭策略
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupKind"></param>
        public abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

        /// <summary>
        /// 获取信件标签
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected abstract string GetLetterLabel(IncidentParms parms);

        /// <summary>
        /// 获取信件内容
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="pawns"></param>
        /// <returns></returns>
        protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

        /// <summary>
        /// 获取信件定义
        /// </summary>
        /// <returns></returns>
        protected abstract LetterDef GetLetterDef();

        /// <summary>
        /// 获取相关的角色信息信件内容
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

        /// <summary>
        /// 解决袭击点数
        /// </summary>
        /// <param name="parms"></param>
        protected abstract void ResolveRaidPoints(IncidentParms parms);

        /// <summary>
        /// 解决袭击到达模式
        /// </summary>
        /// <param name="parms"></param>
        public virtual void ResolveRaidArriveMode(IncidentParms parms)
        {
            //到达模式存在
            if (parms.raidArrivalMode != null)
            {
                return;
            }
            //快速军事援助的突袭抵达模式
            if (parms.raidArrivalModeForQuickMilitaryAid)
            {
                //找到快速军事援助的到达模式 且这些模式中权重评估均不大于0
                if (!(from d in DefDatabase<PawnsArrivalModeDef>.AllDefs
                    where d.forQuickMilitaryAid
                    select d).Any((PawnsArrivalModeDef d) => d.Worker.GetSelectionWeight(parms) > 0f))
                {
                    //设置边缘或中心掉落
                    parms.raidArrivalMode = ((Rand.Value < 0.6f)
                        ? PawnsArrivalModeDefOf.EdgeDrop
                        : PawnsArrivalModeDefOf.CenterDrop);
                    return;
                }
            }

            //袭击策略不存在
            if (parms.raidStrategy == null)
            {
                Log.Error("parms raidStrategy was null but shouldn't be. Defaulting to ImmediateAttack.", false);
                parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            }

            //到达模式不存在
            if (!(from x in parms.raidStrategy.arriveModes
                where x.Worker.CanUseWith(parms)
                select x).TryRandomElementByWeight((PawnsArrivalModeDef x) => x.Worker.GetSelectionWeight(parms),
                out parms.raidArrivalMode))
            {
                Log.Error("Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=" + parms, false);
                parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            }
        }

        /// <summary>
        /// 生成突袭战利品
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="raidLootPoints"></param>
        /// <param name="pawns"></param>
        protected virtual void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
        {
        }

        /// <summary>
        /// 尝试执行事件
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            //解决袭击点数
            this.ResolveRaidPoints(parms);
            //尝试解决袭击派系
            if (!this.TryResolveRaidFaction(parms))
            {
                return false;
            }
            //角色组定义 战斗
            var combat = PawnGroupKindDefOf.Combat;
            //解决袭击策略
            ResolveRaidStrategy(parms, combat);
            //解决到达方式
            ResolveRaidArriveMode(parms);
            //尝试生成威胁（参数）
            parms.raidStrategy.Worker.TryGenerateThreats(parms);
            //尝试解决袭击召唤中心
            if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
            {
                return false;
            }
            //袭击点数
            var points = parms.points;
            //调整袭击点数
            parms.points = AdjustedRaidPoints(parms.points, parms.raidArrivalMode,
                parms.raidStrategy, parms.faction, combat);
            //生成威胁
            var list = parms.raidStrategy.Worker.SpawnThreats(parms);
            //生成失败 尝试用默认角色组生成器
            if (list == null)
            {
                list = PawnGroupMakerUtility
                    .GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms))
                    .ToList();
                if (list.Count == 0)
                {
                    Log.Error("Got no pawns spawning raid from parms " + parms, false);
                    return false;
                }

                parms.raidArrivalMode.Worker.Arrive(list, parms);
            }
            //生成突袭战利品
            GenerateRaidLoot(parms, points, list);
            TaggedString baseLetterLabel = GetLetterLabel(parms);
            TaggedString baseLetterText = GetLetterText(parms, list);
            PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText,
                GetRelatedPawnsInfoLetterText(parms), true);
            //目标列表
            var list2 = new List<TargetInfo>();
            //分组成员存在
            if (parms.pawnGroups != null)
            {
                //分组
                List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
                //选长度最大的组
                List<Pawn> list4 = list3.MaxBy(delegate(List<Pawn> x)
                {
                    return x.Count;
                });
                //第一个角色丢进目标列表
                if (list4.Any<Pawn>())
                {
                    list2.Add(list4[0]);
                }
                
                for (int i = 0; i < list3.Count; i++)
                {
                    //当前组不是长度最大的组 并且里面有角色
                    if (list3[i] != list4 && list3[i].Any<Pawn>())
                    {
                        //组内第一个角色丢进目标列表
                        list2.Add(list3[i][0]);
                    }
                }
            }
            //存在威胁角色
            else if (list.Any())
            {
                //添加到目标列表
                foreach (Pawn t in list)
                {
                    list2.Add(t);
                }
            }
            //提示信件
            SendStandardLetter(baseLetterLabel, baseLetterText, GetLetterDef(), parms, list2,
                Array.Empty<NamedArgument>());
            //根据分组生成集群AI 在袭击策略中
            parms.raidStrategy.Worker.MakeLords(parms, list);
            //教程激活
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
            //教程 护盾腰带
            if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
                    {
                        LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
                        break;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 调整袭击点数
        /// </summary>
        /// <param name="points"></param>
        /// <param name="raidArrivalMode"></param>
        /// <param name="raidStrategy"></param>
        /// <param name="faction"></param>
        /// <param name="groupKind"></param>
        /// <returns></returns>
        public static float AdjustedRaidPoints(float points, PawnsArrivalModeDef raidArrivalMode,
            RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind)
        {
            if (raidArrivalMode.pointsFactorCurve != null)
            {
                points *= raidArrivalMode.pointsFactorCurve.Evaluate(points);
            }

            if (raidStrategy.pointsFactorCurve != null)
            {
                points *= raidStrategy.pointsFactorCurve.Evaluate(points);
            }

            points = Mathf.Max(points, raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f);
            return points;
        }

        /// <summary>
        /// 袭击派系采样测试
        /// </summary>
        public void DoTable_RaidFactionSampled()
        {
            int ticksGame = Find.TickManager.TicksGame;
            Find.TickManager.DebugSetTicksGame(36000000);
            List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
            list.Add(new TableDataGetter<Faction>("name", (Faction f) => f.Name));
            foreach (float points in DebugActionsUtility.PointsOptions(false))
            {
                Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
                foreach (Faction key in Find.FactionManager.AllFactions)
                {
                    factionCount.Add(key, 0);
                }

                for (int i = 0; i < 500; i++)
                {
                    IncidentParms incidentParms = new IncidentParms();
                    incidentParms.target = Find.CurrentMap;
                    incidentParms.points = points;
                    if (this.TryResolveRaidFaction(incidentParms))
                    {
                        Dictionary<Faction, int> factionCount2 = factionCount;
                        Faction faction = incidentParms.faction;
                        int num = factionCount2[faction];
                        factionCount2[faction] = num + 1;
                    }
                }

                list.Add(new TableDataGetter<Faction>(points.ToString("F0"),
                    (Faction str) => ((float)factionCount[str] / 500f).ToStringPercent()));
            }

            Find.TickManager.DebugSetTicksGame(ticksGame);
            DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
        }

        /// <summary>
        /// 袭击策略采样测试
        /// </summary>
        /// <param name="fac"></param>
        public void DoTable_RaidStrategySampled(Faction fac)
        {
            int ticksGame = Find.TickManager.TicksGame;
            Find.TickManager.DebugSetTicksGame(36000000);
            List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
            list.Add(new TableDataGetter<RaidStrategyDef>("defName", (RaidStrategyDef d) => d.defName));
            foreach (float points in DebugActionsUtility.PointsOptions(false))
            {
                Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
                foreach (RaidStrategyDef key in DefDatabase<RaidStrategyDef>.AllDefs)
                {
                    strats.Add(key, 0);
                }

                for (int i = 0; i < 500; i++)
                {
                    IncidentParms incidentParms = new IncidentParms();
                    incidentParms.target = Find.CurrentMap;
                    incidentParms.points = points;
                    incidentParms.faction = fac;
                    if (this.TryResolveRaidFaction(incidentParms))
                    {
                        this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
                        if (incidentParms.raidStrategy != null)
                        {
                            Dictionary<RaidStrategyDef, int> strats2 = strats;
                            RaidStrategyDef raidStrategy = incidentParms.raidStrategy;
                            int num = strats2[raidStrategy];
                            strats2[raidStrategy] = num + 1;
                        }
                    }
                }

                list.Add(new TableDataGetter<RaidStrategyDef>(points.ToString("F0"),
                    (RaidStrategyDef str) => ((float)strats[str] / 500f).ToStringPercent()));
            }

            Find.TickManager.DebugSetTicksGame(ticksGame);
            DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
        }

        /// <summary>
        /// 入场方式采样测试
        /// </summary>
        /// <param name="fac"></param>
        public void DoTable_RaidArrivalModeSampled(Faction fac)
        {
            int ticksGame = Find.TickManager.TicksGame;
            Find.TickManager.DebugSetTicksGame(36000000);
            List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
            list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (PawnsArrivalModeDef f) => f.defName));
            foreach (float points in DebugActionsUtility.PointsOptions(false))
            {
                Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
                foreach (PawnsArrivalModeDef key in DefDatabase<PawnsArrivalModeDef>.AllDefs)
                {
                    modeCount.Add(key, 0);
                }

                for (int i = 0; i < 500; i++)
                {
                    IncidentParms incidentParms = new IncidentParms();
                    incidentParms.target = Find.CurrentMap;
                    incidentParms.points = points;
                    incidentParms.faction = fac;
                    if (this.TryResolveRaidFaction(incidentParms))
                    {
                        this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
                        if (incidentParms.raidStrategy != null)
                        {
                            this.ResolveRaidArriveMode(incidentParms);
                            Dictionary<PawnsArrivalModeDef, int> modeCount2 = modeCount;
                            PawnsArrivalModeDef raidArrivalMode = incidentParms.raidArrivalMode;
                            int num = modeCount2[raidArrivalMode];
                            modeCount2[raidArrivalMode] = num + 1;
                        }
                    }
                }

                list.Add(new TableDataGetter<PawnsArrivalModeDef>(points.ToString("F0"),
                    (PawnsArrivalModeDef str) => ((float)modeCount[str] / 500f).ToStringPercent()));
            }

            Find.TickManager.DebugSetTicksGame(ticksGame);
            DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
        }
    }
}
