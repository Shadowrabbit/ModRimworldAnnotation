using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
    // Token: 0x02001424 RID: 5156
    public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
    {
        /// <summary>
        /// 生成需要的最小点数
        /// </summary>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker)
        {
            return (from x in groupMaker.options
                where x.kind.isFighter
                select x).Min(g => g.Cost);
        }

        /// <summary>
        /// 是否可以从参数中生成
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
        {
            return base.CanGenerateFrom(parms, groupMaker) && PawnGroupMakerUtility
                .ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).Any<PawnGenOption>();
        }

        /// <summary>
        /// 生成角色列表
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <param name="outPawns"></param>
        /// <param name="errorOnZeroResults"></param>
        protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns,
            bool errorOnZeroResults = true)
        {
            //无法生成的情况
            if (!CanGenerateFrom(parms, groupMaker))
            {
                if (!errorOnZeroResults)
                    return;
                Log.Error("Cannot generate pawns for " + parms.faction + " with " + parms.points +
                          ". Defaulting to a single random cheap group.");
            }
            else
            {
                //袭击策略不存在 或 袭击策略允许带食物 或 派系存在并且派系对玩家不是敌对
                var allowFood = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood ||
                                parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer);
                //判定函数 
                var validatorPostGear = parms.raidStrategy != null
                    ? p => parms.raidStrategy.Worker.CanUsePawn(p, outPawns)
                    : (Predicate<Pawn>) null;
                var flag = false;
                foreach (var genOptionsByPoint in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(
                    parms.points, groupMaker.options, parms))
                {
                    var pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(genOptionsByPoint.kind,
                        parms.faction, tile: parms.tile, mustBeCapableOfViolence: true, allowFood: allowFood,
                        inhabitant: parms.inhabitants, validatorPostGear: validatorPostGear));
                    if (parms.forceOneIncap && !flag)
                    {
                        pawn.health.forceIncap = true;
                        pawn.mindState.canFleeIndividual = false;
                        flag = true;
                    }

                    outPawns.Add(pawn);
                }
            }
        }

        /// <summary>
        /// 生成示例角色种类定义列表 根据生成选项
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms,
            PawnGroupMaker groupMaker)
        {
            return PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points,
                groupMaker.options, parms).Select(pawnGenOption => pawnGenOption.kind);
        }
    }
}