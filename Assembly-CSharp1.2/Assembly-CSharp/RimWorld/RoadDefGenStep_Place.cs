using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129D RID: 4765
	public class RoadDefGenStep_Place : RoadDefGenStep_Bulldoze
	{
		// Token: 0x060067A1 RID: 26529 RVA: 0x001FED78 File Offset: 0x001FCF78
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			if (this.onlyIfOriginAllows)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.place, origin, map, Rot4.North, null, null) && origin.GetTerrain(map) != this.place)
				{
					return;
				}
				bool flag = false;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = position + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(map) && this.chancePerPositionCurve.Evaluate(distance[intVec.x, intVec.z].fromRoad) > 0f && (GenConstruct.CanBuildOnTerrain(this.place, intVec, map, Rot4.North, null, null) || intVec.GetTerrain(map) == this.place) && (GenConstruct.CanBuildOnTerrain(this.place, distance[intVec.x, intVec.z].origin, map, Rot4.North, null, null) || distance[intVec.x, intVec.z].origin.GetTerrain(map) == this.place))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			if (!this.suppressOnTerrainTag.NullOrEmpty() && map.terrainGrid.TerrainAt(position).HasTag(this.suppressOnTerrainTag))
			{
				return;
			}
			base.Place(map, position, rockDef, origin, distance);
			if (this.place is TerrainDef)
			{
				if (this.proximitySpacing != 0)
				{
					Log.ErrorOnce("Proximity spacing used for road terrain placement; not yet supported", 60936625, false);
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
				TerrainDef terrainDef2 = (TerrainDef)this.place;
				if (terrainDef2 == TerrainDefOf.FlagstoneSandstone)
				{
					terrainDef2 = rockDef;
				}
				if (terrainDef2 == TerrainDefOf.Bridge)
				{
					if (terrainDef == TerrainDefOf.WaterDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterShallow);
					}
					if (terrainDef == TerrainDefOf.WaterOceanDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterOceanShallow);
					}
				}
				if (GenConstruct.CanBuildOnTerrain(terrainDef2, position, map, Rot4.North, null, null) && (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, position, map, Rot4.North, null, null) || terrainDef2 == TerrainDefOf.Bridge) && terrainDef != TerrainDefOf.Bridge)
				{
					if (terrainDef.HasTag("Road") && !terrainDef.Removable)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.Gravel);
					}
					map.terrainGrid.SetTerrain(position, terrainDef2);
				}
				if (position.OnEdge(map) && !map.roadInfo.roadEdgeTiles.Contains(position))
				{
					map.roadInfo.roadEdgeTiles.Add(position);
					return;
				}
			}
			else if (this.place is ThingDef)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.place, position, map, Rot4.North, null, null))
				{
					return;
				}
				if (this.proximitySpacing > 0 && GenClosest.ClosestThing_Global(position, map.listerThings.ThingsOfDef((ThingDef)this.place), (float)this.proximitySpacing, null, null) != null)
				{
					return;
				}
				while (position.GetThingList(map).Count > 0)
				{
					position.GetThingList(map)[0].Destroy(DestroyMode.Vanish);
				}
				RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, TerrainDefOf.Gravel);
				GenSpawn.Spawn(ThingMaker.MakeThing((ThingDef)this.place, null), position, map, WipeMode.Vanish);
				return;
			}
			else
			{
				Log.ErrorOnce(string.Format("Can't figure out how to place object {0} while building road", this.place), 10785584, false);
			}
		}

		// Token: 0x040044F9 RID: 17657
		public BuildableDef place;

		// Token: 0x040044FA RID: 17658
		public int proximitySpacing;

		// Token: 0x040044FB RID: 17659
		public bool onlyIfOriginAllows;

		// Token: 0x040044FC RID: 17660
		public string suppressOnTerrainTag;
	}
}
