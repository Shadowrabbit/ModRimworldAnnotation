using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
    /// <summary>
    /// 敌人袭击
    /// </summary>
    public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
    {
        /// <summary>
        /// 派系是否可以是组
        /// </summary>
        /// <param name="f"></param>
        /// <param name="map"></param>
        /// <param name="desperate"></param>
        /// <returns></returns>
        protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
        {
            return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) &&
                   (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
        }

        /// <summary>
        /// 尝试执行
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (!base.TryExecuteWorker(parms))
            {
                return false;
            }
            //袭击时设置一倍速
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            //更新参与袭击的敌人记录
            Find.StoryWatcher.statsRecord.numRaidsEnemy++;
            return true;
        }

        /// <summary>
        /// 尝试解决突袭派系
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override bool TryResolveRaidFaction(IncidentParms parms)
        {
            var map = (Map)parms.target;
            if (parms.faction != null)
            {
                return true;
            }

            var num = parms.points;
            if (num <= 0f)
            {
                num = 999999f;
            }

            return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction,
                       (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) ||
                   PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction,
                       (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true);
        }

        /// <summary>
        /// 解决袭击点数
        /// </summary>
        /// <param name="parms"></param>
        protected override void ResolveRaidPoints(IncidentParms parms)
        {
            if (parms.points <= 0f)
            {
                Log.Error(
                    "RaidEnemy is resolving raid points. They should always be set before initiating the incident.",
                    false);
                parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
            }
        }

        /// <summary>
        /// 解决突袭策略
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupKind"></param>
        public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
        {
            //突袭策略存在
            if (parms.raidStrategy != null)
            {
                return;
            }

            Map map = (Map)parms.target;
            RaidStrategyDef result;
            DefDatabase<RaidStrategyDef>.AllDefs.Where(d =>
            {
                //无法使用
                if (!d.Worker.CanUseWith(parms, groupKind))
                    return false;
                //当前事件存在袭击到达模式
                if (parms.raidArrivalMode != null)
                    return true;
                //某个袭击策略定义 存在到达模式 并且可以使用
                return d.arriveModes != null &&
                       d.arriveModes.Any(
                           x => x.Worker.CanUseWith(parms));
            }).TryRandomElementByWeight(
                d => d.Worker.SelectionWeight(map, parms.points), out result);
            //设置事件的袭击策略
            parms.raidStrategy = result;
            if (parms.raidStrategy != null)
                return;
            //找不到袭击策略
            Log.Error("No raid stategy found, defaulting to ImmediateAttack. Faction=" + parms.faction.def.defName +
                      ", points=" + parms.points + ", groupKind=" + groupKind + ", parms=" +
                      parms);
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
        }

        /// <summary>
        /// 获取信件标签
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override string GetLetterLabel(IncidentParms parms)
        {
            return parms.raidStrategy.letterLabelEnemy + ": " + parms.faction.Name;
        }

        /// <summary>
        /// 获取信件文本
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="pawns"></param>
        /// <returns></returns>
        protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
        {
            var text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural,
                parms.faction.Name.ApplyTag(parms.faction)).CapitalizeFirst();
            text += "\n\n";
            text += parms.raidStrategy.arrivalTextEnemy;
            var pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
            if (pawn != null)
            {
                text += "\n\n";
                text += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort,
                    pawn.Named("LEADER"));
            }

            return text;
        }

        /// <summary>
        /// 获取信件定义
        /// </summary>
        /// <returns></returns>
        protected override LetterDef GetLetterDef()
        {
            return LetterDefOf.ThreatBig;
        }

        /// <summary>
        /// 到达的角色中有关系户
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
        {
            return "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural,
                parms.faction.def.pawnsPlural);
        }

        /// <summary>
        /// 生成突袭战利品
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="raidLootPoints"></param>
        /// <param name="pawns"></param>
        protected override void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
        {
            //袭击战利品制作器不存在
            if (parms.faction.def.raidLootMaker == null || !pawns.Any<Pawn>())
            {
                return;
            }
            //袭击战利品点数
            raidLootPoints *= Find.Storyteller.difficultyValues.EffectiveRaidLootPointsFactor;
            //点数调整
            var num = parms.faction.def.raidLootValueFromPointsCurve.Evaluate(raidLootPoints);
            //袭击策略存在
            if (parms.raidStrategy != null)
            {
                //*袭击战利品因数
                num *= parms.raidStrategy.raidLootValueFactor;
            }

            var parms2 = default(ThingSetMakerParams);
            //袭击点数范围
            parms2.totalMarketValueRange = new FloatRange(num, num);
            //制作派系
            parms2.makingFaction = parms.faction;
            //生成战利品
            var loot = parms.faction.def.raidLootMaker.root.Generate(parms2);
            //分发
            new RaidLootDistributor(parms, pawns, loot).DistributeLoot();
        }
    }
}
