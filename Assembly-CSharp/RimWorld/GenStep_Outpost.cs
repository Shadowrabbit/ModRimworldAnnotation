// Decompiled with JetBrains decompiler
// Type: RimWorld.GenStep_Outpost
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: E:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimworldFactionalWar\Source\ModRimworldFactionalWar\Lib\Assembly-CSharp.dll

using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
  public class GenStep_Outpost : GenStep
  {
    public int size = 16;
    public FloatRange defaultPawnGroupPointsRange = SymbolResolver_Settlement.DefaultPawnsPoints;
    private static List<CellRect> possibleRects = new List<CellRect>();

    public override int SeedPart => 398638181;

    public override void Generate(Map map, GenStepParams parms)
    {
      CellRect var1;
      //感兴趣的区域不存在
      if (!MapGenerator.TryGetVar("RectOfInterest", out var1))
        //从中心取个矩形
        var1 = CellRect.SingleCell(map.Center);
      //使用中的区域
      List<CellRect> var2;
      if (!MapGenerator.TryGetVar("UsedRects", out var2))
      {
        var2 = new List<CellRect>();
        MapGenerator.SetVar("UsedRects", var2);
      }
      //派系
      Faction faction = map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer ? Find.FactionManager.RandomEnemyFaction() : map.ParentFaction;
      ResolveParams resolveParams = new ResolveParams();
      //获取据点所在的矩形
      resolveParams.rect = GetOutpostRect(var1, var2, map);
      resolveParams.faction = faction;
      resolveParams.edgeDefenseWidth = 2;
      resolveParams.edgeDefenseTurretsCount = Rand.RangeInclusive(0, 1);
      resolveParams.edgeDefenseMortarsCount = 0;
      if (parms.sitePart != null)
      {
        resolveParams.settlementPawnGroupPoints = parms.sitePart.parms.threatPoints;
        resolveParams.settlementPawnGroupSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
      }
      else
        resolveParams.settlementPawnGroupPoints = defaultPawnGroupPointsRange.RandomInRange;
      RimWorld.BaseGen.BaseGen.globalSettings.map = map;
      RimWorld.BaseGen.BaseGen.globalSettings.minBuildings = 1;
      RimWorld.BaseGen.BaseGen.globalSettings.minBarracks = 1;
      RimWorld.BaseGen.BaseGen.symbolStack.Push("settlement", resolveParams);
      if (faction != null && faction == Faction.Empire)
      {
        RimWorld.BaseGen.BaseGen.globalSettings.minThroneRooms = 1;
        RimWorld.BaseGen.BaseGen.globalSettings.minLandingPads = 1;
      }
      RimWorld.BaseGen.BaseGen.Generate();
      if (faction != null && faction == Faction.Empire && RimWorld.BaseGen.BaseGen.globalSettings.landingPadsGenerated == 0)
      {
        CellRect usedRect;
        GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out usedRect);
        var2.Add(usedRect);
      }
      var2.Add(resolveParams.rect);
    }

    private CellRect GetOutpostRect(
      CellRect rectToDefend,
      List<CellRect> usedRects,
      Map map)
    {
      possibleRects.Add(new CellRect(rectToDefend.minX - 1 - size, rectToDefend.CenterCell.z - size / 2, size, size));
      possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - size / 2, size, size));
      possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - size / 2, rectToDefend.minZ - 1 - size, size, size));
      possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - size / 2, rectToDefend.maxZ + 1, size, size));
      CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
      possibleRects.RemoveAll(x => !x.FullyContainedWithin(mapRect));
      if (!possibleRects.Any())
        return rectToDefend;
      IEnumerable<CellRect> source = possibleRects.Where(x => !usedRects.Any(y => x.Overlaps(y)));
      if (!source.Any())
      {
        possibleRects.Add(new CellRect(rectToDefend.minX - 1 - size * 2, rectToDefend.CenterCell.z - size / 2, size, size));
        possibleRects.Add(new CellRect(rectToDefend.maxX + 1 + size, rectToDefend.CenterCell.z - size / 2, size, size));
        possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - size / 2, rectToDefend.minZ - 1 - size * 2, size, size));
        possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - size / 2, rectToDefend.maxZ + 1 + size, size, size));
      }
      return source.Any() ? source.RandomElement() : possibleRects.RandomElement();
    }
  }
}
