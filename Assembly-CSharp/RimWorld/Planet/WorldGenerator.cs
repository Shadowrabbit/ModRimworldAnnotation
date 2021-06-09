using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002092 RID: 8338
	public static class WorldGenerator
	{
		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x0600B0C5 RID: 45253 RVA: 0x003352D8 File Offset: 0x003334D8
		public static IEnumerable<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				return from x in DefDatabase<WorldGenStepDef>.AllDefs
				orderby x.order, x.index
				select x;
			}
		}

		// Token: 0x0600B0C6 RID: 45254 RVA: 0x00335334 File Offset: 0x00333534
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature, OverallPopulation population)
		{
			DeepProfiler.Start("GenerateWorld");
			Rand.PushState();
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			Rand.Seed = seedFromSeedString;
			World creatingWorld;
			try
			{
				Current.CreatingWorld = new World();
				Current.CreatingWorld.info.seedString = seedString;
				Current.CreatingWorld.info.planetCoverage = planetCoverage;
				Current.CreatingWorld.info.overallRainfall = overallRainfall;
				Current.CreatingWorld.info.overallTemperature = overallTemperature;
				Current.CreatingWorld.info.overallPopulation = population;
				Current.CreatingWorld.info.name = NameGenerator.GenerateName(RulePackDefOf.NamerWorld, null, false, null, null);
				WorldGenerator.tmpGenSteps.Clear();
				WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
				for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
				{
					DeepProfiler.Start("WorldGenStep - " + WorldGenerator.tmpGenSteps[i]);
					try
					{
						Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
						WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFresh(seedString);
					}
					catch (Exception arg)
					{
						Log.Error("Error in WorldGenStep: " + arg, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
				Rand.Seed = seedFromSeedString;
				Current.CreatingWorld.grid.StandardizeTileData();
				Current.CreatingWorld.FinalizeInit();
				Find.Scenario.PostWorldGenerate();
				creatingWorld = Current.CreatingWorld;
			}
			finally
			{
				Rand.PopState();
				DeepProfiler.End();
				Current.CreatingWorld = null;
			}
			return creatingWorld;
		}

		// Token: 0x0600B0C7 RID: 45255 RVA: 0x003354F4 File Offset: 0x003336F4
		public static void GenerateWithoutWorldData(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateWithoutWorldData(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x0600B0C8 RID: 45256 RVA: 0x00335594 File Offset: 0x00333794
		public static void GenerateFromScribe(string seedString)
		{
			int seedFromSeedString = WorldGenerator.GetSeedFromSeedString(seedString);
			WorldGenerator.tmpGenSteps.Clear();
			WorldGenerator.tmpGenSteps.AddRange(WorldGenerator.GenStepsInOrder);
			Rand.PushState();
			for (int i = 0; i < WorldGenerator.tmpGenSteps.Count; i++)
			{
				try
				{
					Rand.Seed = Gen.HashCombineInt(seedFromSeedString, WorldGenerator.GetSeedPart(WorldGenerator.tmpGenSteps, i));
					WorldGenerator.tmpGenSteps[i].worldGenStep.GenerateFromScribe(seedString);
				}
				catch (Exception arg)
				{
					Log.Error("Error in WorldGenStep: " + arg, false);
				}
			}
			Rand.PopState();
		}

		// Token: 0x0600B0C9 RID: 45257 RVA: 0x00335634 File Offset: 0x00333834
		private static int GetSeedPart(List<WorldGenStepDef> genSteps, int index)
		{
			int seedPart = genSteps[index].worldGenStep.SeedPart;
			int num = 0;
			for (int i = 0; i < index; i++)
			{
				if (WorldGenerator.tmpGenSteps[i].worldGenStep.SeedPart == seedPart)
				{
					num++;
				}
			}
			return seedPart + num;
		}

		// Token: 0x0600B0CA RID: 45258 RVA: 0x00072EB1 File Offset: 0x000710B1
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x040079BA RID: 31162
		private static List<WorldGenStepDef> tmpGenSteps = new List<WorldGenStepDef>();

		// Token: 0x040079BB RID: 31163
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x040079BC RID: 31164
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x040079BD RID: 31165
		public const OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;

		// Token: 0x040079BE RID: 31166
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
	}
}
