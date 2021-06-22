// Decompiled with JetBrains decompiler
// Type: RimWorld.BaseGen.SymbolResolver_Settlement
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: E:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimworldFactionalWar\Source\ModRimworldFactionalWar\Lib\Assembly-CSharp.dll

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld.BaseGen
{
    public class SymbolResolver_Settlement : SymbolResolver
    {
        public static readonly FloatRange DefaultPawnsPoints = new FloatRange(1150f, 1600f);

        public override void Resolve(ResolveParams rp)
        {
            //全局缓存的地图
            Map map = RimWorld.BaseGen.BaseGen.globalSettings.map;
            //解决方案中的派系 或者随机一个敌对派系
            Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction();
            //边缘防卫宽度是否有值
            int dist = 0;
            if (rp.edgeDefenseWidth.HasValue)
                dist = rp.edgeDefenseWidth.Value;
            else if (rp.rect.Width >= 20 && rp.rect.Height >= 20 && (faction.def.techLevel >= TechLevel.Industrial || Rand.Bool))
                dist = Rand.Bool ? 2 : 4;
            float f = (float)((double)rp.rect.Area / 144.0 * 0.170000001788139);
            RimWorld.BaseGen.BaseGen.globalSettings.minEmptyNodes = (double)f < 1.0 ? 0 : GenMath.RoundRandom(f);
            //集群AI防卫
            Lord lord = rp.singlePawnLord ??
                        LordMaker.MakeNewLord(faction, (LordJob)new LordJob_DefendBase(faction, rp.rect.CenterCell), map);
            TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
            ResolveParams resolveParams1 = rp;
            resolveParams1.rect = rp.rect;
            resolveParams1.faction = faction;
            resolveParams1.singlePawnLord = lord;
            resolveParams1.pawnGroupKindDef = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Settlement;
            resolveParams1.singlePawnSpawnCellExtraPredicate = rp.singlePawnSpawnCellExtraPredicate ??
                                                               (Predicate<IntVec3>)(x =>
                                                                   map.reachability.CanReachMapEdge(x, traverseParms));
            if (resolveParams1.pawnGroupMakerParams == null)
            {
                resolveParams1.pawnGroupMakerParams = new PawnGroupMakerParms();
                resolveParams1.pawnGroupMakerParams.tile = map.Tile;
                resolveParams1.pawnGroupMakerParams.faction = faction;
                PawnGroupMakerParms groupMakerParams = resolveParams1.pawnGroupMakerParams;
                float? settlementPawnGroupPoints = rp.settlementPawnGroupPoints;
                double num = settlementPawnGroupPoints.HasValue
                    ? (double)settlementPawnGroupPoints.GetValueOrDefault()
                    : (double)SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange;
                groupMakerParams.points = (float)num;
                resolveParams1.pawnGroupMakerParams.inhabitants = true;
                resolveParams1.pawnGroupMakerParams.seed = rp.settlementPawnGroupSeed;
            }
            //生成角色
            RimWorld.BaseGen.BaseGen.symbolStack.Push("pawnGroup", resolveParams1);
            RimWorld.BaseGen.BaseGen.symbolStack.Push("outdoorLighting", rp);
            //泡沫灭火器
            if (faction.def.techLevel >= TechLevel.Industrial)
            {
                int num = Rand.Chance(0.75f) ? GenMath.RoundRandom((float)rp.rect.Area / 400f) : 0;
                for (int index = 0; index < num; ++index)
                {
                    ResolveParams resolveParams2 = rp;
                    resolveParams2.faction = faction;
                    RimWorld.BaseGen.BaseGen.symbolStack.Push("firefoamPopper", resolveParams2);
                }
            }
            bool? nullable1;
            if (dist > 0)
            {
                ResolveParams resolveParams2 = rp;
                resolveParams2.faction = faction;
                resolveParams2.edgeDefenseWidth = new int?(dist);
                ref ResolveParams local = ref resolveParams2;
                nullable1 = rp.edgeThingMustReachMapEdge;
                bool? nullable2 = new bool?(!nullable1.HasValue || nullable1.GetValueOrDefault());
                local.edgeThingMustReachMapEdge = nullable2;
                RimWorld.BaseGen.BaseGen.symbolStack.Push("edgeDefense", resolveParams2);
            }
            ResolveParams resolveParams3 = rp;
            resolveParams3.rect = rp.rect.ContractedBy(dist);
            resolveParams3.faction = faction;
            RimWorld.BaseGen.BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams3);
            ResolveParams resolveParams4 = rp;
            resolveParams4.rect = rp.rect.ContractedBy(dist);
            resolveParams4.faction = faction;
            ref ResolveParams local1 = ref resolveParams4;
            nullable1 = rp.floorOnlyIfTerrainSupports;
            bool? nullable3 = new bool?(!nullable1.HasValue || nullable1.GetValueOrDefault());
            local1.floorOnlyIfTerrainSupports = nullable3;
            RimWorld.BaseGen.BaseGen.symbolStack.Push("basePart_outdoors", resolveParams4);
            ResolveParams resolveParams5 = rp;
            resolveParams5.floorDef = TerrainDefOf.Bridge;
            ref ResolveParams local2 = ref resolveParams5;
            nullable1 = rp.floorOnlyIfTerrainSupports;
            bool? nullable4 = new bool?(!nullable1.HasValue || nullable1.GetValueOrDefault());
            local2.floorOnlyIfTerrainSupports = nullable4;
            ref ResolveParams local3 = ref resolveParams5;
            nullable1 = rp.allowBridgeOnAnyImpassableTerrain;
            bool? nullable5 = new bool?(!nullable1.HasValue || nullable1.GetValueOrDefault());
            local3.allowBridgeOnAnyImpassableTerrain = nullable5;
            RimWorld.BaseGen.BaseGen.symbolStack.Push("floor", resolveParams5);
        }
    }
}
