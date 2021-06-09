using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001C02 RID: 7170
	public static class DropCellFinder
	{
		// Token: 0x06009DD4 RID: 40404 RVA: 0x002E2F28 File Offset: 0x002E1128
		public static IntVec3 RandomDropSpot(Map map)
		{
			return CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map), map, 1000);
		}

		// Token: 0x06009DD5 RID: 40405 RVA: 0x002E2F60 File Offset: 0x002E1160
		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false));
			IntVec3 position;
			if (building != null)
			{
				position = building.Position;
				IntVec3 result;
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out result, false, false, true, null))
				{
					Log.Error("Could find no good TradeDropSpot near dropCenter " + position + ". Using a random standable unfogged cell.", false);
					result = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Fogged(map), map, 1000);
				}
				return result;
			}
			List<Building> list = new List<Building>();
			list.AddRange(enumerable);
			list.AddRange(collection);
			list.RemoveAll(delegate(Building b)
			{
				CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
				return compPowerTrader != null && !compPowerTrader.PowerOn;
			});
			Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false, true);
			if (!list.Any<Building>())
			{
				list.AddRange(map.listerBuildings.allBuildingsColonist);
				list.Shuffle<Building>();
				if (!list.Any<Building>())
				{
					return CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
			}
			int num = 8;
			for (;;)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (CellFinder.TryFindRandomCellNear(list[i].Position, map, num, validator, out position, -1))
					{
						return position;
					}
				}
				num = Mathf.RoundToInt((float)num * 1.1f);
				if (num > map.Size.x)
				{
					goto Block_9;
				}
			}
			return position;
			Block_9:
			Log.Error("Failed to generate trade drop center. Giving random.", false);
			return CellFinderLoose.RandomCellWith(validator, map, 1000);
		}

		// Token: 0x06009DD6 RID: 40406 RVA: 0x002E3170 File Offset: 0x002E1370
		public static IntVec3 TryFindSafeLandingSpotCloseToColony(Map map, IntVec2 size, Faction faction = null, int borderWidth = 2)
		{
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass3_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.size = size;
			CS$<>8__locals1.faction = faction;
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.size.x = CS$<>8__locals2.size.x + borderWidth;
			DropCellFinder.<>c__DisplayClass3_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.size.z = CS$<>8__locals3.size.z + borderWidth;
			DropCellFinder.tmpColonyBuildings.Clear();
			DropCellFinder.tmpColonyBuildings.AddRange(CS$<>8__locals1.map.listerBuildings.allBuildingsColonist);
			if (!DropCellFinder.tmpColonyBuildings.Any<Building>())
			{
				return CellFinderLoose.RandomCellWith(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSafeLandingSpotCloseToColony>g__SpotValidator|0), CS$<>8__locals1.map, 1000);
			}
			DropCellFinder.tmpColonyBuildings.Shuffle<Building>();
			for (int i = 0; i < DropCellFinder.tmpColonyBuildings.Count; i++)
			{
				IntVec3 intVec;
				if (DropCellFinder.TryFindDropSpotNear(DropCellFinder.tmpColonyBuildings[i].Position, CS$<>8__locals1.map, out intVec, false, false, false, new IntVec2?(CS$<>8__locals1.size)) && DropCellFinder.SkyfallerCanLandAt(intVec, CS$<>8__locals1.map, CS$<>8__locals1.size, CS$<>8__locals1.faction))
				{
					DropCellFinder.tmpColonyBuildings.Clear();
					return intVec;
				}
			}
			DropCellFinder.tmpColonyBuildings.Clear();
			return CellFinderLoose.RandomCellWith(new Predicate<IntVec3>(CS$<>8__locals1.<TryFindSafeLandingSpotCloseToColony>g__SpotValidator|0), CS$<>8__locals1.map, 1000);
		}

		// Token: 0x06009DD7 RID: 40407 RVA: 0x002E32A0 File Offset: 0x002E14A0
		public static bool SkyfallerCanLandAt(IntVec3 c, Map map, IntVec2 size, Faction faction = null)
		{
			if (!DropCellFinder.IsSafeDropSpot(c, map, faction, new IntVec2?(size), 5, 35, 15))
			{
				return false;
			}
			foreach (IntVec3 c2 in GenAdj.OccupiedRect(c, Rot4.North, size))
			{
				List<Thing> thingList = c2.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is IActiveDropPod || thing is Skyfaller)
					{
						return false;
					}
					PlantProperties plant = thing.def.plant;
					if (plant != null && plant.IsTree)
					{
						return false;
					}
					if (thing.def.preventSkyfallersLandingOn)
					{
						return false;
					}
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009DD8 RID: 40408 RVA: 0x002E33AC File Offset: 0x002E15AC
		public static IntVec3 GetBestShuttleLandingSpot(Map map, Faction factionForFindingSpot, out Thing firstBlockingThing)
		{
			IntVec3 result;
			if (!DropCellFinder.TryFindShipLandingArea(map, out result, out firstBlockingThing))
			{
				result = DropCellFinder.TryFindSafeLandingSpotCloseToColony(map, ThingDefOf.Shuttle.Size, factionForFindingSpot, 2);
			}
			if (!result.IsValid && !DropCellFinder.FindSafeLandingSpot(out result, factionForFindingSpot, map, 35, 15, 25, new IntVec2?(ThingDefOf.Shuttle.Size)))
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
				if (!DropCellFinder.TryFindDropSpotNear(intVec, map, out result, false, false, false, new IntVec2?(ThingDefOf.Shuttle.Size)))
				{
					result = intVec;
				}
			}
			return result;
		}

		// Token: 0x06009DD9 RID: 40409 RVA: 0x002E3428 File Offset: 0x002E1628
		public static bool TryFindShipLandingArea(Map map, out IntVec3 result, out Thing firstBlockingThing)
		{
			DropCellFinder.tmpShipLandingAreas.Clear();
			List<ShipLandingArea> landingZones = ShipLandingBeaconUtility.GetLandingZones(map);
			if (landingZones.Any<ShipLandingArea>())
			{
				for (int i = 0; i < landingZones.Count; i++)
				{
					if (landingZones[i].Clear)
					{
						DropCellFinder.tmpShipLandingAreas.Add(landingZones[i]);
					}
				}
				if (DropCellFinder.tmpShipLandingAreas.Any<ShipLandingArea>())
				{
					result = DropCellFinder.tmpShipLandingAreas.RandomElement<ShipLandingArea>().CenterCell;
					firstBlockingThing = null;
					DropCellFinder.tmpShipLandingAreas.Clear();
					return true;
				}
				firstBlockingThing = landingZones[0].FirstBlockingThing;
			}
			else
			{
				firstBlockingThing = null;
			}
			result = IntVec3.Invalid;
			DropCellFinder.tmpShipLandingAreas.Clear();
			return false;
		}

		// Token: 0x06009DDA RID: 40410 RVA: 0x002E34D8 File Offset: 0x002E16D8
		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch, bool allowIndoors = true, IntVec2? size = null)
		{
			DropCellFinder.<>c__DisplayClass8_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass8_0();
			CS$<>8__locals1.size = size;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.allowFogged = allowFogged;
			CS$<>8__locals1.canRoofPunch = canRoofPunch;
			CS$<>8__locals1.allowIndoors = allowIndoors;
			CS$<>8__locals1.center = center;
			if (DebugViewSettings.drawDestSearch)
			{
				CS$<>8__locals1.map.debugDrawer.FlashCell(CS$<>8__locals1.center, 1f, "center", 50);
			}
			CS$<>8__locals1.centerRoom = CS$<>8__locals1.center.GetRoom(CS$<>8__locals1.map, RegionType.Set_Passable);
			CS$<>8__locals1.validator = delegate(IntVec3 c)
			{
				if (CS$<>8__locals1.size != null)
				{
					using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(c, Rot4.North, CS$<>8__locals1.size.Value).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!DropCellFinder.IsGoodDropSpot(enumerator.Current, CS$<>8__locals1.map, CS$<>8__locals1.allowFogged, CS$<>8__locals1.canRoofPunch, CS$<>8__locals1.allowIndoors))
							{
								return false;
							}
						}
						goto IL_93;
					}
				}
				if (!DropCellFinder.IsGoodDropSpot(c, CS$<>8__locals1.map, CS$<>8__locals1.allowFogged, CS$<>8__locals1.canRoofPunch, CS$<>8__locals1.allowIndoors))
				{
					return false;
				}
				IL_93:
				return CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals1.center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly);
			};
			if ((CS$<>8__locals1.allowIndoors & CS$<>8__locals1.canRoofPunch) && CS$<>8__locals1.centerRoom != null && !CS$<>8__locals1.centerRoom.PsychologicallyOutdoors)
			{
				Predicate<IntVec3> v = (IntVec3 c) => CS$<>8__locals1.validator(c) && c.GetRoom(CS$<>8__locals1.map, RegionType.Set_Passable) == CS$<>8__locals1.centerRoom;
				if (CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(v, out result))
				{
					return true;
				}
				Predicate<IntVec3> v2 = delegate(IntVec3 c)
				{
					if (!CS$<>8__locals1.validator(c))
					{
						return false;
					}
					Room room = c.GetRoom(CS$<>8__locals1.map, RegionType.Set_Passable);
					return room != null && !room.PsychologicallyOutdoors;
				};
				if (CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(v2, out result))
				{
					return true;
				}
			}
			return CS$<>8__locals1.<TryFindDropSpotNear>g__TryFindCell|1(CS$<>8__locals1.validator, out result);
		}

		// Token: 0x06009DDB RID: 40411 RVA: 0x002E35D0 File Offset: 0x002E17D0
		public static bool IsGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch, bool allowIndoors = true)
		{
			if (!c.InBounds(map) || !c.Standable(map))
			{
				return false;
			}
			if (!DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors))
			{
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(c, 0f, "phys", 50);
				}
				return false;
			}
			if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is IActiveDropPod || thing is Skyfaller)
				{
					return false;
				}
				if (thing.def.IsEdifice())
				{
					return false;
				}
				if (thing.def.preventSkyfallersLandingOn)
				{
					return false;
				}
				if (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009DDC RID: 40412 RVA: 0x002E36A8 File Offset: 0x002E18A8
		private static bool AnyAdjacentGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			return DropCellFinder.IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch, true) || DropCellFinder.IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch, true);
		}

		// Token: 0x06009DDD RID: 40413 RVA: 0x00069173 File Offset: 0x00067373
		[Obsolete]
		public static IntVec3 FindRaidDropCenterDistant(Map map)
		{
			return DropCellFinder.FindRaidDropCenterDistant_NewTemp(map, false);
		}

		// Token: 0x06009DDE RID: 40414 RVA: 0x002E3710 File Offset: 0x002E1910
		public static IntVec3 FindRaidDropCenterDistant_NewTemp(Map map, bool allowRoofed = false)
		{
			Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
			IEnumerable<Thing> enumerable = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction).Cast<Thing>();
			if (hostFaction == Faction.OfPlayer)
			{
				enumerable = enumerable.Concat(map.listerBuildings.allBuildingsColonist.Cast<Thing>());
			}
			else
			{
				enumerable = enumerable.Concat(from x in map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where x.Faction == hostFaction
				select x);
			}
			int num = 0;
			float num2 = 65f;
			IntVec3 intVec;
			for (;;)
			{
				intVec = CellFinder.RandomCell(map);
				num++;
				if (DropCellFinder.CanPhysicallyDropInto(intVec, map, true, false) && !intVec.Fogged(map))
				{
					if (num > 300)
					{
						break;
					}
					if (allowRoofed || !intVec.Roofed(map))
					{
						num2 -= 0.2f;
						bool flag = false;
						foreach (Thing thing in enumerable)
						{
							if ((float)(intVec - thing.Position).LengthHorizontalSquared < num2 * num2)
							{
								flag = true;
								break;
							}
						}
						if (!flag && map.reachability.CanReachFactionBase(intVec, hostFaction))
						{
							return intVec;
						}
					}
				}
			}
			return intVec;
		}

		// Token: 0x06009DDF RID: 40415 RVA: 0x002E3864 File Offset: 0x002E1A64
		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map, bool canRoofPunch = true, bool allowIndoors = true, bool closeWalk = true, int maxRadius = -1)
		{
			Faction parentFaction = map.ParentFaction;
			if (parentFaction == null)
			{
				return RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map), map, out spot);
			}
			int num = 0;
			Predicate<IntVec3> <>9__1;
			for (;;)
			{
				IntVec3 root = IntVec3.Invalid;
				if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).Count<Pawn>() > 0)
				{
					root = map.mapPawns.FreeHumanlikesSpawnedOfFaction(parentFaction).RandomElement<Pawn>().Position;
				}
				else
				{
					if (parentFaction == Faction.OfPlayer)
					{
						List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
						for (int i = 0; i < allBuildingsColonist.Count; i++)
						{
							if (DropCellFinder.TryFindDropSpotNear(allBuildingsColonist[i].Position, map, out root, true, canRoofPunch, allowIndoors, null))
							{
								break;
							}
						}
					}
					else
					{
						List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						int num2 = 0;
						while (num2 < list.Count && (list[num2].Faction != parentFaction || !DropCellFinder.TryFindDropSpotNear(list[num2].Position, map, out root, true, canRoofPunch, allowIndoors, null)))
						{
							num2++;
						}
					}
					if (!root.IsValid)
					{
						Predicate<IntVec3> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 x) => DropCellFinder.CanPhysicallyDropInto(x, map, canRoofPunch, allowIndoors) && !x.Fogged(map) && x.Standable(map)));
						}
						RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(validator, map, out root);
					}
				}
				int num3 = (maxRadius >= 0) ? maxRadius : 10;
				if (!closeWalk)
				{
					CellFinder.TryFindRandomCellNear(root, map, num3 * num3, null, out spot, 50);
				}
				else
				{
					spot = CellFinder.RandomClosewalkCellNear(root, map, num3, null);
				}
				if (DropCellFinder.CanPhysicallyDropInto(spot, map, canRoofPunch, allowIndoors) && !spot.Fogged(map))
				{
					break;
				}
				num++;
				if (num > 300)
				{
					goto Block_13;
				}
			}
			return true;
			Block_13:
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> validator2;
			if ((validator2 = <>9__2) == null)
			{
				validator2 = (<>9__2 = ((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch, allowIndoors)));
			}
			spot = CellFinderLoose.RandomCellWith(validator2, map, 1000);
			return false;
		}

		// Token: 0x06009DE0 RID: 40416 RVA: 0x002E3AC0 File Offset: 0x002E1CC0
		public static bool FindSafeLandingSpot(out IntVec3 spot, Faction faction, Map map, int distToHostiles = 35, int distToFires = 15, int distToEdge = 25, IntVec2? size = null)
		{
			spot = IntVec3.Invalid;
			int num = 200;
			while (num-- > 0)
			{
				IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
				if (DropCellFinder.IsSafeDropSpot(intVec, map, faction, size, distToEdge, distToHostiles, distToFires))
				{
					spot = intVec;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DE1 RID: 40417 RVA: 0x002E3B0C File Offset: 0x002E1D0C
		public static bool FindSafeLandingSpotNearAvoidingHostiles(Thing thing, Map map, out IntVec3 spot, int distToHostiles = 35, int distToFires = 15, int distToEdge = 25, IntVec2? size = null)
		{
			return RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, map, (IntVec3 s) => DropCellFinder.IsSafeDropSpot(s, map, thing.Faction, size, distToEdge, distToHostiles, distToFires), out spot, 100f, 10f, 50f, true);
		}

		// Token: 0x06009DE2 RID: 40418 RVA: 0x002E3B7C File Offset: 0x002E1D7C
		public static bool CanPhysicallyDropInto(IntVec3 c, Map map, bool canRoofPunch, bool allowedIndoors = true)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			RoofDef roof = c.GetRoof(map);
			if (roof != null)
			{
				if (!canRoofPunch)
				{
					return false;
				}
				if (roof.isThickRoof)
				{
					return false;
				}
			}
			if (!allowedIndoors)
			{
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room != null && !room.PsychologicallyOutdoors)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009DE3 RID: 40419 RVA: 0x002E3BC8 File Offset: 0x002E1DC8
		private static bool IsSafeDropSpot(IntVec3 cell, Map map, Faction faction, IntVec2? size = null, int distToEdge = 25, int distToHostiles = 35, int distToFires = 15)
		{
			DropCellFinder.<>c__DisplayClass17_0 CS$<>8__locals1 = new DropCellFinder.<>c__DisplayClass17_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.cell = cell;
			Faction factionBaseFaction = CS$<>8__locals1.map.ParentFaction ?? Faction.OfPlayer;
			if (size != null)
			{
				using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(CS$<>8__locals1.cell, Rot4.North, size.Value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!DropCellFinder.IsGoodDropSpot(enumerator.Current, CS$<>8__locals1.map, false, false, false))
						{
							return false;
						}
					}
					goto IL_A4;
				}
			}
			if (!DropCellFinder.IsGoodDropSpot(CS$<>8__locals1.cell, CS$<>8__locals1.map, false, false, false))
			{
				return false;
			}
			IL_A4:
			if (distToEdge > 0 && CS$<>8__locals1.cell.CloseToEdge(CS$<>8__locals1.map, distToEdge))
			{
				return false;
			}
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in CS$<>8__locals1.map.attackTargetsCache.TargetsHostileToFaction(faction))
				{
					if (!attackTarget.ThreatDisabled(null) && attackTarget.Thing.Position.InHorDistOf(CS$<>8__locals1.cell, (float)distToHostiles))
					{
						return false;
					}
				}
			}
			if (!CS$<>8__locals1.map.reachability.CanReachFactionBase(CS$<>8__locals1.cell, factionBaseFaction))
			{
				return false;
			}
			if (size != null)
			{
				using (IEnumerator<IntVec3> enumerator3 = CellRect.CenteredOn(CS$<>8__locals1.cell, size.Value.x, size.Value.z).Cells.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						IntVec3 c = enumerator3.Current;
						if (CS$<>8__locals1.<IsSafeDropSpot>g__CellHasCrops|0(c))
						{
							return false;
						}
					}
					goto IL_1CC;
				}
			}
			if (CS$<>8__locals1.<IsSafeDropSpot>g__CellHasCrops|0(CS$<>8__locals1.cell))
			{
				return false;
			}
			IL_1CC:
			CS$<>8__locals1.minDistToFiresSq = (float)(distToFires * distToFires);
			CS$<>8__locals1.closestDistSq = float.MaxValue;
			CS$<>8__locals1.firesCount = 0;
			RegionTraverser.BreadthFirstTraverse(CS$<>8__locals1.cell, CS$<>8__locals1.map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Fire);
				for (int i = 0; i < list.Count; i++)
				{
					float num = (float)CS$<>8__locals1.cell.DistanceToSquared(list[i].Position);
					if (num <= CS$<>8__locals1.minDistToFiresSq)
					{
						if (num < CS$<>8__locals1.closestDistSq)
						{
							CS$<>8__locals1.closestDistSq = num;
						}
						int firesCount = CS$<>8__locals1.firesCount;
						CS$<>8__locals1.firesCount = firesCount + 1;
					}
				}
				return CS$<>8__locals1.closestDistSq <= CS$<>8__locals1.minDistToFiresSq && CS$<>8__locals1.firesCount >= 5;
			}, 15, RegionType.Set_Passable);
			return CS$<>8__locals1.closestDistSq > CS$<>8__locals1.minDistToFiresSq || CS$<>8__locals1.firesCount < 5;
		}

		// Token: 0x04006484 RID: 25732
		private static List<Building> tmpColonyBuildings = new List<Building>();

		// Token: 0x04006485 RID: 25733
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
