using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E91 RID: 7825
	public class SymbolResolver_OutdoorLighting : SymbolResolver
	{
		// Token: 0x0600A855 RID: 43093 RVA: 0x00310624 File Offset: 0x0030E824
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
			//寻找供电
			this.FindNearbyGlowers(rp.rect);
			for (int i = 0; i < rp.rect.Area / 4; i++)
			{
				IntVec3 randomCell = rp.rect.RandomCell;
				//能站立 没人 没建筑 没物品
				if (randomCell.Standable(map) && randomCell.GetFirstItem(map) == null && randomCell.GetFirstPawn(map) == null && randomCell.GetFirstBuilding(map) == null)
				{
					//当前随机坐标的地区
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

		// Token: 0x0600A856 RID: 43094 RVA: 0x0031078C File Offset: 0x0030E98C
		private void FindNearbyGlowers(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			nearbyGlowers.Clear();
			//扩涨
			rect = rect.ExpandedBy(4);
			//越界校验
			rect = rect.ClipInsideMap(map);
			foreach (IntVec3 intVec in rect)
			{
				Region region = intVec.GetRegion(map);
				//当前坐标在地区内 并且是户外地区
				if (region != null && region.Room.PsychologicallyOutdoors)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						//把附近能供电的物体加入列表
						CompGlower compGlower = thingList[i].TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							nearbyGlowers.Add(compGlower);
						}
					}
				}
			}
		}

		// Token: 0x0600A857 RID: 43095 RVA: 0x00310858 File Offset: 0x0030EA58
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

		// Token: 0x04007234 RID: 29236
		private static List<CompGlower> nearbyGlowers = new List<CompGlower>();

		// Token: 0x04007235 RID: 29237
		private const float Margin = 2f;
	}
}
