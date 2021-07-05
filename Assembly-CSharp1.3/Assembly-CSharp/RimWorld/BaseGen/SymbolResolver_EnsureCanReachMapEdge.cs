using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015BB RID: 5563
	public class SymbolResolver_EnsureCanReachMapEdge : SymbolResolver
	{
		// Token: 0x06008314 RID: 33556 RVA: 0x002EAA8C File Offset: 0x002E8C8C
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Clear();
			foreach (IntVec3 item in rp.rect)
			{
				SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Add(item);
			}
			SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Shuffle<IntVec3>();
			this.TryMakeAllCellsReachable(false, rp);
			this.TryMakeAllCellsReachable(true, rp);
		}

		// Token: 0x06008315 RID: 33557 RVA: 0x002EAB08 File Offset: 0x002E8D08
		private void TryMakeAllCellsReachable(bool canPathThroughNonStandable, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder.Count; i++)
			{
				IntVec3 intVec = SymbolResolver_EnsureCanReachMapEdge.cellsInRandomOrder[i];
				if (this.CanTraverse(intVec, canPathThroughNonStandable))
				{
					District district = intVec.GetDistrict(map, RegionType.Set_Passable);
					if (district != null && !SymbolResolver_EnsureCanReachMapEdge.visited.Contains(district))
					{
						SymbolResolver_EnsureCanReachMapEdge.visited.Add(district);
						TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
						if (!map.reachability.CanReachMapEdge(intVec, traverseParms))
						{
							bool found = false;
							IntVec3 foundDest = IntVec3.Invalid;
							map.floodFiller.FloodFill(intVec, (IntVec3 x) => !found && this.CanTraverse(x, canPathThroughNonStandable), delegate(IntVec3 x)
							{
								if (found)
								{
									return;
								}
								if (map.reachability.CanReachMapEdge(x, traverseParms))
								{
									found = true;
									foundDest = x;
								}
							}, int.MaxValue, true, null);
							if (found)
							{
								this.ReconstructPathAndDestroyWalls(foundDest, district, rp);
							}
						}
						district = intVec.GetDistrict(map, RegionType.Set_Passable);
						if (district != null)
						{
							RegionTraverser.BreadthFirstTraverse(district.Regions[0], (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
							{
								if (r.District.TouchesMapEdge)
								{
									MapGenerator.rootsToUnfog.Add(r.AnyCell);
									return true;
								}
								return false;
							}, 9999, RegionType.Set_Passable);
						}
					}
				}
			}
			SymbolResolver_EnsureCanReachMapEdge.visited.Clear();
		}

		// Token: 0x06008316 RID: 33558 RVA: 0x002EACD8 File Offset: 0x002E8ED8
		private void ReconstructPathAndDestroyWalls(IntVec3 foundDest, District room, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			map.floodFiller.ReconstructLastFloodFillPath(foundDest, SymbolResolver_EnsureCanReachMapEdge.path);
			while (SymbolResolver_EnsureCanReachMapEdge.path.Count >= 2 && SymbolResolver_EnsureCanReachMapEdge.path[0].AdjacentToCardinal(room) && SymbolResolver_EnsureCanReachMapEdge.path[1].AdjacentToCardinal(room))
			{
				SymbolResolver_EnsureCanReachMapEdge.path.RemoveAt(0);
			}
			IntVec3 intVec = IntVec3.Invalid;
			ThingDef thingDef = null;
			IntVec3 intVec2 = IntVec3.Invalid;
			ThingDef thingDef2 = null;
			for (int i = 0; i < SymbolResolver_EnsureCanReachMapEdge.path.Count; i++)
			{
				Building edifice = SymbolResolver_EnsureCanReachMapEdge.path[i].GetEdifice(map);
				if (this.IsWallOrRock(edifice))
				{
					if (!intVec.IsValid)
					{
						intVec = SymbolResolver_EnsureCanReachMapEdge.path[i];
						thingDef = edifice.Stuff;
					}
					intVec2 = SymbolResolver_EnsureCanReachMapEdge.path[i];
					thingDef2 = edifice.Stuff;
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef3;
				if ((thingDef3 = thingDef) == null)
				{
					thingDef3 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef3;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, map, WipeMode.Vanish);
			}
			if (intVec2.IsValid && intVec2 != intVec && !intVec2.AdjacentToCardinal(intVec))
			{
				ThingDef thingDef4;
				if ((thingDef4 = thingDef2) == null)
				{
					thingDef4 = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff2 = thingDef4;
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.Door, stuff2);
				thing2.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing2, intVec2, map, WipeMode.Vanish);
			}
		}

		// Token: 0x06008317 RID: 33559 RVA: 0x002EAE74 File Offset: 0x002E9074
		private bool CanTraverse(IntVec3 c, bool canPathThroughNonStandable)
		{
			Map map = BaseGen.globalSettings.map;
			Building edifice = c.GetEdifice(map);
			return this.IsWallOrRock(edifice) || ((canPathThroughNonStandable || (c.Standable(map) && c.GetEdifice(map) == null)) && !c.Impassable(map));
		}

		// Token: 0x06008318 RID: 33560 RVA: 0x002EAEC2 File Offset: 0x002E90C2
		private bool IsWallOrRock(Building b)
		{
			return b != null && (b.def == ThingDefOf.Wall || b.def.building.isNaturalRock);
		}

		// Token: 0x040051F9 RID: 20985
		private static HashSet<District> visited = new HashSet<District>();

		// Token: 0x040051FA RID: 20986
		private static List<IntVec3> path = new List<IntVec3>();

		// Token: 0x040051FB RID: 20987
		private static List<IntVec3> cellsInRandomOrder = new List<IntVec3>();
	}
}
