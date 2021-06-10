// Decompiled with JetBrains decompiler
// Type: RimWorld.PawnGroupMakerUtility
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimworldFactionalWar\Source\ModRimworldFactionalWar\Lib\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
    public class PawnGroupMakerUtility
    {
        private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve()
        {
            {
                new CurvePoint(0.2f, 0.01f),
                true
            },
            {
                new CurvePoint(0.3f, 0.3f),
                true
            },
            {
                new CurvePoint(0.5f, 1f),
                true
            }
        };

        public static IEnumerable<Pawn> GeneratePawns(
            PawnGroupMakerParms parms,
            bool warnOnZeroResults = true)
        {
            if (parms.groupKind == null)
                Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + (object) parms);
            else if (parms.faction == null)
                Log.Error("Tried to generate pawn kinds with null faction. parms=" + (object) parms);
            else if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
            {
                Log.Error("Faction " + (object) parms.faction + " of def " + (object) parms.faction.def +
                          " has no any PawnGroupMakers.");
            }
            else
            {
                PawnGroupMaker pawnGroupMaker;
                if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker))
                {
                    Log.Error("Faction " + (object) parms.faction + " of def " + (object) parms.faction.def +
                              " has no usable PawnGroupMakers for parms " + (object) parms);
                }
                else
                {
                    foreach (Pawn pawn in pawnGroupMaker.GeneratePawns(parms, warnOnZeroResults))
                        yield return pawn;
                }
            }
        }

        public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(
            PawnGroupMakerParms parms)
        {
            if (parms.groupKind == null)
                Log.Error("Tried to generate pawn kinds with null pawn group kind def. parms=" + (object) parms);
            else if (parms.faction == null)
                Log.Error("Tried to generate pawn kinds with null faction. parms=" + (object) parms);
            else if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
            {
                Log.Error("Faction " + (object) parms.faction + " of def " + (object) parms.faction.def +
                          " has no any PawnGroupMakers.");
            }
            else
            {
                PawnGroupMaker pawnGroupMaker;
                if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker))
                {
                    Log.Error("Faction " + (object) parms.faction + " of def " + (object) parms.faction.def +
                              " has no usable PawnGroupMakers for parms " + (object) parms);
                }
                else
                {
                    foreach (PawnKindDef pawnKindDef in pawnGroupMaker.GeneratePawnKindsExample(parms))
                        yield return pawnKindDef;
                }
            }
        }

        private static bool TryGetRandomPawnGroupMaker(
            PawnGroupMakerParms parms,
            out PawnGroupMaker pawnGroupMaker)
        {
            if (parms.seed.HasValue)
                Rand.PushState(parms.seed.Value);
            int num = parms.faction.def.pawnGroupMakers
                .Where<PawnGroupMaker>((Func<PawnGroupMaker, bool>) (gm =>
                    gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)))
                .TryRandomElementByWeight<PawnGroupMaker>((Func<PawnGroupMaker, float>) (gm => gm.commonality),
                    out pawnGroupMaker)
                ? 1
                : 0;
            if (!parms.seed.HasValue)
                return num != 0;
            Rand.PopState();
            return num != 0;
        }

        /// <summary>
        /// 根据点数选择角色生成选项
        /// </summary>
        /// <param name="pointsTotal"></param>
        /// <param name="options"></param>
        /// <param name="groupParms"></param>
        /// <returns></returns>
        public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(
            float pointsTotal,
            List<PawnGenOption> options,
            PawnGroupMakerParms groupParms)
        {
            //参数存在种子
            if (groupParms.seed.HasValue)
                Rand.PushState(groupParms.seed.Value);
            //最大角色消耗
            var num1 = MaxPawnCost(groupParms.faction, pointsTotal, groupParms.raidStrategy,
                groupParms.groupKind);
            var source = new List<PawnGenOption>();
            var chosenGroups = new List<PawnGenOption>();
            //剩余点数
            var num2 = pointsTotal;
            var flag = false;
            var highestCost = -1f;
            while (true)
            {
                PawnGenOption pawnGenOption;
                //选项种类不是派系首领
                do
                {
                    source.Clear();
                    //遍历选项
                    for (int index = 0; index < options.Count; ++index)
                    {
                        PawnGenOption option = options[index];
                        //选项消耗小于等于剩余点数 并且选项消耗小于等于最大角色消耗 并且选项种类是战士或不仅仅生成战士
                        //并且袭击策略不存在或袭击策略可使用
                        //并且可以使用单发火箭发射器或武器标签不存在或武器标签包含可以使用一次性武器
                        //并且选项种类不是派系首领
                        if ((double) option.Cost <= num2 && option.Cost <= (double) num1 &&
                            (!groupParms.generateFightersOnly || option.kind.isFighter) 
                            && (groupParms.raidStrategy == null ||
                                groupParms.raidStrategy.Worker.CanUsePawnGenOption(option, chosenGroups)) &&
                            ((!groupParms.dontUseSingleUseRocketLaunchers || option.kind.weaponTags == null ||
                              !option.kind.weaponTags.Contains("GunSingleUse")) &&
                             (!flag || !option.kind.factionLeader)))
                        {
                            //记录最高消耗者
                            if ((double) option.Cost > highestCost)
                                highestCost = option.Cost;
                            source.Add(option);
                        }
                    }
                    //选项列表存在
                    if (source.Count != 0)
                    {
                        //权重选择器
                        Func<PawnGenOption, float> weightSelector = gr =>
                            gr.selectionWeight *
                            PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(
                                gr.Cost / highestCost);
                        //随机选择选项
                        pawnGenOption = source.RandomElementByWeight(weightSelector);
                        //添加结果
                        chosenGroups.Add(pawnGenOption);
                        num2 -= pawnGenOption.Cost;
                    }
                    //选项列表为空
                    else
                        goto label_13;
                } while (!pawnGenOption.kind.factionLeader);

                flag = true;
            }

            label_13:
            if (chosenGroups.Count == 1 && num2 > pointsTotal / 2.0)
                Log.Warning("Used only " + (pointsTotal - (double) num2) + " / " +
                            pointsTotal + " points generating for " + groupParms.faction);
            //取消种子保存
            if (groupParms.seed.HasValue)
                Rand.PopState();
            //返回选项列表
            return chosenGroups;
        }

        public static float MaxPawnCost(
            Faction faction,
            float totalPoints,
            RaidStrategyDef raidStrategy,
            PawnGroupKindDef groupKind)
        {
            //计算评估点数
            var a1 = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
            //评估点数 不能大于总点数/最小角色数
            if (raidStrategy != null)
                a1 = Mathf.Min(a1, totalPoints / raidStrategy.minPawns);
            //取评估点数 与 1.2倍最低生成点数中的最大值
            var a2 = Mathf.Max(a1, faction.def.MinPointsToGeneratePawnGroup(groupKind) * 1.2f);
            //如果袭击策略存在
            if (raidStrategy != null)
                a2 = Mathf.Max(a2, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
            return a2;
        }

        public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
        {
            if (faction.def.pawnGroupMakers == null)
                return false;
            PawnGroupMakerParms parms = new PawnGroupMakerParms();
            parms.faction = faction;
            parms.points = points;
            for (int index = 0; index < faction.def.pawnGroupMakers.Count; ++index)
            {
                PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[index];
                if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat && pawnGroupMaker.CanGenerateFrom(parms))
                    return true;
            }

            return false;
        }

        [DebugOutput]
        public static void PawnGroupsMade() => Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(
            Find.FactionManager.AllFactions.Where<Faction>((Func<Faction, bool>) (fac =>
                !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())),
            (Func<Faction, string>) (fac => fac.Name + " (" + fac.def.defName + ")"), (Action<Faction>) (fac =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("FACTION: " + fac.Name + " (" + fac.def.defName + ") min=" +
                              (object) fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
                Action<float> action = (Action<float>) (points =>
                {
                    if ((double) points < (double) fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
                        return;
                    PawnGroupMakerParms parms = new PawnGroupMakerParms();
                    parms.groupKind = PawnGroupKindDefOf.Combat;
                    parms.tile = Find.CurrentMap.Tile;
                    parms.points = points;
                    parms.faction = fac;
                    sb.AppendLine("Group with " + (object) parms.points + " points (max option cost: " +
                                  (object) PawnGroupMakerUtility.MaxPawnCost(fac, points,
                                      RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat) + ")");
                    float num = 0.0f;
                    foreach (Pawn pawn in (IEnumerable<Pawn>) PawnGroupMakerUtility.GeneratePawns(parms, false)
                        .OrderBy<Pawn, float>((Func<Pawn, float>) (pa => pa.kindDef.combatPower)))
                    {
                        string str1 = pawn.equipment.Primary == null ? "no-equipment" : pawn.equipment.Primary.Label;
                        Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
                        string str2 = apparel == null ? "shirtless" : apparel.LabelCap;
                        sb.AppendLine("  " + pawn.kindDef.combatPower.ToString("F0").PadRight(6) +
                                      pawn.kindDef.defName + ", " + str1 + ", " + str2);
                        num += pawn.kindDef.combatPower;
                    }

                    sb.AppendLine("         totalCost " + (object) num);
                    sb.AppendLine();
                });
                foreach (float pointsOption in DebugActionsUtility.PointsOptions(false))
                    action(pointsOption);
                Log.Message(sb.ToString());
            }));

        public static bool TryGetRandomFactionForCombatPawnGroup(
            float points,
            out Faction faction,
            Predicate<Faction> validator = null,
            bool allowNonHostileToPlayer = false,
            bool allowHidden = false,
            bool allowDefeated = false,
            bool allowNonHumanlike = true)
        {
            return Find.FactionManager.AllFactions.Where<Faction>((Func<Faction, bool>) (f =>
                    (allowHidden || !f.Hidden) && !f.temporary &&
                    ((allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction)) &&
                    ((allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null) &&
                    (f.def.pawnGroupMakers.Any<PawnGroupMaker>(
                         (Predicate<PawnGroupMaker>) (x => x.kindDef == PawnGroupKindDefOf.Combat)) &&
                     (validator == null || validator(f))) && (double) points >=
                    (double) f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))).ToList<Faction>()
                .TryRandomElementByWeight<Faction>(
                    (Func<Faction, float>) (f => f.def.RaidCommonalityFromPoints(points)),
                    out faction);
        }
    }
}