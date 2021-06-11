using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
    // Token: 0x02000C96 RID: 3222
    public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
    {
        private float targetAcquireRadius = 56f; //目标获取范围
        private float targetKeepRadius = 65f; //目标维持范围
        private bool needLOSToAcquireNonPawnTargets; //是否需要LOS来获取非pawn目标
        private bool chaseTarget; //追逐目标
        public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550); //射击间隔
        private static readonly IntRange ExpiryInterval_Melee = new IntRange(360, 480); //近战间隔
        private const int MinTargetDistanceToMove = 5; //目标最小距离
        private const int TicksSinceEngageToLoseTarget = 400; //多少tick判定丢失目标
        // Token: 0x06004B14 RID: 19220
        protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest);

        /// <summary>
        /// 标记半径
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        protected virtual float GetFlagRadius(Pawn pawn)
        {
            return 999999f;
        }

        /// <summary>
        /// 标记位置
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        protected virtual IntVec3 GetFlagPosition(Pawn pawn)
        {
            return IntVec3.Invalid;
        }

        /// <summary>
        /// 额外的目标验证器
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
        {
            return true;
        }

        // Token: 0x06004B18 RID: 19224 RVA: 0x0003594A File Offset: 0x00033B4A
        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
            jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
            jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
            jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
            jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
            return jobGiver_AIFightEnemy;
        }

        // Token: 0x06004B19 RID: 19225 RVA: 0x001A40EC File Offset: 0x001A22EC
        protected override Job TryGiveJob(Pawn pawn)
        {
            this.UpdateEnemyTarget(pawn);
            Thing enemyTarget = pawn.mindState.enemyTarget;
            //没有目标
            if (enemyTarget == null)
            {
                return null;
            }
            Pawn pawn2 = enemyTarget as Pawn;
            //目标不可见
            if (pawn2 != null && pawn2.IsInvisible())
            {
                return null;
            }
            bool allowManualCastWeapons = !pawn.IsColonist;
            Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
            //没有攻击动作
            if (verb == null)
            {
                return null;
            }
            //近战动作
            if (verb.verbProps.IsMeleeAttack)
            {
                return MeleeAttackJob(enemyTarget);
            }
            //命中>1%
            bool flag = CoverUtility.CalculateOverallBlockChance(pawn, enemyTarget.Position, pawn.Map) > 0.01f;
            //角色位置可以站立 并且角色可以保留
            bool flag2 = pawn.Position.Standable(pawn.Map) &&
                         pawn.Map.pawnDestinationReservationManager.CanReserve(pawn.Position, pawn, pawn.Drafted);
            //动作可以攻击目标
            bool flag3 = verb.CanHitTarget(enemyTarget);
            //距离目标小于25
            bool flag4 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
            //等待战斗
            if ((flag && flag2 && flag3) || (flag4 && flag3))
            {
                return JobMaker.MakeJob(JobDefOf.Wait_Combat, ExpiryInterval_ShooterSucceeded.RandomInRange, true);
            }
            IntVec3 intVec;
            //没有合适的射击位置
            if (!TryFindShootingPosition(pawn, out intVec))
            {
                return null;
            }
            //查找的射击位置与当前位置相同
            if (intVec == pawn.Position)
            {
                return JobMaker.MakeJob(JobDefOf.Wait_Combat, ExpiryInterval_ShooterSucceeded.RandomInRange, true);
            }
            //前往找到的射击位置
            Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
            job.expiryInterval = ExpiryInterval_ShooterSucceeded.RandomInRange;
            job.checkOverrideOnExpire = true;
            return job;
        }

        // Token: 0x06004B1A RID: 19226 RVA: 0x001A4260 File Offset: 0x001A2460
        protected virtual Job MeleeAttackJob(Thing enemyTarget)
        {
            Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, enemyTarget);
            job.expiryInterval = ExpiryInterval_Melee.RandomInRange;
            job.checkOverrideOnExpire = true;
            job.expireRequiresEnemiesNearby = true;
            return job;
        }

        /// <summary>
        /// 更新敌人目标
        /// </summary>
        /// <param name="pawn"></param>
        protected virtual void UpdateEnemyTarget(Pawn pawn)
        {
            //当前目标
            Thing thing = pawn.mindState.enemyTarget;
            //当前目标存在 并且 目标已销毁或目标已丢失或角色无法解除目标或目标太远或目标威胁禁用
            if (thing != null && (thing.Destroyed || Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > 400 ||
                                  !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly) ||
                                  (pawn.Position - thing.Position).LengthHorizontalSquared >
                                  targetKeepRadius * targetKeepRadius || ((IAttackTarget)thing).ThreatDisabled(pawn)))
            {
                //清理目标
                thing = null;
            }
            //如果目标不存在
            if (thing == null)
            {
                //寻找可能的攻击目标
                thing = FindAttackTargetIfPossible(pawn);
                //找到目标
                if (thing != null)
                {
                    //记录目标获取时间
                    pawn.mindState.Notify_EngagedTarget();
                    //获取集群AI 
                    Lord lord = pawn.GetLord();
                    if (lord != null)
                    {
                        //通知AI找到了目标
                        lord.Notify_PawnAcquiredTarget(pawn, thing);
                    }
                }
            }
            //目标存在
            else
            {
                //再次寻找一个目标
                Thing thing2 = FindAttackTargetIfPossible(pawn);
                //目标不存在 并且不追逐目标
                if (thing2 == null && !chaseTarget)
                {
                    thing = null;
                }
                //目标存在 并且两个目标不相同
                else if (thing2 != null && thing2 != thing)
                {
                    //更换目标 
                    pawn.mindState.Notify_EngagedTarget();
                    thing = thing2;
                }
            }
            //设置攻击目标
            pawn.mindState.enemyTarget = thing;
            //被攻击的是玩家的角色 并且当前角色与玩家角色距离40以内
            if (thing is Pawn && thing.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(thing.Position, 40f))
            {
                //设置一倍速 
                Find.TickManager.slower.SignalForceNormalSpeed();
            }
        }

        // Token: 0x06004B1C RID: 19228 RVA: 0x00035988 File Offset: 0x00033B88
        private Thing FindAttackTargetIfPossible(Pawn pawn)
        {
            //获取攻击动作
            if (pawn.TryGetAttackVerb(null, !pawn.IsColonist) == null)
            {
                return null;
            }
            //攻击动作存在 寻找目标
            return FindAttackTarget(pawn);
        }

        // Token: 0x06004B1D RID: 19229 RVA: 0x001A43D0 File Offset: 0x001A25D0
        protected virtual Thing FindAttackTarget(Pawn pawn)
        {
            //目标扫描标记 开关组
            TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos |
                                              TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
            //非角色目标是否需要LOS查找
            if (needLOSToAcquireNonPawnTargets)
            {
                targetScanFlags |= TargetScanFlags.NeedLOSToNonPawns;
            }
            //角色基本动作是燃烧性的
            if (PrimaryVerbIsIncendiary(pawn))
            {
                //目标需要非可燃
                targetScanFlags |= TargetScanFlags.NeedNonBurning;
            }
            return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, x => ExtraTargetValidator(pawn, x), 0f,
                targetAcquireRadius, GetFlagPosition(pawn), GetFlagRadius(pawn));
        }

        /// <summary>
        /// 基本动作是燃烧性的
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        private bool PrimaryVerbIsIncendiary(Pawn pawn)
        {
            if (pawn.equipment != null && pawn.equipment.Primary != null)
            {
                List<Verb> allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
                for (int i = 0; i < allVerbs.Count; i++)
                {
                    if (allVerbs[i].verbProps.isPrimary)
                    {
                        return allVerbs[i].IsIncendiary();
                    }
                }
            }
            return false;
        }
    }
}
