using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
    //角色组制作器 适配器模式 使用不同的PawnGroupKindWorker去生成角色
    public class PawnGroupMaker
    {
        /// <summary>
        /// 生成需要的最小点数
        /// </summary>
        public float MinPointsToGenerateAnything
        {
            get { return this.kindDef.Worker.MinPointsToGenerateAnything(this); }
        }

        /// <summary>
        /// 生成角色
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="errorOnZeroResults"></param>
        /// <returns></returns>
        public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
        {
            return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
        }

        /// <summary>
        /// 生成示例角色种类定义列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
        {
            return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
        }

        /// <summary>
        /// 是否可以从参数中生成
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public bool CanGenerateFrom(PawnGroupMakerParms parms)
        {
            return parms.points <= this.maxTotalPoints &&
                   (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) &&
                   this.kindDef.Worker.CanGenerateFrom(parms, this);
        }

        // Token: 0x040049A1 RID: 18849
        public PawnGroupKindDef kindDef; //角色组定义

        // Token: 0x040049A2 RID: 18850
        public float commonality = 100f;

        // Token: 0x040049A3 RID: 18851
        public List<RaidStrategyDef> disallowedStrategies; //袭击策略

        // Token: 0x040049A4 RID: 18852
        public float maxTotalPoints = 9999999f;

        // Token: 0x040049A5 RID: 18853
        public List<PawnGenOption> options = new List<PawnGenOption>();

        // Token: 0x040049A6 RID: 18854
        public List<PawnGenOption> traders = new List<PawnGenOption>();

        // Token: 0x040049A7 RID: 18855
        public List<PawnGenOption> carriers = new List<PawnGenOption>();

        // Token: 0x040049A8 RID: 18856
        public List<PawnGenOption> guards = new List<PawnGenOption>();
    }
}