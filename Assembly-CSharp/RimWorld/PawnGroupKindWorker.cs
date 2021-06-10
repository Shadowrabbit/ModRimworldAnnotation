using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
    // Token: 0x02000FC1 RID: 4033
    public abstract class PawnGroupKindWorker
    {
        public PawnGroupKindDef def; //角色组定义
        public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>(); //等待生成的角色列表

        /// <summary>
        /// 生成需要的最小点数
        /// </summary>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker);

        /// <summary>
        /// 生成角色列表
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <param name="errorOnZeroResults"></param>
        /// <returns></returns>
        public List<Pawn> GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker,
            bool errorOnZeroResults = true)
        {
            List<Pawn> list = new List<Pawn>();
            pawnsBeingGeneratedNow.Add(list);
            try
            {
                GeneratePawns(parms, groupMaker, list, errorOnZeroResults);
            }
            catch (Exception arg)
            {
                Log.Error("Exception while generating pawn group: " + arg, false);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Destroy(DestroyMode.Vanish);
                }

                list.Clear();
            }
            finally
            {
                pawnsBeingGeneratedNow.Remove(list);
            }

            return list;
        }

        /// <summary>
        /// 生成角色列表 outPawns是返回值
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <param name="outPawns"></param>
        /// <param name="errorOnZeroResults"></param>
        protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns,
            bool errorOnZeroResults = true);

        /// <summary>
        /// 是否可以从参数中生成
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
        {
            return true;
        }

        /// <summary>
        /// 生成示例角色种类定义列表
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="groupMaker"></param>
        /// <returns></returns>
        public abstract IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms,
            PawnGroupMaker groupMaker);
    }
}