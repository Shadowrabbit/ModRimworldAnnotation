// Decompiled with JetBrains decompiler
// Type: RimWorld.DropCellFinder
// Assembly: Assembly-CSharp, Version=1.2.7705.25110, Culture=neutral, PublicKeyToken=null
// MVID: C36F9493-C984-4DDA-A7FB-5C788416098F
// Assembly location: E:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ModRimWorldTouchAnimal\Source\ModRimWorldTouchAnimal\Lib\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
    public static class DropCellFinder
    {
        private static List<Building> tmpColonyBuildings = new List<Building>();
        public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();

        public static IntVec3 RandomDropSpot(Map map) =>
            CellFinderLoose.RandomCellWith(c => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map), map);

        public static IntVec3 TradeDropSpot(Map map)
        {
            IEnumerable<Building> collection =
                map.listerBuildings.allBuildingsColonist.Where(b => b.def.IsCommsConsole);
            IEnumerable<Building> buildings =
                map.listerBuildings.allBuildingsColonist.Where(b => b.def.IsOrbitalTradeBeacon);
            Building building = buildings.FirstOrDefault(b => !map.roofGrid.Roofed(b.Position) &&
                                                              AnyAdjacentGoodDropSpot(b.Position, map, false, false));
            if (building != null)
            {
                IntVec3 position = building.Position;
                IntVec3 result;
                if (!TryFindDropSpotNear(position, map, out result, false, false))
                {
                    Log.Error("Could find no good TradeDropSpot near dropCenter " + position +
                              ". Using a random standable unfogged cell.");
                    result = CellFinderLoose.RandomCellWith(c => c.Standable(map) && !c.Fogged(map), map);
                }
                return result;
            }
            List<Building> list = new List<Building>();
            list.AddRange(buildings);
            list.AddRange(collection);
            list.RemoveAll(b =>
            {
                CompPowerTrader comp = b.TryGetComp<CompPowerTrader>();
                return comp != null && !comp.PowerOn;
            });
            Predicate<IntVec3> validator = c => IsGoodDropSpot(c, map, false, false);
            if (!list.Any())
            {
                list.AddRange(map.listerBuildings.allBuildingsColonist);
                list.Shuffle();
                if (!list.Any())
                    return CellFinderLoose.RandomCellWith(validator, map);
            }
            int squareRadius = 8;
            do
            {
                for (int index = 0; index < list.Count; ++index)
                {
                    IntVec3 result;
                    if (CellFinder.TryFindRandomCellNear(list[index].Position, map, squareRadius, validator, out result))
                        return result;
                }
                squareRadius = Mathf.RoundToInt((float)squareRadius * 1.1f);
            } while (squareRadius <= map.Size.x);
            Log.Error("Failed to generate trade drop center. Giving random.");
            return CellFinderLoose.RandomCellWith(validator, map);
        }

        public static IntVec3 TryFindSafeLandingSpotCloseToColony(
            Map map,
            IntVec2 size,
            Faction faction = null,
            int borderWidth = 2)
        {
            size.x += borderWidth;
            size.z += borderWidth;
            tmpColonyBuildings.Clear();
            tmpColonyBuildings.AddRange(map.listerBuildings.allBuildingsColonist);
            if (!tmpColonyBuildings.Any())
                return CellFinderLoose.RandomCellWith(SpotValidator, map);
            tmpColonyBuildings.Shuffle();
            for (int index = 0; index < tmpColonyBuildings.Count; ++index)
            {
                IntVec3 result;
                if (TryFindDropSpotNear(tmpColonyBuildings[index].Position, map, out result, false, false,
                    false, size) && SkyfallerCanLandAt(result, map, size, faction))
                {
                    tmpColonyBuildings.Clear();
                    return result;
                }
            }
            tmpColonyBuildings.Clear();
            return CellFinderLoose.RandomCellWith(SpotValidator, map);

            bool SpotValidator(IntVec3 c)
            {
                if (!SkyfallerCanLandAt(c, map, size, faction))
                    return false;
                if (ModsConfig.RoyaltyActive)
                {
                    List<Thing> thingList = map.listerThings.ThingsOfDef(ThingDefOf.ActivatorProximity);
                    for (int index = 0; index < thingList.Count; ++index)
                    {
                        if (thingList[index].Faction != null && thingList[index].Faction.HostileTo(faction))
                        {
                            CompSendSignalOnPawnProximity comp = thingList[index].TryGetComp<CompSendSignalOnPawnProximity>();
                            if (comp != null && c.InHorDistOf(thingList[index].Position, comp.Props.radius + 10f))
                                return false;
                        }
                    }
                }
                return true;
            }
        }

        public static bool SkyfallerCanLandAt(IntVec3 c, Map map, IntVec2 size, Faction faction = null)
        {
            if (!IsSafeDropSpot(c, map, faction, size, 5))
                return false;
            foreach (IntVec3 c1 in GenAdj.OccupiedRect(c, Rot4.North, size))
            {
                List<Thing> thingList = c1.GetThingList(map);
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    switch (thing)
                    {
                        case IActiveDropPod _:
                        case Skyfaller _:
                            return false;
                        default:
                            PlantProperties plant = thing.def.plant;
                            if (plant != null && plant.IsTree || thing.def.preventSkyfallersLandingOn ||
                                (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building))
                                return false;
                            continue;
                    }
                }
            }
            return true;
        }

        public static IntVec3 GetBestShuttleLandingSpot(
            Map map,
            Faction factionForFindingSpot,
            out Thing firstBlockingThing)
        {
            IntVec3 intVec3;
            if (!TryFindShipLandingArea(map, out intVec3, out firstBlockingThing))
                intVec3 = TryFindSafeLandingSpotCloseToColony(map, ThingDefOf.Shuttle.Size, factionForFindingSpot);
            if (!intVec3.IsValid && !FindSafeLandingSpot(out intVec3, factionForFindingSpot, map,
                size: ThingDefOf.Shuttle.Size))
            {
                IntVec3 center = RandomDropSpot(map);
                if (!TryFindDropSpotNear(center, map, out intVec3, false, false, false,
                    ThingDefOf.Shuttle.Size))
                    intVec3 = center;
            }
            return intVec3;
        }

        public static bool TryFindShipLandingArea(
            Map map,
            out IntVec3 result,
            out Thing firstBlockingThing)
        {
            tmpShipLandingAreas.Clear();
            List<ShipLandingArea> landingZones = ShipLandingBeaconUtility.GetLandingZones(map);
            if (landingZones.Any())
            {
                for (int index = 0; index < landingZones.Count; ++index)
                {
                    if (landingZones[index].Clear)
                        tmpShipLandingAreas.Add(landingZones[index]);
                }
                if (tmpShipLandingAreas.Any())
                {
                    result = tmpShipLandingAreas.RandomElement().CenterCell;
                    firstBlockingThing = null;
                    tmpShipLandingAreas.Clear();
                    return true;
                }
                firstBlockingThing = landingZones[0].FirstBlockingThing;
            }
            else
                firstBlockingThing = null;
            result = IntVec3.Invalid;
            tmpShipLandingAreas.Clear();
            return false;
        }

        public static bool TryFindDropSpotNear(
            IntVec3 center,
            Map map,
            out IntVec3 result,
            bool allowFogged,
            bool canRoofPunch,
            bool allowIndoors = true,
            IntVec2? size = null)
        {
            if (DebugViewSettings.drawDestSearch)
                map.debugDrawer.FlashCell(center, 1f, nameof(center));
            Room centerRoom = center.GetRoom(map);
            Predicate<IntVec3> validator = c =>
            {
                if (size.HasValue)
                {
                    foreach (IntVec3 c1 in GenAdj.OccupiedRect(c, Rot4.North, size.Value))
                    {
                        if (!IsGoodDropSpot(c1, map, allowFogged, canRoofPunch, allowIndoors))
                            return false;
                    }
                }
                else if (!IsGoodDropSpot(c, map, allowFogged, canRoofPunch, allowIndoors))
                    return false;
                return map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly);
            };
            if (allowIndoors & canRoofPunch && centerRoom != null && !centerRoom.PsychologicallyOutdoors)
            {
                if (TryFindCell(c => validator(c) && c.GetRoom(map) == centerRoom, out result))
                    return true;
                if (TryFindCell(c =>
                {
                    if (!validator(c))
                        return false;
                    Room room = c.GetRoom(map);
                    return room != null && !room.PsychologicallyOutdoors;
                }, out result))
                    return true;
            }
            return TryFindCell(validator, out result);

            bool TryFindCell(Predicate<IntVec3> v, out IntVec3 r)
            {
                int squareRadius = 5;
                while (!CellFinder.TryFindRandomCellNear(center, map, squareRadius, v, out r))
                {
                    squareRadius += 3;
                    if (squareRadius > 16)
                    {
                        r = center;
                        return false;
                    }
                }
                return true;
            }
        }

        public static bool IsGoodDropSpot(
            IntVec3 c,
            Map map,
            bool allowFogged,
            bool canRoofPunch,
            bool allowIndoors = true)
        {
            if (!c.InBounds(map) || !c.Standable(map))
                return false;
            if (!CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors))
            {
                if (DebugViewSettings.drawDestSearch)
                    map.debugDrawer.FlashCell(c, text: "phys");
                return false;
            }
            if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
                return false;
            List<Thing> thingList = c.GetThingList(map);
            for (int index = 0; index < thingList.Count; ++index)
            {
                Thing thing = thingList[index];
                if (thing is IActiveDropPod || thing is Skyfaller || (thing.def.IsEdifice() || thing.def.preventSkyfallersLandingOn) ||
                    thing.def.category != ThingCategory.Plant &&
                    GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
                    return false;
            }
            return true;
        }

        private static bool AnyAdjacentGoodDropSpot(
            IntVec3 c,
            Map map,
            bool allowFogged,
            bool canRoofPunch)
        {
            return IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch) ||
                   IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch) ||
                   IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch) ||
                   IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch);
        }

        [Obsolete]
        public static IntVec3 FindRaidDropCenterDistant(Map map) => FindRaidDropCenterDistant_NewTemp(map);

        public static IntVec3 FindRaidDropCenterDistant_NewTemp(Map map, bool allowRoofed = false)
        {
            Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
            IEnumerable<Thing> first = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction);
            IEnumerable<Thing> things;
            things = hostFaction != Faction.OfPlayer
                ? first.Concat(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Where(x => x.Faction == hostFaction))
                : first.Concat(map.listerBuildings.allBuildingsColonist);
            int num1 = 0;
            float num2 = 65f;
            IntVec3 c;
            bool flag;
            do
            {
                do
                {
                    do
                    {
                        c = CellFinder.RandomCell(map);
                        ++num1;
                    } while (!CanPhysicallyDropInto(c, map, true, false) || c.Fogged(map));
                    if (num1 > 300)
                        return c;
                } while (!allowRoofed && c.Roofed(map));
                num2 -= 0.2f;
                flag = false;
                foreach (Thing thing in things)
                {
                    if ((c - thing.Position).LengthHorizontalSquared < num2 * (double)num2)
                    {
                        flag = true;
                        break;
                    }
                }
            } while (flag || !map.reachability.CanReachFactionBase(c, hostFaction));
            return c;
        }

        public static bool TryFindRaidDropCenterClose(
            out IntVec3 spot,
            Map map,
            bool canRoofPunch = true,
            bool allowIndoors = true,
            bool closeWalk = true,
            int maxRadius = -1)
        {
            Faction parentFaction = map.ParentFaction;
            if (parentFaction == null)
                return RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(
                    x => CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) &&
                         x.Standable(map), map, out spot);
            int num = 0;
            do
            {
                IntVec3 result = IntVec3.Invalid;
                if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).Count() > 0)
                {
                    result = map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).RandomElement().Position;
                }
                else
                {
                    if (parentFaction == Faction.OfPlayer)
                    {
                        List<Building> buildingsColonist = map.listerBuildings.allBuildingsColonist;
                        int index = 0;
                        while (index < buildingsColonist.Count && !TryFindDropSpotNear(buildingsColonist[index].Position,
                            map, out result, true, canRoofPunch, allowIndoors))
                            ++index;
                    }
                    else
                    {
                        List<Thing> thingList = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
                        int index = 0;
                        while (index < thingList.Count && (thingList[index].Faction != parentFaction ||
                                                           !TryFindDropSpotNear(thingList[index].Position, map, out result,
                                                               true, canRoofPunch, allowIndoors)))
                            ++index;
                    }
                    if (!result.IsValid)
                        RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(
                            x => CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) &&
                                 !x.Fogged(map) && x.Standable(map), map, out result);
                }
                int radius = maxRadius >= 0 ? maxRadius : 10;
                if (!closeWalk)
                    CellFinder.TryFindRandomCellNear(result, map, radius * radius, null, out spot, 50);
                else
                    spot = CellFinder.RandomClosewalkCellNear(result, map, radius);
                if (CanPhysicallyDropInto(spot, map, canRoofPunch, allowIndoors) && !spot.Fogged(map))
                    return true;
                ++num;
            } while (num <= 300);
            spot = CellFinderLoose.RandomCellWith(
                c => CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors), map);
            return false;
        }

        public static bool FindSafeLandingSpot(
            out IntVec3 spot,
            Faction faction,
            Map map,
            int distToHostiles = 35,
            int distToFires = 15,
            int distToEdge = 25,
            IntVec2? size = null)
        {
            spot = IntVec3.Invalid;
            int num = 200;
            while (num-- > 0)
            {
                IntVec3 cell = RandomDropSpot(map);
                if (IsSafeDropSpot(cell, map, faction, size, distToEdge, distToHostiles, distToFires))
                {
                    spot = cell;
                    return true;
                }
            }
            return false;
        }

        public static bool FindSafeLandingSpotNearAvoidingHostiles(
            Thing thing,
            Map map,
            out IntVec3 spot,
            int distToHostiles = 35,
            int distToFires = 15,
            int distToEdge = 25,
            IntVec2? size = null)
        {
            return RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, map,
                s =>
                    IsSafeDropSpot(s, map, thing.Faction, size, distToEdge, distToHostiles, distToFires), out spot);
        }

        public static bool CanPhysicallyDropInto(
            IntVec3 c,
            Map map,
            bool canRoofPunch,
            bool allowedIndoors = true)
        {
            if (!c.Walkable(map))
                return false;
            RoofDef roof = c.GetRoof(map);
            if (roof != null && (!canRoofPunch || roof.isThickRoof))
                return false;
            if (!allowedIndoors)
            {
                Room room = c.GetRoom(map);
                if (room != null && !room.PsychologicallyOutdoors)
                    return false;
            }
            return true;
        }

        private static bool IsSafeDropSpot(
            IntVec3 cell,
            Map map,
            Faction faction,
            IntVec2? size = null,
            int distToEdge = 25,
            int distToHostiles = 35,
            int distToFires = 15)
        {
            Faction factionBaseFaction = map.ParentFaction ?? Faction.OfPlayer;
            if (size.HasValue)
            {
                foreach (IntVec3 c in GenAdj.OccupiedRect(cell, Rot4.North, size.Value))
                {
                    if (!IsGoodDropSpot(c, map, false, false, false))
                        return false;
                }
            }
            else if (!IsGoodDropSpot(cell, map, false, false, false))
                return false;
            if (distToEdge > 0 && cell.CloseToEdge(map, distToEdge))
                return false;
            if (faction != null)
            {
                foreach (IAttackTarget attackTarget in map.attackTargetsCache.TargetsHostileToFaction(faction))
                {
                    if (!attackTarget.ThreatDisabled(null) &&
                        attackTarget.Thing.Position.InHorDistOf(cell, distToHostiles))
                        return false;
                }
            }
            if (!map.reachability.CanReachFactionBase(cell, factionBaseFaction))
                return false;
            if (size.HasValue)
            {
                foreach (IntVec3 cell1 in CellRect.CenteredOn(cell, size.Value.x, size.Value.z).Cells)
                {
                    if (CellHasCrops(cell1))
                        return false;
                }
            }
            else if (CellHasCrops(cell))
                return false;
            float minDistToFiresSq = distToFires * distToFires;
            float closestDistSq = float.MaxValue;
            int firesCount = 0;
            RegionTraverser.BreadthFirstTraverse(cell, map, (from, to) => true, x =>
            {
                List<Thing> thingList = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
                for (int index = 0; index < thingList.Count; ++index)
                {
                    float squared = cell.DistanceToSquared(thingList[index].Position);
                    if (squared <= (double)minDistToFiresSq)
                    {
                        if (squared < (double)closestDistSq)
                            closestDistSq = squared;
                        ++firesCount;
                    }
                }
                return (double)closestDistSq <= (double)minDistToFiresSq && firesCount >= 5;
            }, 15);
            return closestDistSq > (double)minDistToFiresSq || firesCount < 5;

            bool CellHasCrops(IntVec3 c)
            {
                Plant plant = c.GetPlant(map);
                return plant != null && plant.sown && map.zoneManager.ZoneAt(c) is Zone_Growing;
            }
        }
    }
}
