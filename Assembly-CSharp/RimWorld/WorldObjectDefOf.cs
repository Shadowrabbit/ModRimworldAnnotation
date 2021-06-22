// Decompiled with JetBrains decompiler
// Type: RimWorld.WorldObjectDefOf
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: E:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimworldFactionalWar\Source\ModRimworldFactionalWar\Lib\Assembly-CSharp.dll

namespace RimWorld
{
    [DefOf]
    public static class WorldObjectDefOf
    {
        public static WorldObjectDef Caravan; //商队
        public static WorldObjectDef Settlement; //乡镇
        public static WorldObjectDef AbandonedSettlement; //废弃的定居点
        public static WorldObjectDef EscapeShip; //逃生飞船
        public static WorldObjectDef Ambush; //伏击
        public static WorldObjectDef DestroyedSettlement; //被摧毁的乡镇
        public static WorldObjectDef AttackedNonPlayerCaravan; //被攻击的非玩家商队
        public static WorldObjectDef TravelingTransportPods; //旅行运输舱
        public static WorldObjectDef RoutePlannerWaypoint; //路线规划器航点
        public static WorldObjectDef Site; //地点
        public static WorldObjectDef PeaceTalks; //友好交谈
        [MayRequireRoyalty]
        public static WorldObjectDef TravelingShuttle; //飞行器
        public static WorldObjectDef Debug_Arena;

        static WorldObjectDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof (WorldObjectDefOf));
    }
}
