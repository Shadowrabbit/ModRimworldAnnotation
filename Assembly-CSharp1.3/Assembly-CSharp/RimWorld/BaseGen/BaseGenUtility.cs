using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001596 RID: 5526
	public static class BaseGenUtility
	{
		// Token: 0x06008281 RID: 33409 RVA: 0x002E5024 File Offset: 0x002E3224
		public static ThingDef CheapStuffFor(ThingDef thingDef, Faction faction)
		{
			ThingDef thingDef2 = (from stuff in DefDatabase<ThingDef>.AllDefs
			where stuff.stuffProps != null && stuff.BaseMarketValue / stuff.VolumePerUnit < 5f && stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony) && stuff.stuffProps.CanMake(thingDef)
			select stuff).RandomElementWithFallback(null);
			if (thingDef2 != null)
			{
				return thingDef2;
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(thingDef))
			{
				return ThingDefOf.WoodLog;
			}
			return GenStuff.RandomStuffInexpensiveFor(thingDef, faction, null);
		}

		// Token: 0x06008282 RID: 33410 RVA: 0x002E508A File Offset: 0x002E328A
		public static ThingDef RandomCheapWallStuff(Faction faction, bool notVeryFlammable = false)
		{
			return BaseGenUtility.RandomCheapWallStuff((faction == null) ? TechLevel.Spacer : faction.def.techLevel, notVeryFlammable);
		}

		// Token: 0x06008283 RID: 33411 RVA: 0x002E50A4 File Offset: 0x002E32A4
		public static ThingDef RandomCheapWallStuff(TechLevel techLevel, bool notVeryFlammable = false)
		{
			if (techLevel.IsNeolithicOrWorse())
			{
				return ThingDefOf.WoodLog;
			}
			ThingDef thingDef = (from stuff in DefDatabase<ThingDef>.AllDefs
			where stuff.stuffProps != null && BaseGenUtility.IsCheapWallStuff(stuff) && stuff.stuffProps.categories.Contains(StuffCategoryDefOf.Stony)
			select stuff).RandomElementWithFallback(null);
			if (thingDef != null)
			{
				return thingDef;
			}
			return (from d in DefDatabase<ThingDef>.AllDefsListForReading
			where BaseGenUtility.IsCheapWallStuff(d) && (!notVeryFlammable || d.BaseFlammability < 0.5f)
			select d).RandomElement<ThingDef>();
		}

		// Token: 0x06008284 RID: 33412 RVA: 0x002E511C File Offset: 0x002E331C
		public static bool IsCheapWallStuff(ThingDef d)
		{
			return d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) && d.BaseMarketValue / d.VolumePerUnit < 5f;
		}

		// Token: 0x06008285 RID: 33413 RVA: 0x002E514E File Offset: 0x002E334E
		public static ThingDef RandomHightechWallStuff()
		{
			if (Rand.Value < 0.15f)
			{
				return ThingDefOf.Plasteel;
			}
			return ThingDefOf.Steel;
		}

		// Token: 0x06008286 RID: 33414 RVA: 0x002E5167 File Offset: 0x002E3367
		public static TerrainDef RandomHightechFloorDef()
		{
			return Rand.Element<TerrainDef>(TerrainDefOf.Concrete, TerrainDefOf.Concrete, TerrainDefOf.PavedTile, TerrainDefOf.PavedTile, TerrainDefOf.MetalTile);
		}

		// Token: 0x06008287 RID: 33415 RVA: 0x002E5187 File Offset: 0x002E3387
		private static IEnumerable<TerrainDef> IdeoFloorTypes(Faction faction, bool allowCarpet = true)
		{
			if (faction == null || faction.ideos == null)
			{
				yield break;
			}
			foreach (Ideo ideo in faction.ideos.AllIdeos)
			{
				using (HashSet<BuildableDef>.Enumerator enumerator2 = ideo.cachedPossibleBuildables.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TerrainDef terrainDef;
						if ((terrainDef = (enumerator2.Current as TerrainDef)) != null && (allowCarpet || !terrainDef.IsCarpet))
						{
							yield return terrainDef;
						}
					}
				}
				HashSet<BuildableDef>.Enumerator enumerator2 = default(HashSet<BuildableDef>.Enumerator);
			}
			IEnumerator<Ideo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008288 RID: 33416 RVA: 0x002E51A0 File Offset: 0x002E33A0
		public static TerrainDef RandomBasicFloorDef(Faction faction, bool allowCarpet = false)
		{
			bool flag = allowCarpet && (faction == null || !faction.def.techLevel.IsNeolithicOrWorse()) && Rand.Chance(0.1f);
			TerrainDef result;
			if (faction != null && faction.ideos != null && BaseGenUtility.IdeoFloorTypes(faction, flag).TryRandomElement(out result))
			{
				return result;
			}
			if (flag)
			{
				return (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			return Rand.Element<TerrainDef>(TerrainDefOf.MetalTile, TerrainDefOf.PavedTile, TerrainDefOf.WoodPlankFloor, TerrainDefOf.TileSandstone);
		}

		// Token: 0x06008289 RID: 33417 RVA: 0x002E523C File Offset: 0x002E343C
		public static bool TryRandomInexpensiveFloor(out TerrainDef floor, Predicate<TerrainDef> validator = null)
		{
			Func<TerrainDef, float> costCalculator = delegate(TerrainDef x)
			{
				List<ThingDefCountClass> list = x.CostListAdjusted(null, true);
				float num2 = 0f;
				for (int i = 0; i < list.Count; i++)
				{
					num2 += (float)list[i].count * list[i].thingDef.BaseMarketValue;
				}
				return num2;
			};
			IEnumerable<TerrainDef> enumerable = from x in DefDatabase<TerrainDef>.AllDefs
			where x.BuildableByPlayer && x.terrainAffordanceNeeded != TerrainAffordanceDefOf.Bridgeable && !x.IsCarpet && (validator == null || validator(x)) && costCalculator(x) > 0f
			select x;
			float cheapest = -1f;
			foreach (TerrainDef arg in enumerable)
			{
				float num = costCalculator(arg);
				if (cheapest == -1f || num < cheapest)
				{
					cheapest = num;
				}
			}
			return (from x in enumerable
			where costCalculator(x) <= cheapest * 4f
			select x).TryRandomElement(out floor);
		}

		// Token: 0x0600828A RID: 33418 RVA: 0x002E5318 File Offset: 0x002E3518
		public static TerrainDef CorrespondingTerrainDef(ThingDef stuffDef, bool beautiful, Faction faction = null)
		{
			BaseGenUtility.tmpFactionFloors.Clear();
			if (faction != null && faction.ideos != null)
			{
				foreach (TerrainDef terrainDef in BaseGenUtility.IdeoFloorTypes(faction, true))
				{
					if (terrainDef.CostList != null)
					{
						for (int i = 0; i < terrainDef.CostList.Count; i++)
						{
							if (terrainDef.CostList[i].thingDef == stuffDef)
							{
								BaseGenUtility.tmpFactionFloors.Add(terrainDef);
								break;
							}
						}
					}
				}
				TerrainDef result;
				if (BaseGenUtility.tmpFactionFloors.Any<TerrainDef>() && BaseGenUtility.tmpFactionFloors.TryRandomElementByWeight(delegate(TerrainDef x)
				{
					float statOffsetFromList = x.statBases.GetStatOffsetFromList(StatDefOf.Beauty);
					if (statOffsetFromList == 0f)
					{
						return 0f;
					}
					if (!beautiful)
					{
						return 1f / statOffsetFromList;
					}
					return statOffsetFromList;
				}, out result))
				{
					return result;
				}
			}
			TerrainDef terrainDef2 = null;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				if (allDefsListForReading[j].CostList != null)
				{
					for (int k = 0; k < allDefsListForReading[j].CostList.Count; k++)
					{
						if (allDefsListForReading[j].CostList[k].thingDef == stuffDef && (terrainDef2 == null || (beautiful ? (terrainDef2.statBases.GetStatOffsetFromList(StatDefOf.Beauty) < allDefsListForReading[j].statBases.GetStatOffsetFromList(StatDefOf.Beauty)) : (terrainDef2.statBases.GetStatOffsetFromList(StatDefOf.Beauty) > allDefsListForReading[j].statBases.GetStatOffsetFromList(StatDefOf.Beauty)))))
						{
							terrainDef2 = allDefsListForReading[j];
						}
					}
				}
			}
			if (terrainDef2 == null)
			{
				terrainDef2 = TerrainDefOf.Concrete;
			}
			return terrainDef2;
		}

		// Token: 0x0600828B RID: 33419 RVA: 0x002E54EC File Offset: 0x002E36EC
		public static TerrainDef RegionalRockTerrainDef(int tile, bool beautiful)
		{
			ThingDef thingDef = Find.World.NaturalRockTypesIn(tile).RandomElementWithFallback(null);
			ThingDef thingDef2 = (thingDef != null) ? thingDef.building.mineableThing : null;
			return BaseGenUtility.CorrespondingTerrainDef((thingDef2 != null && thingDef2.butcherProducts != null && thingDef2.butcherProducts.Count > 0) ? thingDef2.butcherProducts[0].thingDef : null, beautiful, null);
		}

		// Token: 0x0600828C RID: 33420 RVA: 0x002E5554 File Offset: 0x002E3754
		public static bool AnyDoorAdjacentCardinalTo(IntVec3 cell, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600828D RID: 33421 RVA: 0x002E5594 File Offset: 0x002E3794
		public static bool AnyDoorAdjacentCardinalTo(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.AdjacentCellsCardinal)
			{
				if (c.InBounds(map) && c.GetDoor(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600828E RID: 33422 RVA: 0x002E55F4 File Offset: 0x002E37F4
		public static ThingDef WallStuffAt(IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def == ThingDefOf.Wall)
			{
				return edifice.Stuff;
			}
			return null;
		}

		// Token: 0x0600828F RID: 33423 RVA: 0x002E5624 File Offset: 0x002E3824
		public static void CheckSpawnBridgeUnder(ThingDef thingDef, IntVec3 c, Rot4 rot)
		{
			if (thingDef.category != ThingCategory.Building)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			CellRect cellRect = GenAdj.OccupiedRect(c, rot, thingDef.size);
			BaseGenUtility.bridgeCells.Clear();
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec, map, Rot4.North, null, null))
				{
					BaseGenUtility.bridgeCells.Add(intVec);
				}
			}
			if (!BaseGenUtility.bridgeCells.Any<IntVec3>())
			{
				return;
			}
			if (thingDef.size.x != 1 || thingDef.size.z != 1)
			{
				for (int i = BaseGenUtility.bridgeCells.Count - 1; i >= 0; i--)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 intVec2 = BaseGenUtility.bridgeCells[i] + GenAdj.AdjacentCells[j];
						if (!BaseGenUtility.bridgeCells.Contains(intVec2) && intVec2.InBounds(map) && !intVec2.SupportsStructureType(map, thingDef.terrainAffordanceNeeded) && GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, intVec2, map, Rot4.North, null, null))
						{
							BaseGenUtility.bridgeCells.Add(intVec2);
						}
					}
				}
			}
			for (int k = 0; k < BaseGenUtility.bridgeCells.Count; k++)
			{
				map.terrainGrid.SetTerrain(BaseGenUtility.bridgeCells[k], TerrainDefOf.Bridge);
			}
		}

		// Token: 0x06008290 RID: 33424 RVA: 0x002E57C0 File Offset: 0x002E39C0
		[DebugOutput]
		private static void WallStuffs()
		{
			IEnumerable<ThingDef> dataSources = GenStuff.AllowedStuffsFor(ThingDefOf.Wall, TechLevel.Undefined);
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("cheap", (ThingDef d) => BaseGenUtility.IsCheapWallStuff(d).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("floor", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, false, null).defName);
			array[3] = new TableDataGetter<ThingDef>("floor (beautiful)", (ThingDef d) => BaseGenUtility.CorrespondingTerrainDef(d, true, null).defName);
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x002E5894 File Offset: 0x002E3A94
		public static void DoPathwayBetween(IntVec3 a, IntVec3 b, TerrainDef terrainDef, int size = 3)
		{
			foreach (IntVec3 center in GenSight.PointsOnLineOfSight(a, b))
			{
				foreach (IntVec3 c in CellRect.CenteredOn(center, size, size))
				{
					if (c.InBounds(BaseGen.globalSettings.map))
					{
						BaseGen.globalSettings.map.terrainGrid.SetTerrain(c, terrainDef);
					}
				}
			}
		}

		// Token: 0x04005137 RID: 20791
		private static List<TerrainDef> tmpFactionFloors = new List<TerrainDef>();

		// Token: 0x04005138 RID: 20792
		private static List<IntVec3> bridgeCells = new List<IntVec3>();
	}
}
