using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
    /// <summary>
    /// 集群AI 突击殖民者
    /// </summary>
    public class LordJob_AssaultColony : LordJob
    {
        /// <summary>
        /// 被击倒有罪
        /// </summary>
        public override bool GuiltyOnDowned
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06004FED RID: 20461 RVA: 0x000381A8 File Offset: 0x000363A8
        public LordJob_AssaultColony()
        {
        }

        // Token: 0x06004FEE RID: 20462 RVA: 0x001B5B00 File Offset: 0x001B3D00
        public LordJob_AssaultColony(SpawnedPawnParams parms)
        {
            assaulterFaction = parms.spawnerThing.Faction;
            canKidnap = false;
            canTimeoutOrFlee = false;
            canSteal = false;
        }

        // Token: 0x06004FEF RID: 20463 RVA: 0x001B5B50 File Offset: 0x001B3D50
        public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false,
            bool useAvoidGridSmart = false, bool canSteal = true)
        {
            this.assaulterFaction = assaulterFaction;
            this.canKidnap = canKidnap;
            this.canTimeoutOrFlee = canTimeoutOrFlee;
            this.sappers = sappers;
            this.useAvoidGridSmart = useAvoidGridSmart;
            this.canSteal = canSteal;
        }

        /// <summary>
        /// 创建状态图
        /// </summary>
        /// <returns></returns>
        public override StateGraph CreateGraph()
        {
            var stateGraph = new StateGraph();
            LordToil lordToil = null;
            //工兵流程
            if (sappers)
            {
                //工兵行为流程
                lordToil = new LordToil_AssaultColonySappers();
                //使用智能躲避格子
                if (useAvoidGridSmart)
                {
                    lordToil.useAvoidGrid = true;
                }
                //加入状态图
                stateGraph.AddToil(lordToil);
                //过渡
                var transition = new Transition(lordToil, lordToil, true);
                //触发器 角色丢失
                transition.AddTrigger(new Trigger_PawnLost());
                //添加过渡
                stateGraph.AddTransition(transition);
            }
            //突击行为流程
            LordToil lordToil2 = new LordToil_AssaultColony();
            if (useAvoidGridSmart)
            {
                lordToil2.useAvoidGrid = true;
            }
            stateGraph.AddToil(lordToil2);
            //离开地图流程
            var lordToilExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true)
            {
                useAvoidGrid = true
            };
            stateGraph.AddToil(lordToilExitMap);
            if (sappers)
            {
                //过渡 工兵流程 到 突击流程
                var transition2 = new Transition(lordToil, lordToil2);
                //触发器 不存在作战工兵
                transition2.AddTrigger(new Trigger_NoFightingSappers());
                stateGraph.AddTransition(transition2);
            }
            //突击者派系是类人派系
            if (assaulterFaction.def.humanlikeFaction)
            {
                //可以超时或逃离
                if (canTimeoutOrFlee)
                {
                    //过渡 突击流程 到 离开地图流程
                    var transition3 = new Transition(lordToil2, lordToilExitMap);
                    //工兵流程存在
                    if (lordToil != null)
                    {
                        //储存工兵流程
                        transition3.AddSource(lordToil);
                    }
                    //添加过渡触发器 超时 
                    transition3.AddTrigger(new Trigger_TicksPassed(sappers
                        ? SapTimeBeforeGiveUp.RandomInRange
                        : AssaultTimeBeforeGiveUp.RandomInRange));
                    //触发前回调
                    transition3.AddPreAction(new TransitionAction_Message(
                        "MessageRaidersGivenUpLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
                            this.assaulterFaction.Name), null, 1f));
                    stateGraph.AddTransition(transition3);
                    //过渡 突击流程 到 离开地图流程
                    var transition4 = new Transition(lordToil2, lordToilExitMap);
                    if (lordToil != null)
                    {
                        transition4.AddSource(lordToil);
                    }
                    //伤害承受比例 取0.25到0.35随机数
                    var floatRange = new FloatRange(0.25f, 0.35f);
                    var randomInRange = floatRange.RandomInRange;
                    //触发器 达到承受比例 或超过900点
                    transition4.AddTrigger(new Trigger_FractionColonyDamageTaken(randomInRange, 900f));
                    //突击者造成破坏 心满意足离开
                    transition4.AddPreAction(new TransitionAction_Message(
                        "MessageRaidersSatisfiedLeaving".Translate(assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
                            assaulterFaction.Name)));
                    stateGraph.AddTransition(transition4);
                }
                //允许绑架
                if (canKidnap)
                {
                    var startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
                    var transition5 = new Transition(lordToil2, startingToil, false, true);
                    if (lordToil != null)
                    {
                        transition5.AddSource(lordToil);
                    }
                    transition5.AddPreAction(new TransitionAction_Message(
                        "MessageRaidersKidnapping".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
                            this.assaulterFaction.Name), null, 1f));
                    transition5.AddTrigger(new Trigger_KidnapVictimPresent());
                    stateGraph.AddTransition(transition5, false);
                }
                //允许偷窃
                if (canSteal)
                {
                    
                    var startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
                    //过渡 突击 到 偷窃
                    var transition6 = new Transition(lordToil2, startingToil2, false, true);
                    if (lordToil != null)
                    {
                        transition6.AddSource(lordToil);
                    }
                    transition6.AddPreAction(new TransitionAction_Message(
                        "MessageRaidersStealing".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(),
                            this.assaulterFaction.Name), null, 1f));
                    //触发方式 有高价值物品在附近 300个tick内集群无成员受伤
                    transition6.AddTrigger(new Trigger_HighValueThingsAround());
                    stateGraph.AddTransition(transition6, false);
                }
            }
            //过渡 突击流程 到 离开地图流程
            var transition7 = new Transition(lordToil2, lordToilExitMap, false, true);
            if (lordToil != null)
            {
                transition7.AddSource(lordToil);
            }
            //触发方式 阵营关系变为非敌对
            transition7.AddTrigger(new Trigger_BecameNonHostileToPlayer());
            transition7.AddPreAction(new TransitionAction_Message(
                "MessageRaidersLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name),
                null, 1f));
            stateGraph.AddTransition(transition7, false);
            return stateGraph;
        }

        // Token: 0x06004FF1 RID: 20465 RVA: 0x001B5F5C File Offset: 0x001B415C
        public override void ExposeData()
        {
            Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
            Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
            Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
            Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
            Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
            Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
        }

        // Token: 0x040033A2 RID: 13218
        private Faction assaulterFaction;

        // Token: 0x040033A3 RID: 13219
        private bool canKidnap = true;

        // Token: 0x040033A4 RID: 13220
        private bool canTimeoutOrFlee = true;

        // Token: 0x040033A5 RID: 13221
        private bool sappers;

        // Token: 0x040033A6 RID: 13222
        private bool useAvoidGridSmart;

        // Token: 0x040033A7 RID: 13223
        private bool canSteal = true;

        // Token: 0x040033A8 RID: 13224
        private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

        // Token: 0x040033A9 RID: 13225
        private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);
    }
}
