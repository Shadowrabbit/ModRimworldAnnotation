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
                   (desperate || (float) GenDate.DaysPassed >= f.def.earliestRaidDays);
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

            Find.TickManager.slower.SignalForceNormalSpeedShort();
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
            Map map = (Map) parms.target;
            if (parms.faction != null)
            {
                return true;
            }

            float num = parms.points;
            if (num <= 0f)
            {
                num = 999999f;
            }

            return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction,
                       (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) ||
                   PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction,
                       (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true);
        }

        // Token: 0x06006475 RID: 25717 RVA: 0x00044E0F File Offset: 0x0004300F
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

            Map map = (Map) parms.target;
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

        // Token: 0x06006477 RID: 25719 RVA: 0x00044E3A File Offset: 0x0004303A
        protected override string GetLetterLabel(IncidentParms parms)
        {
            return parms.raidStrategy.letterLabelEnemy + ": " + parms.faction.Name;
        }

        // Token: 0x06006478 RID: 25720 RVA: 0x001F2ECC File Offset: 0x001F10CC
        protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
        {
            string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural,
                parms.faction.Name.ApplyTag(parms.faction)).CapitalizeFirst();
            text += "\n\n";
            text += parms.raidStrategy.arrivalTextEnemy;
            Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
            if (pawn != null)
            {
                text += "\n\n";
                text += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort,
                    pawn.Named("LEADER"));
            }

            return text;
        }

        // Token: 0x06006479 RID: 25721 RVA: 0x00044E5C File Offset: 0x0004305C
        protected override LetterDef GetLetterDef()
        {
            return LetterDefOf.ThreatBig;
        }

        // Token: 0x0600647A RID: 25722 RVA: 0x00044E63 File Offset: 0x00043063
        protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
        {
            return "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural,
                parms.faction.def.pawnsPlural);
        }

        // Token: 0x0600647B RID: 25723 RVA: 0x001F2FB0 File Offset: 0x001F11B0
        protected override void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
        {
            if (parms.faction.def.raidLootMaker == null || !pawns.Any<Pawn>())
            {
                return;
            }

            raidLootPoints *= Find.Storyteller.difficultyValues.EffectiveRaidLootPointsFactor;
            float num = parms.faction.def.raidLootValueFromPointsCurve.Evaluate(raidLootPoints);
            if (parms.raidStrategy != null)
            {
                num *= parms.raidStrategy.raidLootValueFactor;
            }

            ThingSetMakerParams parms2 = default(ThingSetMakerParams);
            parms2.totalMarketValueRange = new FloatRange?(new FloatRange(num, num));
            parms2.makingFaction = parms.faction;
            List<Thing> loot = parms.faction.def.raidLootMaker.root.Generate(parms2);
            new RaidLootDistributor(parms, pawns, loot).DistributeLoot();
        }
    }
}