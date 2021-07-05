using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A7 RID: 935
	public static class DebugOutputsEcology
	{
		// Token: 0x06001C62 RID: 7266 RVA: 0x000A7B18 File Offset: 0x000A5D18
		[DebugOutput]
		public static void Plants()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("grow days", (ThingDef d) => d.plant.growDays.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__Nutrition|0_0(d).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nutrition\n/day", (ThingDef d) => (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / d.plant.growDays).ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("fertility\nmin", (ThingDef d) => d.plant.fertilityMin.ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("fertility\nsensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("harvest\nnutrition", (ThingDef d) => DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("nutrition\n/ harvest nutrition", delegate(ThingDef d)
			{
				if (DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d) > 0f)
				{
					return (DebugOutputsEcology.<Plants>g__Nutrition|0_0(d) / DebugOutputsEcology.<Plants>g__HarvestNutrition|0_1(d)).ToString("F2");
				}
				return "";
			});
			array[8] = new TableDataGetter<ThingDef>("tree", delegate(ThingDef d)
			{
				if (!d.plant.IsTree)
				{
					return "";
				}
				return "yes";
			});
			array[9] = new TableDataGetter<ThingDef>("farm animal edible", delegate(ThingDef d)
			{
				if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(d))
				{
					return "";
				}
				return "yes";
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000A7D37 File Offset: 0x000A5F37
		[DebugOutput(true)]
		public static void PlantCurrentProportions()
		{
			PlantUtility.LogPlantProportions();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000A7D40 File Offset: 0x000A5F40
		[DebugOutput]
		public static void Biomes()
		{
			IEnumerable<BiomeDef> dataSources = from d in DefDatabase<BiomeDef>.AllDefs
			orderby d.plantDensity descending
			select d;
			TableDataGetter<BiomeDef>[] array = new TableDataGetter<BiomeDef>[12];
			array[0] = new TableDataGetter<BiomeDef>("defName", (BiomeDef d) => d.defName);
			array[1] = new TableDataGetter<BiomeDef>("animalDensity", (BiomeDef d) => d.animalDensity.ToString("F2"));
			array[2] = new TableDataGetter<BiomeDef>("plantDensity", (BiomeDef d) => d.plantDensity.ToString("F2"));
			array[3] = new TableDataGetter<BiomeDef>("tree density", (BiomeDef d) => d.TreeDensity.ToStringPercent());
			array[4] = new TableDataGetter<BiomeDef>("tree sightings\nper hour", (BiomeDef d) => d.TreeSightingsPerHourFromCaravan);
			array[5] = new TableDataGetter<BiomeDef>("diseaseMtbDays", (BiomeDef d) => d.diseaseMtbDays.ToString("F0"));
			array[6] = new TableDataGetter<BiomeDef>("movementDifficulty", delegate(BiomeDef d)
			{
				if (!d.impassable)
				{
					return d.movementDifficulty.ToString("F1");
				}
				return "-";
			});
			array[7] = new TableDataGetter<BiomeDef>("forageability", (BiomeDef d) => d.forageability.ToStringPercent());
			array[8] = new TableDataGetter<BiomeDef>("forageFood", delegate(BiomeDef d)
			{
				if (d.foragedFood == null)
				{
					return "";
				}
				return d.foragedFood.label;
			});
			array[9] = new TableDataGetter<BiomeDef>("forageable plants", (BiomeDef d) => (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false, false));
			array[10] = new TableDataGetter<BiomeDef>("wildPlantRegrowDays", (BiomeDef d) => d.wildPlantRegrowDays.ToString("F0"));
			array[11] = new TableDataGetter<BiomeDef>("wildPlantsCareAboutLocalFertility", (BiomeDef d) => d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<BiomeDef>(dataSources, array);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000A7F95 File Offset: 0x000A6195
		[DebugOutput]
		public static void BiomeAnimalsSpawnChances()
		{
			DebugOutputsEcology.BiomeAnimalsInternal(delegate(PawnKindDef k, BiomeDef b)
			{
				float num = b.CommonalityOfAnimal(k);
				if (num == 0f)
				{
					return "";
				}
				return (num / DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki))).ToStringPercent("F1");
			});
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000A7FBB File Offset: 0x000A61BB
		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b).ToStringEmptyZero("F2"));
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000A7FE4 File Offset: 0x000A61E4
		private static float ExpectedAnimalCount(PawnKindDef k, BiomeDef b)
		{
			float num = b.CommonalityOfAnimal(k);
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num3 = num / num2;
			float num4 = 10000f / b.animalDensity;
			float num5 = 62500f / num4;
			float totalCommonality = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
			float num6 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => k.ecoSystemWeight * (b.CommonalityOfAnimal(ki) / totalCommonality));
			return num5 / num6 * num3;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000A8094 File Offset: 0x000A6294
		private static void BiomeAnimalsInternal(Func<PawnKindDef, BiomeDef, string> densityInBiomeOutputter)
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, (PawnKindDef k) => densityInBiomeOutputter(k, b))).ToList<TableDataGetter<PawnKindDef>>();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName + (k.race.race.predator ? " (P)" : "")));
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000A819C File Offset: 0x000A639C
		[DebugOutput]
		public static void BiomePlantsExpectedCount()
		{
			Func<ThingDef, BiomeDef, string> expectedCountInBiomeOutputter = (ThingDef p, BiomeDef b) => (b.CommonalityOfPlant(p) * b.plantDensity * 4000f).ToString("F0");
			List<TableDataGetter<ThingDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.plantDensity
			select new TableDataGetter<ThingDef>(b.defName + " (" + b.plantDensity.ToString("F2") + ")", (ThingDef k) => expectedCountInBiomeOutputter(k, b))).ToList<TableDataGetter<ThingDef>>();
			list.Insert(0, new TableDataGetter<ThingDef>("plant", (ThingDef k) => k.defName));
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000A82C0 File Offset: 0x000A64C0
		[DebugOutput]
		public static void AnimalWildCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<PawnKindDef> dataSources = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race != null && k.RaceProps.Animal && DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome) > 0f
			orderby DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome) descending
			select k;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[3];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("expected count on map (inaccurate)", (PawnKindDef k) => DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome).ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("actual count on map", (PawnKindDef k) => (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == k
			select p).Count<Pawn>().ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000A8370 File Offset: 0x000A6570
		[DebugOutput]
		public static void PlantCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<ThingDef> dataSources = from p in DefDatabase<ThingDef>.AllDefs
			where p.category == ThingCategory.Plant && map.Biome.CommonalityOfPlant(p) > 0f
			orderby map.Biome.CommonalityOfPlant(p) descending
			select p;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef p) => p.defName);
			array[1] = new TableDataGetter<ThingDef>("biome-defined commonality", (ThingDef p) => map.Biome.CommonalityOfPlant(p).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("expected count (rough)", (ThingDef p) => (map.Biome.CommonalityOfPlant(p) * map.Biome.plantDensity * 4000f).ToString("F0"));
			array[3] = new TableDataGetter<ThingDef>("actual count on map", (ThingDef p) => (from c in map.AllCells
			where c.GetPlant(map) != null && c.GetPlant(map).def == p
			select c).Count<IntVec3>().ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000A8438 File Offset: 0x000A6638
		[DebugOutput]
		public static void BiomeRanching()
		{
			List<TerrainDef> list = new List<TerrainDef>();
			list.Add(TerrainDefOf.Soil);
			list.Add(TerrainDefOf.SoilRich);
			list.Add(TerrainDefOf.Sand);
			List<TableDataGetter<BiomeDef>> list2 = new List<TableDataGetter<BiomeDef>>();
			list2.Add(new TableDataGetter<BiomeDef>("biome", (BiomeDef b) => b.defName));
			using (List<Quadrum>.Enumerator enumerator = QuadrumUtility.Quadrums.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Quadrum quadrum = enumerator.Current;
					using (List<TerrainDef>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							TerrainDef terrain = enumerator2.Current;
							Predicate<TerrainThreshold> <>9__5;
							list2.Add(new TableDataGetter<BiomeDef>(quadrum.LabelShort() + "\nnutrition\ndaily\n/10x10\n" + terrain.defName, delegate(BiomeDef b)
							{
								List<TerrainThreshold> terrainsByFertility = b.terrainsByFertility;
								Predicate<TerrainThreshold> predicate;
								if ((predicate = <>9__5) == null)
								{
									predicate = (<>9__5 = ((TerrainThreshold tbf) => tbf.terrain == terrain));
								}
								if (!terrainsByFertility.Any(predicate))
								{
									return "";
								}
								return (DebugOutputsEcology.<BiomeRanching>g__GetAverageNutritionPerDay|10_1(quadrum, b, terrain) * 10f * 10f).ToString("F3");
							}));
						}
					}
				}
			}
			using (List<Quadrum>.Enumerator enumerator = QuadrumUtility.Quadrums.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Quadrum quadrum = enumerator.Current;
					using (List<TerrainDef>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							TerrainDef terrain = enumerator2.Current;
							Predicate<TerrainThreshold> <>9__7;
							list2.Add(new TableDataGetter<BiomeDef>(quadrum.LabelShort() + "\ncows\n/10x10\n" + terrain.defName, delegate(BiomeDef b)
							{
								List<TerrainThreshold> terrainsByFertility = b.terrainsByFertility;
								Predicate<TerrainThreshold> predicate;
								if ((predicate = <>9__7) == null)
								{
									predicate = (<>9__7 = ((TerrainThreshold tbf) => tbf.terrain == terrain));
								}
								if (!terrainsByFertility.Any(predicate))
								{
									return "";
								}
								return (DebugOutputsEcology.<BiomeRanching>g__CowsFeedPerDay|10_0(quadrum, b, terrain) * 10f * 10f).ToString("F3");
							}));
						}
					}
				}
			}
			DebugTables.MakeTablesDialog<BiomeDef>(DefDatabase<BiomeDef>.AllDefs.Where(delegate(BiomeDef b)
			{
				if (b.canBuildBase)
				{
					return b.terrainsByFertility.Any((TerrainThreshold x) => x.terrain.fertility > 0f);
				}
				return false;
			}), list2.ToArray());
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000A8688 File Offset: 0x000A6888
		[CompilerGenerated]
		internal static float <Plants>g__Nutrition|0_0(ThingDef d)
		{
			if (d.ingestible == null)
			{
				return 0f;
			}
			return d.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000A86A4 File Offset: 0x000A68A4
		[CompilerGenerated]
		internal static float <Plants>g__HarvestNutrition|0_1(ThingDef d)
		{
			if (d.plant.harvestedThingDef == null)
			{
				return 0f;
			}
			return d.plant.harvestYield * d.plant.harvestedThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000A86DB File Offset: 0x000A68DB
		[CompilerGenerated]
		internal static float <BiomeRanching>g__CowsFeedPerDay|10_0(Quadrum q, BiomeDef b, TerrainDef t)
		{
			return DebugOutputsEcology.<BiomeRanching>g__GetAverageNutritionPerDay|10_1(q, b, t) / SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(ThingDefOf.Cow);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000A86F0 File Offset: 0x000A68F0
		[CompilerGenerated]
		internal static float <BiomeRanching>g__GetAverageNutritionPerDay|10_1(Quadrum q, BiomeDef b, TerrainDef t)
		{
			int num = -1;
			for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
			{
				if (Find.WorldGrid.tiles[i].biome == b)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				Log.Error("Could not find tile on map to sample for biome: " + b.label);
				return 0f;
			}
			MapPlantGrowthRateCalculator mapPlantGrowthRateCalculator = new MapPlantGrowthRateCalculator();
			mapPlantGrowthRateCalculator.BuildFor(num);
			MapPastureNutritionCalculator mapPastureNutritionCalculator = new MapPastureNutritionCalculator();
			mapPastureNutritionCalculator.Reset(num, 0.64f, mapPlantGrowthRateCalculator);
			return mapPastureNutritionCalculator.GetAverageNutritionPerDay(q, t);
		}
	}
}
