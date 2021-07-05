using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9D RID: 3229
	public class GenStep_ScatterRoadDebris : GenStep_Scatterer
	{
		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x0019063E File Offset: 0x0018E83E
		public override int SeedPart
		{
			get
			{
				return 765346456;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x00190645 File Offset: 0x0018E845
		public IEnumerable<ThingDef> VehicleBuildings
		{
			get
			{
				yield return ThingDefOf.AncientRustedCarFrame;
				yield return ThingDefOf.AncientRustedJeep;
				yield return ThingDefOf.AncientRustedCar;
				yield break;
			}
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00190650 File Offset: 0x0018E850
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter road debris"))
			{
				return;
			}
			this.allowInWaterBiome = false;
			this.mapHasRoads = this.HasAncientRoads(map);
			this.count = (this.mapHasRoads ? GenStep_ScatterRoadDebris.VehicleRangeRoadMap : GenStep_ScatterRoadDebris.VehicleRangeNonRoadMap).RandomInRange;
			this.thingToPlace = this.VehicleBuildings.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x001906BC File Offset: 0x0018E8BC
		private bool HasAncientRoads(Map map)
		{
			List<Tile.RoadLink> roads = map.TileInfo.Roads;
			if (roads == null)
			{
				return false;
			}
			for (int i = 0; i < roads.Count; i++)
			{
				if (roads[i].road == RoadDefOf.AncientAsphaltHighway || roads[i].road == RoadDefOf.AncientAsphaltRoad)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x00190714 File Offset: 0x0018E914
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (this.mapHasRoads && !c.GetTerrain(map).IsRoad)
			{
				return false;
			}
			int num = Rand.RangeInclusive(1, 4);
			for (int i = 0; i < 4; i++)
			{
				this.rotation = new Rot4((i + num) % 4);
				if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x0018F922 File Offset: 0x0018DB22
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00190784 File Offset: 0x0018E984
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.75f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.25f, 0, int.MaxValue, null);
			IntVec3 loc2;
			if (Rand.Chance(0.5f) && RCellFinder.TryFindRandomCellNearWith(loc, (IntVec3 c) => this.CanPlaceThingAt(c, Rot4.North, map, ThingDefOf.AncientRustedEngineBlock), map, out loc2, 10, 2147483647))
			{
				ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientRustedEngineBlock, null), loc2, map, Rot4.North, WipeMode.Vanish, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.15f, 0, int.MaxValue, null);
			}
			this.thingToPlace = this.VehicleBuildings.RandomElement<ThingDef>();
		}

		// Token: 0x04002DA7 RID: 11687
		private const float EngineBlockChance = 0.5f;

		// Token: 0x04002DA8 RID: 11688
		private const int EngineBlockMinDistance = 10;

		// Token: 0x04002DA9 RID: 11689
		private static readonly IntRange VehicleRangeNonRoadMap = new IntRange(1, 2);

		// Token: 0x04002DAA RID: 11690
		private static readonly IntRange VehicleRangeRoadMap = new IntRange(2, 3);

		// Token: 0x04002DAB RID: 11691
		private const float OilSmearChance = 0.15f;

		// Token: 0x04002DAC RID: 11692
		private ThingDef thingToPlace;

		// Token: 0x04002DAD RID: 11693
		private Rot4 rotation;

		// Token: 0x04002DAE RID: 11694
		private bool mapHasRoads;
	}
}
