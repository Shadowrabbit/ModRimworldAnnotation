using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
    // Token: 0x02000E28 RID: 3624
    public static class SiegeBlueprintPlacer
    {
        // Token: 0x0600522B RID: 21035 RVA: 0x000397B9 File Offset: 0x000379B9
        public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction,
            float points)
        {
            center = placeCenter;
            faction = placeFaction;
            foreach (Blueprint_Build blueprint_Build in PlaceCoverBlueprints(map))
            {
                yield return blueprint_Build;
            }

            IEnumerator<Blueprint_Build> enumerator = null;
            foreach (Blueprint_Build blueprint_Build2 in PlaceArtilleryBlueprints(points, map))
            {
                yield return blueprint_Build2;
            }

            enumerator = null;
            yield break;
            yield break;
        }

        // Token: 0x0600522C RID: 21036 RVA: 0x001BD834 File Offset: 0x001BBA34
        private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map,
            ThingDef stuffDef)
        {
            return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null, null, stuffDef).Accepted;
        }

        // Token: 0x0600522D RID: 21037 RVA: 0x000397DE File Offset: 0x000379DE
        private static IEnumerable<Blueprint_Build> PlaceCoverBlueprints(Map map)
        {
            SiegeBlueprintPlacer.placedCoverLocs.Clear();
            ThingDef coverThing;
            ThingDef coverStuff;
            //一半概率是沙包+布 一半概率是路障+铁或者木头
            if (Rand.Chance(0.5f))
            {
                coverThing = ThingDefOf.Sandbags;
                coverStuff = ThingDefOf.Cloth;
            }
            else
            {
                coverThing = ThingDefOf.Barricade;
                coverStuff = (Rand.Chance(0.5f) ? ThingDefOf.Steel : ThingDefOf.WoodLog);
            }

            //覆盖范围 随机2-4
            int numCover = SiegeBlueprintPlacer.NumCoverRange.RandomInRange;
            int num;
            for (int i = 0; i < numCover; i = num + 1)
            {
                //寻找根节点
                IntVec3 bagRoot = SiegeBlueprintPlacer.FindCoverRoot(map, coverThing, coverStuff);
                if (!bagRoot.IsValid)
                {
                    yield break;
                }
                //扩展方向 向着中心
                Rot4 growDir;
                if (bagRoot.x > SiegeBlueprintPlacer.center.x)
                {
                    growDir = Rot4.West;
                }
                else
                {
                    growDir = Rot4.East;
                }

                Rot4 growDirB;
                if (bagRoot.z > SiegeBlueprintPlacer.center.z)
                {
                    growDirB = Rot4.South;
                }
                else
                {
                    growDirB = Rot4.North;
                }
                //水平方向的建筑
                foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.MakeCoverLine(bagRoot, map, growDir,
                    SiegeBlueprintPlacer.CoverLengthRange.RandomInRange, coverThing, coverStuff))
                {
                    yield return blueprint_Build;
                }

                //垂直方向的建筑
                IEnumerator<Blueprint_Build> enumerator = null;
                bagRoot += growDirB.FacingCell;
                foreach (Blueprint_Build blueprint_Build2 in SiegeBlueprintPlacer.MakeCoverLine(bagRoot, map, growDirB,
                    SiegeBlueprintPlacer.CoverLengthRange.RandomInRange, coverThing, coverStuff))
                {
                    yield return blueprint_Build2;
                }

                enumerator = null;
                num = i;
            }

            yield break;
            yield break;
        }

        // Token: 0x0600522E RID: 21038 RVA: 0x000397EE File Offset: 0x000379EE
        private static IEnumerable<Blueprint_Build> MakeCoverLine(IntVec3 root, Map map, Rot4 growDir, int maxLength,
            ThingDef coverThing, ThingDef coverStuff)
        {
            IntVec3 cur = root;
            int i = 0;
            while (i < maxLength &&
                   SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, coverThing, map, coverStuff))
            {
                yield return GenConstruct.PlaceBlueprintForBuild(coverThing, cur, map, Rot4.North,
                    SiegeBlueprintPlacer.faction, coverStuff);
                SiegeBlueprintPlacer.placedCoverLocs.Add(cur);
                cur += growDir.FacingCell;
                int num = i;
                i = num + 1;
            }

            yield break;
        }

        // Token: 0x0600522F RID: 21039 RVA: 0x00039823 File Offset: 0x00037A23
        private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
        {
            IEnumerable<ThingDef> artyDefs = from def in DefDatabase<ThingDef>.AllDefs
                where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
                select def;
            int numArtillery = Mathf.RoundToInt(points / 60f);
            numArtillery = Mathf.Clamp(numArtillery, 1, 2);
            int num;
            for (int i = 0; i < numArtillery; i = num + 1)
            {
                Rot4 random = Rot4.Random;
                ThingDef thingDef = artyDefs.RandomElement<ThingDef>();
                IntVec3 intVec = SiegeBlueprintPlacer.FindArtySpot(thingDef, random, map);
                if (!intVec.IsValid)
                {
                    yield break;
                }

                yield return GenConstruct.PlaceBlueprintForBuild(thingDef, intVec, map, random,
                    SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
                points -= 60f;
                num = i;
            }

            yield break;
        }

        // Token: 0x06005230 RID: 21040 RVA: 0x001BD858 File Offset: 0x001BBA58
        private static IntVec3 FindCoverRoot(Map map, ThingDef coverThing, ThingDef coverStuff)
        {
            //两个半径 并修正边界
            CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
            cellRect.ClipInsideMap(map);
            CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
            cellRect2.ClipInsideMap(map);
            int num = 0;
            for (;;)
            {
                //最多计算200次
                num++;
                if (num > 200)
                {
                    break;
                }

                //随机一个位置
                IntVec3 randomCell = cellRect.RandomCell;
                //这个位置不在内环 并且 可以接触 并且可以放置蓝图
                if (!cellRect2.Contains(randomCell) &&
                    map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell,
                        TraverseMode.NoPassClosedDoors, Danger.Deadly) &&
                    SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, coverThing, map, coverStuff))
                {
                    bool flag = false;
                    for (int i = 0; i < SiegeBlueprintPlacer.placedCoverLocs.Count; i++)
                    {
                        if ((float) (SiegeBlueprintPlacer.placedCoverLocs[i] - randomCell).LengthHorizontalSquared <
                            36f)
                        {
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        return randomCell;
                    }
                }
            }

            return IntVec3.Invalid;
        }

        // Token: 0x06005231 RID: 21041 RVA: 0x001BD92C File Offset: 0x001BBB2C
        private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
        {
            CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
            cellRect.ClipInsideMap(map);
            int num = 0;
            for (;;)
            {
                num++;
                if (num > 200)
                {
                    break;
                }

                IntVec3 randomCell = cellRect.RandomCell;
                if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell,
                        TraverseMode.NoPassClosedDoors, Danger.Deadly) && !randomCell.Roofed(map) &&
                    SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map, ThingDefOf.Steel))
                {
                    return randomCell;
                }
            }

            return IntVec3.Invalid;
        }

        // Token: 0x04003488 RID: 13448
        private static IntVec3 center;

        // Token: 0x04003489 RID: 13449
        private static Faction faction;

        // Token: 0x0400348A RID: 13450
        private static List<IntVec3> placedCoverLocs = new List<IntVec3>();

        // Token: 0x0400348B RID: 13451
        private const int MaxArtyCount = 2;

        // Token: 0x0400348C RID: 13452
        public const float ArtyCost = 60f;

        // Token: 0x0400348D RID: 13453
        private const int MinCoverDistSquared = 36;

        // Token: 0x0400348E RID: 13454
        private static readonly IntRange NumCoverRange = new IntRange(2, 4);

        // Token: 0x0400348F RID: 13455
        private static readonly IntRange CoverLengthRange = new IntRange(2, 7);
    }
}