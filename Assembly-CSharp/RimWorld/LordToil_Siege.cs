// Decompiled with JetBrains decompiler
// Type: RimWorld.LordToil_Siege
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimＷorldFactionalWar\Source\ModRimWorldFactionalWar\Lib\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
    public class LordToil_Siege : LordToil
    {
        public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();
        private const float BaseRadiusMin = 14f;
        private const float BaseRadiusMax = 25f;
        private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);
        private const int StartBuildingDelay = 450;
        private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);
        private const float FractionLossesToAssault = 0.4f;
        private const int InitalShellsPerCannon = 5;
        private const int ReplenishAtShells = 4;
        private const int ShellReplenishCount = 6;
        private const int ReplenishAtMeals = 5;
        private const int MealReplenishCount = 12;

        public override IntVec3 FlagLoc => this.Data.siegeCenter;

        private LordToilData_Siege Data => (LordToilData_Siege) this.data;

        private IEnumerable<Frame> Frames
        {
            get
            {
                LordToil_Siege lordToilSiege = this;
                LordToilData_Siege data = lordToilSiege.Data;
                float radSquared = (float) (((double) data.baseRadius + 10.0) * ((double) data.baseRadius + 10.0));
                List<Thing> framesList = lordToilSiege.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
                if (framesList.Count != 0)
                {
                    for (int i = 0; i < framesList.Count; ++i)
                    {
                        Frame frame = (Frame) framesList[i];
                        if (frame.Faction == lordToilSiege.lord.faction &&
                            (double) (frame.Position - data.siegeCenter).LengthHorizontalSquared < (double) radSquared)
                            yield return frame;
                    }
                }
            }
        }

        public override bool ForceHighStoryDanger => true;

        public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
        {
            this.data = (LordToilData) new LordToilData_Siege();
            //设置集结中心 蓝图点数
            this.Data.siegeCenter = siegeCenter;
            this.Data.blueprintPoints = blueprintPoints;
        }

        public override void Init()
        {
            base.Init();
            //设置半径
            LordToilData_Siege data = this.Data;
            data.baseRadius = Mathf.InverseLerp(14f, 25f, (float) this.lord.ownedPawns.Count / 50f);
            data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
            //需要的建筑材料列表
            List<Thing> source = new List<Thing>();
            //遍历蓝图
            foreach (Blueprint_Build placeBlueprint in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, this.Map,
                this.lord.faction, data.blueprintPoints))
            {
                data.blueprints.Add((Blueprint) placeBlueprint);
                //当前蓝图消耗的材料和数量
                foreach (ThingDefCountClass thingDefCountClass in placeBlueprint.MaterialsNeeded())
                {
                    ThingDefCountClass cost = thingDefCountClass;
                    //列表中包含当前材料
                    Thing thing1 = source.FirstOrDefault<Thing>((Func<Thing, bool>) (t => t.def == cost.thingDef));
                    //添加数量
                    if (thing1 != null)
                    {
                        thing1.stackCount += cost.count;
                    }
                    //创建新的物品加入列表
                    else
                    {
                        Thing thing2 = ThingMaker.MakeThing(cost.thingDef);
                        thing2.stackCount = cost.count;
                        source.Add(thing2);
                    }
                }

                //如果蓝图是炮塔
                if (placeBlueprint.def.entityDefToBuild is ThingDef entityDefToBuild)
                {
                    //随机炮弹
                    ThingDef randomShellDef = TurretGunUtility.TryFindRandomShellDef(entityDefToBuild, false,
                        techLevel: this.lord.faction.def.techLevel, maxMarketValue: 250f);
                    if (randomShellDef != null)
                    {
                        Thing thing = ThingMaker.MakeThing(randomShellDef);
                        thing.stackCount = 5;
                        source.Add(thing);
                    }
                }
            }

            //数量修正 多带最多20%材料作为应急
            for (int index = 0; index < source.Count; ++index)
                source[index].stackCount = Mathf.CeilToInt((float) source[index].stackCount * Rand.Range(1f, 1.2f));
            //根据最终数量计算堆叠
            List<List<Thing>> thingsGroups = new List<List<Thing>>();
            for (int index = 0; index < source.Count; ++index)
            {
                while (source[index].stackCount > source[index].def.stackLimit)
                {
                    int num = Mathf.CeilToInt((float) source[index].def.stackLimit * Rand.Range(0.9f, 0.999f));
                    Thing thing = ThingMaker.MakeThing(source[index].def);
                    thing.stackCount = num;
                    source[index].stackCount -= num;
                    source.Add(thing);
                }
            }

            List<Thing> thingList1 = new List<Thing>();
            for (int index = 0; index < source.Count; ++index)
            {
                thingList1.Add(source[index]);
                //奇数或者最后一项 分为一组
                if (index % 2 == 1 || index == source.Count - 1)
                {
                    
                    thingsGroups.Add(thingList1);
                    thingList1 = new List<Thing>();
                }
            }

            List<Thing> thingList2 = new List<Thing>();
            //每位袭击者的食物补给
            FloatRange floatRange = LordToil_Siege.MealCountRangePerRaider;
            //食物数量
            int num1 = Mathf.RoundToInt(floatRange.RandomInRange * (float) this.lord.ownedPawns.Count);
            for (int index = 0; index < num1; ++index)
            {
                Thing thing = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack);
                thingList2.Add(thing);
            }
            //添加到组
            thingsGroups.Add(thingList2);
            //发送空投
            DropPodUtility.DropThingGroupsNear(data.siegeCenter, this.Map, thingsGroups);
            
            LordToilData_Siege lordToilDataSiege = data;
            double randomInRange = (double) BuilderCountFraction.RandomInRange;
            //设置建造分数需求
            lordToilDataSiege.desiredBuilderFraction = (float) randomInRange;
        }

        public override void UpdateAllDuties()
        {
            LordToilData_Siege data = this.Data;
            //一开始全员防卫
            if (this.lord.ticksInToil < 450)
            {
                for (int index = 0; index < this.lord.ownedPawns.Count; ++index)
                    this.SetAsDefender(this.lord.ownedPawns[index]);
            }
            //随机选一部分角色作为建筑工 另一部分作为防卫
            else
            {
                this.rememberedDuties.Clear();
                int num1 = Mathf.RoundToInt((float) this.lord.ownedPawns.Count * data.desiredBuilderFraction);
                if (num1 <= 0)
                    num1 = 1;
                int num2 = this.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
                    .Where<Thing>((Func<Thing, bool>) (b =>
                        b.def.hasInteractionCell && b.Faction == this.lord.faction &&
                        b.Position.InHorDistOf(this.FlagLoc, data.baseRadius))).Count<Thing>();
                if (num1 < num2)
                    num1 = num2;
                int num3 = 0;
                for (int index = 0; index < this.lord.ownedPawns.Count; ++index)
                {
                    Pawn ownedPawn = this.lord.ownedPawns[index];
                    if (ownedPawn.mindState.duty.def == DutyDefOf.Build)
                    {
                        this.rememberedDuties.Add(ownedPawn, DutyDefOf.Build);
                        this.SetAsBuilder(ownedPawn);
                        ++num3;
                    }
                }

                int num4 = num1 - num3;
                for (int index = 0; index < num4; ++index)
                {
                    Pawn result;
                    if (this.lord.ownedPawns
                        .Where<Pawn>((Func<Pawn, bool>) (pa =>
                            !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)))
                        .TryRandomElement<Pawn>(out result))
                    {
                        this.rememberedDuties.Add(result, DutyDefOf.Build);
                        this.SetAsBuilder(result);
                        ++num3;
                    }
                }

                for (int index = 0; index < this.lord.ownedPawns.Count; ++index)
                {
                    Pawn ownedPawn = this.lord.ownedPawns[index];
                    if (!this.rememberedDuties.ContainsKey(ownedPawn))
                    {
                        this.SetAsDefender(ownedPawn);
                        this.rememberedDuties.Add(ownedPawn, DutyDefOf.Defend);
                    }
                }

                if (num3 != 0)
                    return;
                this.lord.ReceiveMemo("NoBuilders");
            }
        }

        public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
        {
            this.UpdateAllDuties();
            base.Notify_PawnLost(victim, cond);
        }

        public override void Notify_ConstructionFailed(
            Pawn pawn,
            Frame frame,
            Blueprint_Build newBlueprint)
        {
            base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
            if (frame.Faction != this.lord.faction || newBlueprint == null)
                return;
            this.Data.blueprints.Add((Blueprint) newBlueprint);
        }

        private bool CanBeBuilder(Pawn p) => !p.WorkTypeIsDisabled(WorkTypeDefOf.Construction) &&
                                             !p.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter);

        private void SetAsBuilder(Pawn p)
        {
            LordToilData_Siege data = this.Data;
            p.mindState.duty = new PawnDuty(DutyDefOf.Build, (LocalTargetInfo) data.siegeCenter);
            p.mindState.duty.radius = data.baseRadius;
            int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite,
                ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
            p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
            p.workSettings.EnableAndInitialize();
            List<WorkTypeDef> defsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                WorkTypeDef w = defsListForReading[index];
                if (w == WorkTypeDefOf.Construction)
                    p.workSettings.SetPriority(w, 1);
                else
                    p.workSettings.Disable(w);
            }
        }

        private void SetAsDefender(Pawn p)
        {
            LordToilData_Siege data = this.Data;
            p.mindState.duty = new PawnDuty(DutyDefOf.Defend, (LocalTargetInfo) data.siegeCenter);
            p.mindState.duty.radius = data.baseRadius;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void LordToilTick()
        {
            base.LordToilTick();
            var lordToilDataSiege = Data;
            if (lord.ticksInToil == 450)
                lord.CurLordToil.UpdateAllDuties();
            if (lord.ticksInToil > 450 && lord.ticksInToil % 500 == 0)
                UpdateAllDuties();
            if (Find.TickManager.TicksGame % 500 != 0)
                return;
            //炮塔被摧毁
            if (Frames.All(frame => frame.Destroyed) &&
                (!lordToilDataSiege.blueprints
                    .Any(blue => !blue.Destroyed) && !Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
                    .Any(b =>
                        b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery"))))
            {
                lord.ReceiveMemo("NoArtillery");
            }
            else
            {
                int num1 = GenRadial.NumCellsInRadius(20f);
                int num2 = 0;
                int num3 = 0;
                for (int index1 = 0; index1 < num1; ++index1)
                {
                    IntVec3 c = lordToilDataSiege.siegeCenter + GenRadial.RadialPattern[index1];
                    if (c.InBounds(this.Map))
                    {
                        List<Thing> thingList = c.GetThingList(this.Map);
                        for (int index2 = 0; index2 < thingList.Count; ++index2)
                        {
                            if (thingList[index2].def.IsShell)
                                num2 += thingList[index2].stackCount;
                            if (thingList[index2].def == ThingDefOf.MealSurvivalPack)
                                num3 += thingList[index2].stackCount;
                        }
                    }
                }

                if (num2 < 4)
                {
                    ThingDef randomShellDef = TurretGunUtility.TryFindRandomShellDef(ThingDefOf.Turret_Mortar, false,
                        techLevel: this.lord.faction.def.techLevel, maxMarketValue: 250f);
                    if (randomShellDef != null)
                        this.DropSupplies(randomShellDef, 6);
                }

                if (num3 >= 5)
                    return;
                this.DropSupplies(ThingDefOf.MealSurvivalPack, 12);
            }
        }

        /// <summary>
        /// 空投支援
        /// </summary>
        /// <param name="thingDef"></param>
        /// <param name="count"></param>
        private void DropSupplies(ThingDef thingDef, int count)
        {
            List<Thing> thingList = new List<Thing>();
            Thing thing = ThingMaker.MakeThing(thingDef);
            thing.stackCount = count;
            thingList.Add(thing);
            DropPodUtility.DropThingsNear(this.Data.siegeCenter, this.Map, (IEnumerable<Thing>) thingList);
        }

        public override void Cleanup()
        {
            LordToilData_Siege data = this.Data;
            data.blueprints.RemoveAll((Predicate<Blueprint>) (blue => blue.Destroyed));
            for (int index = 0; index < data.blueprints.Count; ++index)
                data.blueprints[index].Destroy(DestroyMode.Cancel);
            foreach (Thing thing in this.Frames.ToList<Frame>())
                thing.Destroy(DestroyMode.Cancel);
            foreach (Thing ownedBuilding in this.lord.ownedBuildings)
                ownedBuilding.SetFaction((Faction) null);
        }
    }
}