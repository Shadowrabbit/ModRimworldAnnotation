﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E8 RID: 5608
	public class SymbolResolver_OutdoorLighting : SymbolResolver
	{
		// Token: 0x060083A9 RID: 33705 RVA: 0x002F0A74 File Offset: 0x002EEC74
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			this.FindNearbyGlowers(rp.rect);
			for (int i = 0; i < rp.rect.Area / 4; i++)
			{
				IntVec3 randomCell = rp.rect.RandomCell;
				if (randomCell.Standable(map) && randomCell.GetFirstItem(map) == null && randomCell.GetFirstPawn(map) == null && randomCell.GetFirstBuilding(map) == null)
				{
					Region region = randomCell.GetRegion(map, RegionType.Set_Passable);
					if (region != null && region.Room.PsychologicallyOutdoors && region.Room.UsesOutdoorTemperature && !this.AnyGlowerNearby(randomCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(randomCell, map))
					{
						if (rp.spawnBridgeIfTerrainCantSupportThing == null || rp.spawnBridgeIfTerrainCantSupportThing.Value)
						{
							BaseGenUtility.CheckSpawnBridgeUnder(thingDef, randomCell, Rot4.North);
						}
						Thing thing = GenSpawn.Spawn(thingDef, randomCell, map, WipeMode.Vanish);
						if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
						{
							thing.SetFaction(rp.faction, null);
						}
						SymbolResolver_OutdoorLighting.nearbyGlowers.Add(thing.TryGetComp<CompGlower>());
					}
				}
			}
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
		}

		// Token: 0x060083AA RID: 33706 RVA: 0x002F0BDC File Offset: 0x002EEDDC
		private void FindNearbyGlowers(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
			rect = rect.ExpandedBy(4);
			rect = rect.ClipInsideMap(map);
			foreach (IntVec3 intVec in rect)
			{
				Region region = intVec.GetRegion(map, RegionType.Set_Passable);
				if (region != null && region.Room.PsychologicallyOutdoors)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						CompGlower compGlower = thingList[i].TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							SymbolResolver_OutdoorLighting.nearbyGlowers.Add(compGlower);
						}
					}
				}
			}
		}

		// Token: 0x060083AB RID: 33707 RVA: 0x002F0CA8 File Offset: 0x002EEEA8
		private bool AnyGlowerNearby(IntVec3 c)
		{
			for (int i = 0; i < SymbolResolver_OutdoorLighting.nearbyGlowers.Count; i++)
			{
				if (c.InHorDistOf(SymbolResolver_OutdoorLighting.nearbyGlowers[i].parent.Position, SymbolResolver_OutdoorLighting.nearbyGlowers[i].Props.glowRadius + 2f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400522D RID: 21037
		private static List<CompGlower> nearbyGlowers = new List<CompGlower>();

		// Token: 0x0400522E RID: 21038
		private const float Margin = 2f;
	}
}
