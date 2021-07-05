using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C93 RID: 3219
	public class RoadDefGenStep_Place : RoadDefGenStep_Bulldoze
	{
		// Token: 0x06004B28 RID: 19240 RVA: 0x0018F028 File Offset: 0x0018D228
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			if (this.onlyIfOriginAllows)
			{
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
					Log.ErrorOnce("Proximity spacing used for road terrain placement; not yet supported", 60936625);
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(position);
				TerrainDef terrainDef2 = (TerrainDef)this.place;
				if (terrainDef2 == TerrainDefOf.FlagstoneSandstone)
				{
					terrainDef2 = rockDef;
				}
				if (terrainDef2.bridge)
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
				if (GenConstruct.CanBuildOnTerrain(terrainDef2, position, map, Rot4.North, null, null) && (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, position, map, Rot4.North, null, null) || terrainDef2.bridge) && !terrainDef.bridge)
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
				Log.ErrorOnce(string.Format("Can't figure out how to place object {0} while building road", this.place), 10785584);
			}
		}

		// Token: 0x04002D8B RID: 11659
		public BuildableDef place;

		// Token: 0x04002D8C RID: 11660
		public int proximitySpacing;

		// Token: 0x04002D8D RID: 11661
		public bool onlyIfOriginAllows;

		// Token: 0x04002D8E RID: 11662
		public string suppressOnTerrainTag;
	}
}
