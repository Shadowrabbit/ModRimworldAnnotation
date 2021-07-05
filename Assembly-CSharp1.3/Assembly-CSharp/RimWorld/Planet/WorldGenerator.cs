using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200178D RID: 6029
	public static class WorldGenerator
	{
		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06008B20 RID: 35616 RVA: 0x0031F864 File Offset: 0x0031DA64
		public static IEnumerable<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				return from x in DefDatabase<WorldGenStepDef>.AllDefs
				orderby x.order, x.index
				select x;
			}
		}

		// Token: 0x06008B21 RID: 35617 RVA: 0x0031F8C0 File Offset: 0x0031DAC0
		public static World GenerateWorld(float planetCoverage, string seedString, OverallRainfall overallRainfall, OverallTemperature overallTemperature, OverallPopulation population, Dictionary<FactionDef, int> factionCounts = null)
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
				Current.CreatingWorld.info.factionCounts = factionCounts;
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
						Log.Error("Error in WorldGenStep: " + arg);
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
				if (!ModsConfig.IdeologyActive)
				{
					Find.Scenario.PostIdeoChosen();
				}
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

		// Token: 0x06008B22 RID: 35618 RVA: 0x0031FAA4 File Offset: 0x0031DCA4
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
					Log.Error("Error in WorldGenStep: " + arg);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06008B23 RID: 35619 RVA: 0x0031FB44 File Offset: 0x0031DD44
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
					Log.Error("Error in WorldGenStep: " + arg);
				}
			}
			Rand.PopState();
		}

		// Token: 0x06008B24 RID: 35620 RVA: 0x0031FBE4 File Offset: 0x0031DDE4
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

		// Token: 0x06008B25 RID: 35621 RVA: 0x0031FC30 File Offset: 0x0031DE30
		private static int GetSeedFromSeedString(string seedString)
		{
			return GenText.StableStringHash(seedString);
		}

		// Token: 0x040058AE RID: 22702
		private static List<WorldGenStepDef> tmpGenSteps = new List<WorldGenStepDef>();

		// Token: 0x040058AF RID: 22703
		public const float DefaultPlanetCoverage = 0.3f;

		// Token: 0x040058B0 RID: 22704
		public const OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;

		// Token: 0x040058B1 RID: 22705
		public const OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;

		// Token: 0x040058B2 RID: 22706
		public const OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
	}
}
