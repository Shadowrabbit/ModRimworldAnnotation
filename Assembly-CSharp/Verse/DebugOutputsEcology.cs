using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020005FF RID: 1535
	public static class DebugOutputsEcology
	{
		// Token: 0x060025F3 RID: 9715 RVA: 0x001185E0 File Offset: 0x001167E0
		[DebugOutput]
		public static void PlantsBasics()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("growDays", (ThingDef d) => d.plant.growDays.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", delegate(ThingDef d)
			{
				if (d.ingestible == null)
				{
					return "-";
				}
				return d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
			});
			array[3] = new TableDataGetter<ThingDef>("nut/day", delegate(ThingDef d)
			{
				if (d.ingestible == null)
				{
					return "-";
				}
				return (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F4");
			});
			array[4] = new TableDataGetter<ThingDef>("fertilityMin", (ThingDef d) => d.plant.fertilityMin.ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("fertilitySensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0001F182 File Offset: 0x0001D382
		[DebugOutput(true)]
		public static void PlantCurrentProportions()
		{
			PlantUtility.LogPlantProportions();
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00118750 File Offset: 0x00116950
		[DebugOutput]
		public static void Biomes()
		{
			IEnumerable<BiomeDef> dataSources = from d in DefDatabase<BiomeDef>.AllDefs
			orderby d.plantDensity descending
			select d;
			TableDataGetter<BiomeDef>[] array = new TableDataGetter<BiomeDef>[10];
			array[0] = new TableDataGetter<BiomeDef>("defName", (BiomeDef d) => d.defName);
			array[1] = new TableDataGetter<BiomeDef>("animalDensity", (BiomeDef d) => d.animalDensity.ToString("F2"));
			array[2] = new TableDataGetter<BiomeDef>("plantDensity", (BiomeDef d) => d.plantDensity.ToString("F2"));
			array[3] = new TableDataGetter<BiomeDef>("diseaseMtbDays", (BiomeDef d) => d.diseaseMtbDays.ToString("F0"));
			array[4] = new TableDataGetter<BiomeDef>("movementDifficulty", delegate(BiomeDef d)
			{
				if (!d.impassable)
				{
					return d.movementDifficulty.ToString("F1");
				}
				return "-";
			});
			array[5] = new TableDataGetter<BiomeDef>("forageability", (BiomeDef d) => d.forageability.ToStringPercent());
			array[6] = new TableDataGetter<BiomeDef>("forageFood", delegate(BiomeDef d)
			{
				if (d.foragedFood == null)
				{
					return "";
				}
				return d.foragedFood.label;
			});
			array[7] = new TableDataGetter<BiomeDef>("forageable plants", (BiomeDef d) => (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false));
			array[8] = new TableDataGetter<BiomeDef>("wildPlantRegrowDays", (BiomeDef d) => d.wildPlantRegrowDays.ToString("F0"));
			array[9] = new TableDataGetter<BiomeDef>("wildPlantsCareAboutLocalFertility", (BiomeDef d) => d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<BiomeDef>(dataSources, array);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0001F189 File Offset: 0x0001D389
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

		// Token: 0x060025F7 RID: 9719 RVA: 0x0001F1AF File Offset: 0x0001D3AF
		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b).ToStringEmptyZero("F2"));
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0011894C File Offset: 0x00116B4C
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

		// Token: 0x060025F9 RID: 9721 RVA: 0x001189FC File Offset: 0x00116BFC
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

		// Token: 0x060025FA RID: 9722 RVA: 0x00118B04 File Offset: 0x00116D04
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

		// Token: 0x060025FB RID: 9723 RVA: 0x00118C28 File Offset: 0x00116E28
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

		// Token: 0x060025FC RID: 9724 RVA: 0x00118CD8 File Offset: 0x00116ED8
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
	}
}
