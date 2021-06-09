using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001286 RID: 4742
	public class GenStep_Power : GenStep
	{
		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x0600674E RID: 26446 RVA: 0x000468AF File Offset: 0x00044AAF
		public override int SeedPart
		{
			get
			{
				return 1186199651;
			}
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x001FC888 File Offset: 0x001FAA88
		public override void Generate(Map map, GenStepParams parms)
		{
			map.skyManager.ForceSetCurSkyGlow(1f);
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.UpdateDesiredPowerOutputForAllGenerators(map);
			this.EnsureBatteriesConnectedAndMakeSense(map);
			this.EnsurePowerUsersConnected(map);
			this.EnsureGeneratorsConnectedAndMakeSense(map);
			this.tmpThings.Clear();
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x001FC8D8 File Offset: 0x001FAAD8
		private void UpdateDesiredPowerOutputForAllGenerators(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerGenerator(this.tmpThings[i]))
				{
					CompPowerPlant compPowerPlant = this.tmpThings[i].TryGetComp<CompPowerPlant>();
					if (compPowerPlant != null)
					{
						compPowerPlant.UpdateDesiredPowerOutput();
					}
				}
			}
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x001FC950 File Offset: 0x001FAB50
		private void EnsureBatteriesConnectedAndMakeSense(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				CompPowerBattery compPowerBattery = this.tmpThings[i].TryGetComp<CompPowerBattery>();
				if (compPowerBattery != null)
				{
					PowerNet powerNet = compPowerBattery.PowerNet;
					if (powerNet == null || !this.HasAnyPowerGenerator(powerNet))
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2;
						IntVec3 dest;
						Building building2;
						if (this.TryFindClosestReachableNet(compPowerBattery.parent.Position, (PowerNet x) => this.HasAnyPowerGenerator(x), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							if (this.canSpawnPowerGenerators)
							{
								int count = this.tmpCells.Count;
								Building building;
								if (Rand.Chance(Mathf.InverseLerp((float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.min, (float)GenStep_Power.MaxDistanceBetweenBatteryAndTransmitter.max, (float)count)) && this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building))
								{
									this.SpawnTransmitters(compPowerBattery.parent.Position, building.Position, map, compPowerBattery.parent.Faction);
									powerNet2 = null;
								}
							}
							if (powerNet2 != null)
							{
								this.SpawnTransmitters(this.tmpCells, map, compPowerBattery.parent.Faction);
							}
						}
						else if (this.canSpawnPowerGenerators && this.TrySpawnPowerGeneratorNear(compPowerBattery.parent.Position, map, compPowerBattery.parent.Faction, out building2))
						{
							this.SpawnTransmitters(compPowerBattery.parent.Position, building2.Position, map, compPowerBattery.parent.Faction);
						}
					}
				}
			}
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x001FCAFC File Offset: 0x001FACFC
		private void EnsurePowerUsersConnected(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			this.hasAtleast1TurretInt = this.tmpThings.Any((Thing t) => t is Building_Turret);
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerUser(this.tmpThings[i]))
				{
					CompPowerTrader powerComp = this.tmpThings[i].TryGetComp<CompPowerTrader>();
					PowerNet powerNet = powerComp.PowerNet;
					if (powerNet != null && powerNet.hasPowerSource)
					{
						this.TryTurnOnImmediately(powerComp, map);
					}
					else
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2;
						IntVec3 dest;
						Building building;
						if (this.TryFindClosestReachableNet(powerComp.parent.Position, (PowerNet x) => x.CurrentEnergyGainRate() - powerComp.Props.basePowerConsumption * CompPower.WattsToWattDaysPerTick > 1E-07f, map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							bool flag = false;
							if (this.canSpawnPowerGenerators && this.tmpThings[i] is Building_Turret && this.tmpCells.Count > 13)
							{
								flag = this.TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(this.tmpThings[i], map);
							}
							if (!flag)
							{
								this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
							}
							this.TryTurnOnImmediately(powerComp, map);
						}
						else if (this.canSpawnPowerGenerators && this.TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(this.tmpThings[i], map))
						{
							this.TryTurnOnImmediately(powerComp, map);
						}
						else if (this.TryFindClosestReachableNet(powerComp.parent.Position, (PowerNet x) => x.CurrentStoredEnergy() > 1E-07f, map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
						else if (this.canSpawnBatteries && this.TrySpawnBatteryNear(this.tmpThings[i].Position, map, this.tmpThings[i].Faction, out building))
						{
							this.SpawnTransmitters(this.tmpThings[i].Position, building.Position, map, this.tmpThings[i].Faction);
							if (building.GetComp<CompPowerBattery>().StoredEnergy > 0f)
							{
								this.TryTurnOnImmediately(powerComp, map);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x001FCDBC File Offset: 0x001FAFBC
		private void EnsureGeneratorsConnectedAndMakeSense(Map map)
		{
			this.tmpThings.Clear();
			this.tmpThings.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial));
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.IsPowerGenerator(this.tmpThings[i]))
				{
					PowerNet powerNet = this.tmpThings[i].TryGetComp<CompPower>().PowerNet;
					if (powerNet == null || !this.HasAnyPowerUser(powerNet))
					{
						map.powerNetManager.UpdatePowerNetsAndConnections_First();
						PowerNet powerNet2;
						IntVec3 dest;
						if (this.TryFindClosestReachableNet(this.tmpThings[i].Position, (PowerNet x) => this.HasAnyPowerUser(x), map, out powerNet2, out dest))
						{
							map.floodFiller.ReconstructLastFloodFillPath(dest, this.tmpCells);
							this.SpawnTransmitters(this.tmpCells, map, this.tmpThings[i].Faction);
						}
					}
				}
			}
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x001FCEA8 File Offset: 0x001FB0A8
		private bool IsPowerUser(Thing thing)
		{
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput < 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption > 0f));
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x001FCEEC File Offset: 0x001FB0EC
		private bool IsPowerGenerator(Thing thing)
		{
			if (thing.TryGetComp<CompPowerPlant>() != null)
			{
				return true;
			}
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && (compPowerTrader.PowerOutput > 0f || (!compPowerTrader.PowerOn && compPowerTrader.Props.basePowerConsumption < 0f));
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x001FCF3C File Offset: 0x001FB13C
		private bool HasAnyPowerGenerator(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			for (int i = 0; i < powerComps.Count; i++)
			{
				if (this.IsPowerGenerator(powerComps[i].parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x001FCF78 File Offset: 0x001FB178
		private bool HasAnyPowerUser(PowerNet net)
		{
			List<CompPowerTrader> powerComps = net.powerComps;
			for (int i = 0; i < powerComps.Count; i++)
			{
				if (this.IsPowerUser(powerComps[i].parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x001FCFB4 File Offset: 0x001FB1B4
		private bool TryFindClosestReachableNet(IntVec3 root, Predicate<PowerNet> predicate, Map map, out PowerNet foundNet, out IntVec3 closestTransmitter)
		{
			this.tmpPowerNetPredicateResults.Clear();
			PowerNet foundNetLocal = null;
			IntVec3 closestTransmitterLocal = IntVec3.Invalid;
			map.floodFiller.FloodFill(root, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
			{
				Building transmitter = x.GetTransmitter(map);
				PowerNet powerNet = (transmitter != null) ? transmitter.GetComp<CompPower>().PowerNet : null;
				if (powerNet == null)
				{
					return false;
				}
				bool flag;
				if (!this.tmpPowerNetPredicateResults.TryGetValue(powerNet, out flag))
				{
					flag = predicate(powerNet);
					this.tmpPowerNetPredicateResults.Add(powerNet, flag);
				}
				if (flag)
				{
					foundNetLocal = powerNet;
					closestTransmitterLocal = x;
					return true;
				}
				return false;
			}, int.MaxValue, true, null);
			this.tmpPowerNetPredicateResults.Clear();
			if (foundNetLocal != null)
			{
				foundNet = foundNetLocal;
				closestTransmitter = closestTransmitterLocal;
				return true;
			}
			foundNet = null;
			closestTransmitter = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x001FD068 File Offset: 0x001FB268
		private void SpawnTransmitters(List<IntVec3> cells, Map map, Faction faction)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if (cells[i].GetTransmitter(map) == null)
				{
					GenSpawn.Spawn(ThingDefOf.PowerConduit, cells[i], map, WipeMode.Vanish).SetFaction(faction, null);
				}
			}
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x001FD0B0 File Offset: 0x001FB2B0
		private void SpawnTransmitters(IntVec3 start, IntVec3 end, Map map, Faction faction)
		{
			bool foundPath = false;
			map.floodFiller.FloodFill(start, (IntVec3 x) => this.EverPossibleToTransmitPowerAt(x, map), delegate(IntVec3 x)
			{
				if (x == end)
				{
					foundPath = true;
					return true;
				}
				return false;
			}, int.MaxValue, true, null);
			if (foundPath)
			{
				map.floodFiller.ReconstructLastFloodFillPath(end, GenStep_Power.tmpTransmitterCells);
				this.SpawnTransmitters(GenStep_Power.tmpTransmitterCells, map, faction);
			}
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x001FD148 File Offset: 0x001FB348
		private bool TrySpawnPowerTransmittingBuildingNear(IntVec3 position, Map map, Faction faction, ThingDef def, out Building newBuilding, Predicate<IntVec3> extraValidator = null)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false);
			IntVec3 loc;
			if (RCellFinder.TryFindRandomCellNearWith(position, delegate(IntVec3 x)
			{
				if (!x.Standable(map) || x.Roofed(map) || !this.EverPossibleToTransmitPowerAt(x, map))
				{
					return false;
				}
				if (!map.reachability.CanReach(position, x, PathEndMode.OnCell, traverseParams))
				{
					return false;
				}
				foreach (IntVec3 c in GenAdj.OccupiedRect(x, Rot4.North, def.size))
				{
					if (!c.InBounds(map) || c.Roofed(map) || c.GetEdifice(map) != null || c.GetFirstItem(map) != null || c.GetTransmitter(map) != null)
					{
						return false;
					}
				}
				return extraValidator == null || extraValidator(x);
			}, map, out loc, 8, 2147483647))
			{
				newBuilding = (Building)GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map, Rot4.North, WipeMode.Vanish, false);
				newBuilding.SetFaction(faction, null);
				return true;
			}
			newBuilding = null;
			return false;
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x000468B6 File Offset: 0x00044AB6
		private bool TrySpawnPowerGeneratorNear(IntVec3 position, Map map, Faction faction, out Building newPowerGenerator)
		{
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.SolarGenerator, out newPowerGenerator, null))
			{
				map.powerNetManager.UpdatePowerNetsAndConnections_First();
				newPowerGenerator.GetComp<CompPowerPlant>().UpdateDesiredPowerOutput();
				return true;
			}
			return false;
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x001FD1F0 File Offset: 0x001FB3F0
		private bool TrySpawnBatteryNear(IntVec3 position, Map map, Faction faction, out Building newBattery)
		{
			Predicate<IntVec3> extraValidator = null;
			if (this.spawnRoofOverNewBatteries)
			{
				extraValidator = delegate(IntVec3 x)
				{
					foreach (IntVec3 c in GenAdj.OccupiedRect(x, Rot4.North, ThingDefOf.Battery.size).ExpandedBy(3))
					{
						if (c.InBounds(map))
						{
							List<Thing> thingList = c.GetThingList(map);
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].def.PlaceWorkers != null)
								{
									if (thingList[i].def.PlaceWorkers.Any((PlaceWorker y) => y is PlaceWorker_NotUnderRoof))
									{
										return false;
									}
								}
							}
						}
					}
					return true;
				};
			}
			if (this.TrySpawnPowerTransmittingBuildingNear(position, map, faction, ThingDefOf.Battery, out newBattery, extraValidator))
			{
				float randomInRange = this.newBatteriesInitialStoredEnergyPctRange.RandomInRange;
				newBattery.GetComp<CompPowerBattery>().SetStoredEnergyPct(randomInRange);
				if (this.spawnRoofOverNewBatteries)
				{
					this.SpawnRoofOver(newBattery);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x001FD268 File Offset: 0x001FB468
		private bool TrySpawnPowerGeneratorAndBatteryIfCanAndConnect(Thing forThing, Map map)
		{
			if (!this.canSpawnPowerGenerators)
			{
				return false;
			}
			IntVec3 position = forThing.Position;
			Building building;
			if (this.canSpawnBatteries && Rand.Chance(this.hasAtleast1TurretInt ? 1f : 0.1f) && this.TrySpawnBatteryNear(forThing.Position, map, forThing.Faction, out building))
			{
				this.SpawnTransmitters(forThing.Position, building.Position, map, forThing.Faction);
				position = building.Position;
			}
			Building building2;
			if (this.TrySpawnPowerGeneratorNear(position, map, forThing.Faction, out building2))
			{
				this.SpawnTransmitters(position, building2.Position, map, forThing.Faction);
				return true;
			}
			return false;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x000468E6 File Offset: 0x00044AE6
		private bool EverPossibleToTransmitPowerAt(IntVec3 c, Map map)
		{
			return c.GetTransmitter(map) != null || GenConstruct.CanBuildOnTerrain(ThingDefOf.PowerConduit, c, map, Rot4.North, null, null);
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x00046906 File Offset: 0x00044B06
		private void TryTurnOnImmediately(CompPowerTrader powerComp, Map map)
		{
			if (powerComp.PowerOn)
			{
				return;
			}
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
			if (powerComp.PowerNet != null && powerComp.PowerNet.CurrentEnergyGainRate() > 1E-07f)
			{
				powerComp.PowerOn = true;
			}
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x001FD30C File Offset: 0x001FB50C
		private void SpawnRoofOver(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool flag = true;
			using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Roofed(thing.Map))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				return;
			}
			int num = 0;
			CellRect cellRect2 = cellRect.ExpandedBy(2);
			foreach (IntVec3 c in cellRect2)
			{
				if (c.InBounds(thing.Map) && c.GetRoofHolderOrImpassable(thing.Map) != null)
				{
					num++;
				}
			}
			if (num < 2)
			{
				ThingDef stuff = Rand.Element<ThingDef>(ThingDefOf.WoodLog, ThingDefOf.Steel);
				Func<IntVec3, bool> <>9__0;
				foreach (IntVec3 intVec in cellRect2.Corners)
				{
					if (intVec.InBounds(thing.Map) && intVec.Standable(thing.Map) && intVec.GetFirstItem(thing.Map) == null && intVec.GetFirstBuilding(thing.Map) == null && intVec.GetFirstPawn(thing.Map) == null)
					{
						IEnumerable<IntVec3> source = GenAdj.CellsAdjacent8Way(new TargetInfo(intVec, thing.Map, false));
						Func<IntVec3, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((IntVec3 x) => !x.InBounds(thing.Map) || !x.Walkable(thing.Map)));
						}
						if (!source.Any(predicate) && intVec.SupportsStructureType(thing.Map, ThingDefOf.Wall.terrainAffordanceNeeded))
						{
							Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Wall, stuff);
							GenSpawn.Spawn(thing2, intVec, thing.Map, WipeMode.Vanish);
							thing2.SetFaction(thing.Faction, null);
							num++;
						}
					}
				}
			}
			if (num > 0)
			{
				foreach (IntVec3 c2 in cellRect2)
				{
					if (c2.InBounds(thing.Map) && !c2.Roofed(thing.Map))
					{
						thing.Map.roofGrid.SetRoof(c2, RoofDefOf.RoofConstructed);
					}
				}
			}
		}

		// Token: 0x040044B3 RID: 17587
		public bool canSpawnBatteries = true;

		// Token: 0x040044B4 RID: 17588
		public bool canSpawnPowerGenerators = true;

		// Token: 0x040044B5 RID: 17589
		public bool spawnRoofOverNewBatteries = true;

		// Token: 0x040044B6 RID: 17590
		public FloatRange newBatteriesInitialStoredEnergyPctRange = new FloatRange(0.2f, 0.5f);

		// Token: 0x040044B7 RID: 17591
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x040044B8 RID: 17592
		private List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x040044B9 RID: 17593
		private const int MaxDistToExistingNetForTurrets = 13;

		// Token: 0x040044BA RID: 17594
		private const int RoofPadding = 2;

		// Token: 0x040044BB RID: 17595
		private static readonly IntRange MaxDistanceBetweenBatteryAndTransmitter = new IntRange(20, 50);

		// Token: 0x040044BC RID: 17596
		private bool hasAtleast1TurretInt;

		// Token: 0x040044BD RID: 17597
		private Dictionary<PowerNet, bool> tmpPowerNetPredicateResults = new Dictionary<PowerNet, bool>();

		// Token: 0x040044BE RID: 17598
		private static List<IntVec3> tmpTransmitterCells = new List<IntVec3>();
	}
}
