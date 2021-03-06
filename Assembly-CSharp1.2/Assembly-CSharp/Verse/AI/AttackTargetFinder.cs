using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse.AI
{
    // Token: 0x02000AB0 RID: 2736
    public static class AttackTargetFinder
    {
        // Token: 0x060040A9 RID: 16553 RVA: 0x00183378 File Offset: 0x00181578
        public static IAttackTarget BestAttackTarget(IAttackTargetSearcher searcher, TargetScanFlags flags,
            Predicate<Thing> validator = null, float minDist = 0f, float maxDist = 9999f, IntVec3 locus = default(IntVec3),
            float maxTravelRadiusFromLocus = 3.4028235E+38f, bool canBash = false, bool canTakeTargetsCloserThanEffectiveMinRange = true)
        {
            //搜索者
            Thing searcherThing = searcher.Thing;
            //搜索者角色
            Pawn searcherPawn = searcher as Pawn;
            //搜索者当前的动作
            Verb verb = searcher.CurrentEffectiveVerb;
            if (verb == null)
            {
                Log.Error("BestAttackTarget with " + searcher.ToStringSafe() + " who has no attack verb.");
                return null;
            }
            //如果动作是emp类 则标记目标必须是机械
            bool onlyTargetMachines = verb.IsEMP();
            //最小距离平方
            float minDistSquared = minDist * minDist;
            //当前搜索半径 = 轨迹的最大移动半径 与 动作攻击半径的 和
            float num = maxTravelRadiusFromLocus + verb.verbProps.range;
            //搜索半径方
            float maxLocusDistSquared = num * num;
            //视线校验器
            Func<IntVec3, bool> losValidator = null;
            //目标可被气体阻塞
            if ((flags & TargetScanFlags.LOSBlockableByGas) != TargetScanFlags.None)
            {
                losValidator = delegate(IntVec3 vec3)
                {
                    //获取地图上某个坐标的气体
                    Gas gas = vec3.GetGas(searcherThing.Map);
                    //气体不存在 或气体不打断机枪
                    return gas == null || !gas.def.gas.blockTurretTracking;
                };
            }
            //内部校验器
            Predicate<IAttackTarget> innerValidator = delegate(IAttackTarget t)
            {
                //攻击目标
                Thing thing = t.Thing;
                //攻击目标是自身
                if (t == searcher)
                {
                    return false;
                }
                //小于最小距离
                if (minDistSquared > 0f && (float)(searcherThing.Position - thing.Position).LengthHorizontalSquared < minDistSquared)
                {
                    return false;
                }
                //不在最小范围内
                if (!canTakeTargetsCloserThanEffectiveMinRange)
                {
                    float num2 = verb.verbProps.EffectiveMinRange(thing, searcherThing);
                    if (num2 > 0f && (float)(searcherThing.Position - thing.Position).LengthHorizontalSquared < num2 * num2)
                    {
                        return false;
                    }
                }
                //超过最大距离
                if (maxTravelRadiusFromLocus < 9999f && (float)(thing.Position - locus).LengthHorizontalSquared > maxLocusDistSquared)
                {
                    return false;
                }
                //不是敌对的
                if (!searcherThing.HostileTo(thing))
                {
                    return false;
                }
                //验证失败
                if (validator != null && !validator(thing))
                {
                    return false;
                }
                //搜索者是角色
                if (searcherPawn != null)
                {
                    //获取集群AI
                    Lord lord = searcherPawn.GetLord();
                    //集群AI当前工作攻击目标校验失败
                    if (lord != null && !lord.LordJob.ValidateAttackTarget(searcherPawn, thing))
                    {
                        return false;
                    }
                }
                //不需要在厚屋顶下
                if ((flags & TargetScanFlags.NeedNotUnderThickRoof) != TargetScanFlags.None)
                {
                    //当前处于屋顶下
                    RoofDef roof = thing.Position.GetRoof(thing.Map);
                    if (roof != null && roof.isThickRoof)
                    {
                        return false;
                    }
                }
                //全部需要视野
                if ((flags & TargetScanFlags.NeedLOSToAll) != TargetScanFlags.None)
                {
                    //视野校验器存在 并且搜寻者或攻击目标校验失败
                    if (losValidator != null && (!losValidator(searcherThing.Position) || !losValidator(thing.Position)))
                    {
                        return false;
                    }
                    //搜寻者看不见攻击目标
                    if (!searcherThing.CanSee(thing, losValidator))
                    {
                        //目标是角色
                        if (t is Pawn)
                        {
                            //需要角色视野
                            if ((flags & TargetScanFlags.NeedLOSToPawns) != TargetScanFlags.None)
                            {
                                return false;
                            }
                        }
                        //需要非角色视野
                        else if ((flags & TargetScanFlags.NeedLOSToNonPawns) != TargetScanFlags.None)
                        {
                            return false;
                        }
                    }
                }
                //需要有威胁或自动目标 并且攻击目标对于搜寻者威胁禁用
                if (((flags & TargetScanFlags.NeedThreat) != TargetScanFlags.None ||
                     (flags & TargetScanFlags.NeedAutoTargetable) != TargetScanFlags.None) && t.ThreatDisabled(searcher))
                {
                    return false;
                }
                //自动目标 并且攻击目标可以是自动目标
                if ((flags & TargetScanFlags.NeedAutoTargetable) != TargetScanFlags.None && !IsAutoTargetable(t))
                {
                    return false;
                }
                //需要激活威胁 攻击目标是搜寻者派系的威胁
                if ((flags & TargetScanFlags.NeedActiveThreat) != TargetScanFlags.None &&
                    !GenHostility.IsActiveThreatTo(t, searcher.Thing.Faction))
                {
                    return false;
                }
                Pawn pawn = t as Pawn;
                if (onlyTargetMachines && pawn != null && pawn.RaceProps.IsFlesh)
                {
                    return false;
                }
                //需要不可燃
                if ((flags & TargetScanFlags.NeedNonBurning) != TargetScanFlags.None && thing.IsBurning())
                {
                    return false;
                }
                //搜寻者存在种族 并且种族智力不小于人类
                if (searcherThing.def.race != null && searcherThing.def.race.intelligence >= Intelligence.Humanlike)
                {
                    //攻击目标是爆炸物 并且已经引燃
                    CompExplosive compExplosive = thing.TryGetComp<CompExplosive>();
                    if (compExplosive != null && compExplosive.wickStarted)
                    {
                        return false;
                    }
                }
                //尺寸为1 并且有雾
                if (thing.def.size.x == 1 && thing.def.size.z == 1)
                {
                    if (thing.Position.Fogged(thing.Map))
                    {
                        return false;
                    }
                }
                else
                {
                    //网格计算雾的影响
                    bool flag2 = false;
                    using (CellRect.Enumerator enumerator = thing.OccupiedRect().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (!enumerator.Current.Fogged(thing.Map))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                    if (!flag2)
                    {
                        return false;
                    }
                }
                return true;
            };
            //搜寻者有远程动作 并且搜寻者是角色 搜寻者不处于快攻状态
            if (HasRangedAttack(searcher) && (searcherPawn == null || !searcherPawn.InAggroMentalState))
            {
                tmpTargets.Clear();
                //从缓存中添加潜在目标
                tmpTargets.AddRange(searcherThing.Map.attackTargetsCache.GetPotentialTargetsFor(searcher));
                //需要能接触
                if ((flags & TargetScanFlags.NeedReachable) != TargetScanFlags.None)
                {
                    //更变内部校验器 并缓存原来的校验器
                    Predicate<IAttackTarget> oldValidator = innerValidator;
                    innerValidator = (t =>
                        oldValidator(t) && CanReach(searcherThing, t.Thing, canBash));
                }
                //存在攻击目标 通过内部校验器 能从当前位置射击
                bool flag = false;
                for (int i = 0; i < tmpTargets.Count; i++)
                {
                    IAttackTarget attackTarget = tmpTargets[i];
                    if (attackTarget.Thing.Position.InHorDistOf(searcherThing.Position, maxDist) && innerValidator(attackTarget) &&
                        CanShootAtFromCurrentPosition(attackTarget, searcher, verb))
                    {
                        flag = true;
                        break;
                    }
                }
                IAttackTarget result;
                //找到了目标 撤销距离远的 或者 校验失败的目标 从剩余目标中随机选取一个作为目标
                if (flag)
                {
                    tmpTargets.RemoveAll(x =>
                        !x.Thing.Position.InHorDistOf(searcherThing.Position, maxDist) || !innerValidator(x));
                    result = GetRandomShootingTargetByScore(tmpTargets, searcher, verb);
                }
                //没找到目标
                else
                {
                    Predicate<Thing> validator2;
                    //如果无法从当前位置攻击 需要接触 并且 整体不需要接触
                    if ((flags & TargetScanFlags.NeedReachableIfCantHitFromMyPos) != TargetScanFlags.None &&
                        (flags & TargetScanFlags.NeedReachable) == TargetScanFlags.None)
                    {
                        validator2 = (t => innerValidator((IAttackTarget)t) &&
                                           (CanReach(searcherThing, t, canBash) ||
                                            CanShootAtFromCurrentPosition((IAttackTarget)t, searcher, verb)));
                    }
                    else
                    {
                        validator2 = ((Thing t) => innerValidator((IAttackTarget)t));
                    }
                    //从目标列表中选最近的
                    result = (IAttackTarget)GenClosest.ClosestThing_Global(searcherThing.Position, tmpTargets, maxDist,
                        validator2, null);
                }
                tmpTargets.Clear();
                return result;
            }
            //搜寻者是角色 并且当前存在职责 并且职责范围大于0 并且搜寻者没有精神失常
            if (searcherPawn != null && searcherPawn.mindState.duty != null && searcherPawn.mindState.duty.radius > 0f &&
                !searcherPawn.InMentalState)
            {
                //缓存内部校验器
                Predicate<IAttackTarget> oldValidator = innerValidator;
                //添加限制 
                innerValidator = (t => oldValidator(t) &&
                                       t.Thing.Position.InHorDistOf(searcherPawn.mindState.duty.focus.Cell,
                                           searcherPawn.mindState.duty.radius));
            }
            //搜索最近的目标
            IAttackTarget attackTarget2 = (IAttackTarget)GenClosest.ClosestThingReachable(searcherThing.Position, searcherThing.Map,
                ThingRequest.ForGroup(ThingRequestGroup.AttackTarget), PathEndMode.Touch,
                TraverseParms.For(searcherPawn, Danger.Deadly, TraverseMode.ByPawn, canBash), maxDist,
                x => innerValidator((IAttackTarget)x), null, 0, (maxDist > 800f) ? -1 : 40);
            //精神失常 启用近战动作 尝试更变目标
            if (attackTarget2 != null && PawnUtility.ShouldCollideWithPawns(searcherPawn))
            {
                //最合适的近战目标
                IAttackTarget attackTarget3 =
                    FindBestReachableMeleeTarget(innerValidator, searcherPawn, maxDist, canBash);
                //合适的近战目标与远程目标距离在50以内 更换为近战目标
                if (attackTarget3 != null)
                {
                    float lengthHorizontal = (searcherPawn.Position - attackTarget2.Thing.Position).LengthHorizontal;
                    float lengthHorizontal2 = (searcherPawn.Position - attackTarget3.Thing.Position).LengthHorizontal;
                    if (Mathf.Abs(lengthHorizontal - lengthHorizontal2) < 50f)
                    {
                        attackTarget2 = attackTarget3;
                    }
                }
            }
            return attackTarget2;
        }

        // Token: 0x060040AA RID: 16554 RVA: 0x001837C4 File Offset: 0x001819C4
        private static bool CanReach(Thing searcher, Thing target, bool canBash)
        {
            Pawn pawn = searcher as Pawn;
            if (pawn != null)
            {
                if (!pawn.CanReach(target, PathEndMode.Touch, Danger.Some, canBash, TraverseMode.ByPawn))
                {
                    return false;
                }
            }
            else
            {
                TraverseMode mode = canBash ? TraverseMode.PassDoors : TraverseMode.NoPassClosedDoors;
                if (!searcher.Map.reachability.CanReach(searcher.Position, target, PathEndMode.Touch,
                    TraverseParms.For(mode, Danger.Deadly, false)))
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x060040AB RID: 16555 RVA: 0x00183824 File Offset: 0x00181A24
        private static IAttackTarget FindBestReachableMeleeTarget(Predicate<IAttackTarget> validator, Pawn searcherPawn, float maxTargDist,
            bool canBash)
        {
            //目标最远距离
            maxTargDist = Mathf.Min(maxTargDist, 30f);
            IAttackTarget reachableTarget = null;
            //计算最合适的目标回调
            Func<IntVec3, IAttackTarget> bestTargetOnCell = delegate(IntVec3 x)
            {
                List<Thing> thingList = x.GetThingList(searcherPawn.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    Thing thing = thingList[i];
                    IAttackTarget attackTarget = thing as IAttackTarget;
                    //当前物体可以为目标 并且通过校验 并且 可以立刻接触 并且可以立刻到达或可以预留
                    if (attackTarget != null && validator(attackTarget) &&
                        ReachabilityImmediate.CanReachImmediate(x, thing, searcherPawn.Map, PathEndMode.Touch, searcherPawn) &&
                        (searcherPawn.CanReachImmediate(thing, PathEndMode.Touch) ||
                         searcherPawn.Map.attackTargetReservationManager.CanReserve(searcherPawn, attackTarget)))
                    {
                        return attackTarget;
                    }
                }
                return null;
            };

            searcherPawn.Map.floodFiller.FloodFill(searcherPawn.Position,
                //路径检测
                delegate(IntVec3 x)
                {
                    //坐标不可行走
                    if (!x.Walkable(searcherPawn.Map))
                    {
                        return false;
                    }
                    //坐标太远
                    if ((float)x.DistanceToSquared(searcherPawn.Position) > maxTargDist * maxTargDist)
                    {
                        return false;
                    }
                    //不能强制攻击
                    if (!canBash)
                    {
                        Building_Door building_Door = x.GetEdifice(searcherPawn.Map) as Building_Door;
                        if (building_Door != null && !building_Door.CanPhysicallyPass(searcherPawn))
                        {
                            return false;
                        }
                    }
                    return !PawnUtility.AnyPawnBlockingPathAt(x, searcherPawn, true);
                }, delegate(IntVec3 x)
                {
                    //相邻8个格子
                    for (int i = 0; i < 8; i++)
                    {
                        IntVec3 intVec = x + GenAdj.AdjacentCells[i];
                        //坐标在地图内
                        if (intVec.InBounds(searcherPawn.Map))
                        {
                            //计算是否是最好的目标
                            IAttackTarget attackTarget = bestTargetOnCell(intVec);
                            if (attackTarget != null)
                            {
                                reachableTarget = attackTarget;
                                break;
                            }
                        }
                    }
                    return reachableTarget != null;
                });
            return reachableTarget;
        }

        // Token: 0x060040AC RID: 16556 RVA: 0x001838C8 File Offset: 0x00181AC8
        private static bool HasRangedAttack(IAttackTargetSearcher t)
        {
            Verb currentEffectiveVerb = t.CurrentEffectiveVerb;
            return currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack;
        }

        // Token: 0x060040AD RID: 16557 RVA: 0x00030627 File Offset: 0x0002E827
        private static bool CanShootAtFromCurrentPosition(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
        {
            return verb != null && verb.CanHitTargetFrom(searcher.Thing.Position, target.Thing);
        }

        // Token: 0x060040AE RID: 16558 RVA: 0x001838F0 File Offset: 0x00181AF0
        private static IAttackTarget GetRandomShootingTargetByScore(List<IAttackTarget> targets, IAttackTargetSearcher searcher, Verb verb)
        {
            Pair<IAttackTarget, float> pair;
            if (AttackTargetFinder.GetAvailableShootingTargetsByScore(targets, searcher, verb)
                .TryRandomElementByWeight((Pair<IAttackTarget, float> x) => x.Second, out pair))
            {
                return pair.First;
            }
            return null;
        }

        // Token: 0x060040AF RID: 16559 RVA: 0x00183938 File Offset: 0x00181B38
        private static List<Pair<IAttackTarget, float>> GetAvailableShootingTargetsByScore(List<IAttackTarget> rawTargets,
            IAttackTargetSearcher searcher, Verb verb)
        {
            AttackTargetFinder.availableShootingTargets.Clear();
            if (rawTargets.Count == 0)
            {
                return AttackTargetFinder.availableShootingTargets;
            }
            AttackTargetFinder.tmpTargetScores.Clear();
            AttackTargetFinder.tmpCanShootAtTarget.Clear();
            float num = 0f;
            IAttackTarget attackTarget = null;
            for (int i = 0; i < rawTargets.Count; i++)
            {
                AttackTargetFinder.tmpTargetScores.Add(float.MinValue);
                AttackTargetFinder.tmpCanShootAtTarget.Add(false);
                if (rawTargets[i] != searcher)
                {
                    bool flag = AttackTargetFinder.CanShootAtFromCurrentPosition(rawTargets[i], searcher, verb);
                    AttackTargetFinder.tmpCanShootAtTarget[i] = flag;
                    if (flag)
                    {
                        float shootingTargetScore = AttackTargetFinder.GetShootingTargetScore(rawTargets[i], searcher, verb);
                        AttackTargetFinder.tmpTargetScores[i] = shootingTargetScore;
                        if (attackTarget == null || shootingTargetScore > num)
                        {
                            attackTarget = rawTargets[i];
                            num = shootingTargetScore;
                        }
                    }
                }
            }
            if (num < 1f)
            {
                if (attackTarget != null)
                {
                    AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(attackTarget, 1f));
                }
            }
            else
            {
                float num2 = num - 30f;
                for (int j = 0; j < rawTargets.Count; j++)
                {
                    if (rawTargets[j] != searcher && AttackTargetFinder.tmpCanShootAtTarget[j])
                    {
                        float num3 = AttackTargetFinder.tmpTargetScores[j];
                        if (num3 >= num2)
                        {
                            float second = Mathf.InverseLerp(num - 30f, num, num3);
                            AttackTargetFinder.availableShootingTargets.Add(new Pair<IAttackTarget, float>(rawTargets[j], second));
                        }
                    }
                }
            }
            return AttackTargetFinder.availableShootingTargets;
        }

        // Token: 0x060040B0 RID: 16560 RVA: 0x00183A9C File Offset: 0x00181C9C
        private static float GetShootingTargetScore(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
        {
            float num = 60f;
            num -= Mathf.Min((target.Thing.Position - searcher.Thing.Position).LengthHorizontal, 40f);
            if (target.TargetCurrentlyAimingAt == searcher.Thing)
            {
                num += 10f;
            }
            if (searcher.LastAttackedTarget == target.Thing && Find.TickManager.TicksGame - searcher.LastAttackTargetTick <= 300)
            {
                num += 40f;
            }
            num -= CoverUtility.CalculateOverallBlockChance(target.Thing.Position, searcher.Thing.Position, searcher.Thing.Map) * 10f;
            Pawn pawn = target as Pawn;
            if (pawn != null && pawn.RaceProps.Animal && pawn.Faction != null && !pawn.IsFighting())
            {
                num -= 50f;
            }
            num += AttackTargetFinder.FriendlyFireBlastRadiusTargetScoreOffset(target, searcher, verb);
            num += AttackTargetFinder.FriendlyFireConeTargetScoreOffset(target, searcher, verb);
            return num * target.TargetPriorityFactor;
        }

        // Token: 0x060040B1 RID: 16561 RVA: 0x00183BB8 File Offset: 0x00181DB8
        private static float FriendlyFireBlastRadiusTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
        {
            if (verb.verbProps.ai_AvoidFriendlyFireRadius <= 0f)
            {
                return 0f;
            }
            Map map = target.Thing.Map;
            IntVec3 position = target.Thing.Position;
            int num = GenRadial.NumCellsInRadius(verb.verbProps.ai_AvoidFriendlyFireRadius);
            float num2 = 0f;
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = position + GenRadial.RadialPattern[i];
                if (intVec.InBounds(map))
                {
                    bool flag = true;
                    List<Thing> thingList = intVec.GetThingList(map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        if (thingList[j] is IAttackTarget && thingList[j] != target)
                        {
                            if (flag)
                            {
                                if (!GenSight.LineOfSight(position, intVec, map, true, null, 0, 0))
                                {
                                    break;
                                }
                                flag = false;
                            }
                            float num3;
                            if (thingList[j] == searcher)
                            {
                                num3 = 40f;
                            }
                            else if (thingList[j] is Pawn)
                            {
                                num3 = (thingList[j].def.race.Animal ? 7f : 18f);
                            }
                            else
                            {
                                num3 = 10f;
                            }
                            if (searcher.Thing.HostileTo(thingList[j]))
                            {
                                num2 += num3 * 0.6f;
                            }
                            else
                            {
                                num2 -= num3;
                            }
                        }
                    }
                }
            }
            return num2;
        }

        // Token: 0x060040B2 RID: 16562 RVA: 0x00183D28 File Offset: 0x00181F28
        private static float FriendlyFireConeTargetScoreOffset(IAttackTarget target, IAttackTargetSearcher searcher, Verb verb)
        {
            Pawn pawn = searcher.Thing as Pawn;
            if (pawn == null)
            {
                return 0f;
            }
            if (pawn.RaceProps.intelligence < Intelligence.ToolUser)
            {
                return 0f;
            }
            if (pawn.RaceProps.IsMechanoid)
            {
                return 0f;
            }
            Verb_Shoot verb_Shoot = verb as Verb_Shoot;
            if (verb_Shoot == null)
            {
                return 0f;
            }
            ThingDef defaultProjectile = verb_Shoot.verbProps.defaultProjectile;
            if (defaultProjectile == null)
            {
                return 0f;
            }
            if (defaultProjectile.projectile.flyOverhead)
            {
                return 0f;
            }
            Map map = pawn.Map;
            ShotReport report = ShotReport.HitReportFor(pawn, verb, (Thing)target);
            float radius =
                Mathf.Max(
                    VerbUtility.CalculateAdjustedForcedMiss(verb.verbProps.forcedMissRadius,
                        report.ShootLine.Dest - report.ShootLine.Source), 1.5f);
            Func<IntVec3, bool> <>9__3;
            IEnumerable<IntVec3> enumerable = (from dest in GenRadial.RadialCellsAround(report.ShootLine.Dest, radius, true)
                where dest.InBounds(map)
                select new ShootLine(report.ShootLine.Source, dest)).SelectMany(delegate(ShootLine line)
            {
                IEnumerable<IntVec3> source = line.Points().Concat(line.Dest);
                Func<IntVec3, bool> predicate;
                if ((predicate =  <>9__3) == null)
                {
                    predicate = (<>9__3 = ((IntVec3 pos) => pos.CanBeSeenOverFast(map)));
                }
                return source.TakeWhile(predicate);
            }).Distinct<IntVec3>();
            float num = 0f;
            foreach (IntVec3 c in enumerable)
            {
                float num2 = VerbUtility.InterceptChanceFactorFromDistance(report.ShootLine.Source.ToVector3Shifted(), c);
                if (num2 > 0f)
                {
                    List<Thing> thingList = c.GetThingList(map);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        Thing thing = thingList[i];
                        if (thing is IAttackTarget && thing != target)
                        {
                            float num3;
                            if (thing == searcher)
                            {
                                num3 = 40f;
                            }
                            else if (thing is Pawn)
                            {
                                num3 = (thing.def.race.Animal ? 7f : 18f);
                            }
                            else
                            {
                                num3 = 10f;
                            }
                            num3 *= num2;
                            if (searcher.Thing.HostileTo(thing))
                            {
                                num3 *= 0.6f;
                            }
                            else
                            {
                                num3 *= -1f;
                            }
                            num += num3;
                        }
                    }
                }
            }
            return num;
        }

        // Token: 0x060040B3 RID: 16563 RVA: 0x00183FB0 File Offset: 0x001821B0
        public static IAttackTarget BestShootTargetFromCurrentPosition(IAttackTargetSearcher searcher, TargetScanFlags flags,
            Predicate<Thing> validator = null, float minDistance = 0f, float maxDistance = 9999f)
        {
            Verb currentEffectiveVerb = searcher.CurrentEffectiveVerb;
            if (currentEffectiveVerb == null)
            {
                Log.Error(
                    "BestShootTargetFromCurrentPosition with " + searcher.ToStringSafe<IAttackTargetSearcher>() +
                    " who has no attack verb.", false);
                return null;
            }
            return AttackTargetFinder.BestAttackTarget(searcher, flags, validator,
                Mathf.Max(minDistance, currentEffectiveVerb.verbProps.minRange),
                Mathf.Min(maxDistance, currentEffectiveVerb.verbProps.range), default(IntVec3), float.MaxValue, false, false);
        }

        // Token: 0x060040B4 RID: 16564 RVA: 0x00184020 File Offset: 0x00182220
        public static bool CanSee(this Thing seer, Thing target, Func<IntVec3, bool> validator = null)
        {
            ShootLeanUtility.CalcShootableCellsOf(AttackTargetFinder.tempDestList, target);
            for (int i = 0; i < AttackTargetFinder.tempDestList.Count; i++)
            {
                if (GenSight.LineOfSight(seer.Position, AttackTargetFinder.tempDestList[i], seer.Map, true, validator, 0, 0))
                {
                    return true;
                }
            }
            ShootLeanUtility.LeanShootingSourcesFromTo(seer.Position, target.Position, seer.Map, AttackTargetFinder.tempSourceList);
            for (int j = 0; j < AttackTargetFinder.tempSourceList.Count; j++)
            {
                for (int k = 0; k < AttackTargetFinder.tempDestList.Count; k++)
                {
                    if (GenSight.LineOfSight(AttackTargetFinder.tempSourceList[j], AttackTargetFinder.tempDestList[k], seer.Map, true,
                        validator, 0, 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Token: 0x060040B5 RID: 16565 RVA: 0x001840E4 File Offset: 0x001822E4
        public static void DebugDrawAttackTargetScores_Update()
        {
            IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
            if (attackTargetSearcher == null)
            {
                return;
            }
            if (attackTargetSearcher.Thing.Map != Find.CurrentMap)
            {
                return;
            }
            Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
            if (currentEffectiveVerb == null)
            {
                return;
            }
            AttackTargetFinder.tmpTargets.Clear();
            List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
            for (int i = 0; i < list.Count; i++)
            {
                AttackTargetFinder.tmpTargets.Add((IAttackTarget)list[i]);
            }
            List<Pair<IAttackTarget, float>> availableShootingTargetsByScore =
                AttackTargetFinder.GetAvailableShootingTargetsByScore(AttackTargetFinder.tmpTargets, attackTargetSearcher,
                    currentEffectiveVerb);
            for (int j = 0; j < availableShootingTargetsByScore.Count; j++)
            {
                GenDraw.DrawLineBetween(attackTargetSearcher.Thing.DrawPos, availableShootingTargetsByScore[j].First.Thing.DrawPos);
            }
        }

        // Token: 0x060040B6 RID: 16566 RVA: 0x001841C0 File Offset: 0x001823C0
        public static void DebugDrawAttackTargetScores_OnGUI()
        {
            IAttackTargetSearcher attackTargetSearcher = Find.Selector.SingleSelectedThing as IAttackTargetSearcher;
            if (attackTargetSearcher == null)
            {
                return;
            }
            if (attackTargetSearcher.Thing.Map != Find.CurrentMap)
            {
                return;
            }
            Verb currentEffectiveVerb = attackTargetSearcher.CurrentEffectiveVerb;
            if (currentEffectiveVerb == null)
            {
                return;
            }
            List<Thing> list = attackTargetSearcher.Thing.Map.listerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
            Text.Anchor = TextAnchor.MiddleCenter;
            Text.Font = GameFont.Tiny;
            for (int i = 0; i < list.Count; i++)
            {
                Thing thing = list[i];
                if (thing != attackTargetSearcher)
                {
                    string text;
                    Color red;
                    if (!AttackTargetFinder.CanShootAtFromCurrentPosition((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb)
                    )
                    {
                        text = "out of range";
                        red = Color.red;
                    }
                    else
                    {
                        text = AttackTargetFinder.GetShootingTargetScore((IAttackTarget)thing, attackTargetSearcher, currentEffectiveVerb)
                            .ToString("F0");
                        red = new Color(0.25f, 1f, 0.25f);
                    }
                    GenMapUI.DrawThingLabel(thing.DrawPos.MapToUIPosition(), text, red);
                }
            }
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }

        // Token: 0x060040B7 RID: 16567 RVA: 0x001842BC File Offset: 0x001824BC
        public static bool IsAutoTargetable(IAttackTarget target)
        {
            CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
            if (compCanBeDormant != null && !compCanBeDormant.Awake)
            {
                return false;
            }
            CompInitiatable compInitiatable = target.Thing.TryGetComp<CompInitiatable>();
            return compInitiatable == null || compInitiatable.Initiated;
        }

        // Token: 0x04002C80 RID: 11392
        private const float FriendlyFireScoreOffsetPerHumanlikeOrMechanoid = 18f;

        // Token: 0x04002C81 RID: 11393
        private const float FriendlyFireScoreOffsetPerAnimal = 7f;

        // Token: 0x04002C82 RID: 11394
        private const float FriendlyFireScoreOffsetPerNonPawn = 10f;

        // Token: 0x04002C83 RID: 11395
        private const float FriendlyFireScoreOffsetSelf = 40f;

        // Token: 0x04002C84 RID: 11396
        private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

        // Token: 0x04002C85 RID: 11397
        private static List<Pair<IAttackTarget, float>> availableShootingTargets = new List<Pair<IAttackTarget, float>>();

        // Token: 0x04002C86 RID: 11398
        private static List<float> tmpTargetScores = new List<float>();

        // Token: 0x04002C87 RID: 11399
        private static List<bool> tmpCanShootAtTarget = new List<bool>();

        // Token: 0x04002C88 RID: 11400
        private static List<IntVec3> tempDestList = new List<IntVec3>();

        // Token: 0x04002C89 RID: 11401
        private static List<IntVec3> tempSourceList = new List<IntVec3>();
    }
}
