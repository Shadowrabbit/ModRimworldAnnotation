using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200017B RID: 379
	public static class SimplifiedPastureNutritionSimulator
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x0003A598 File Offset: 0x00038798
		public static float NutritionProducedPerDay(BiomeDef biome, ThingDef plantDef, float averageGrowthRate, float mapRespawnChance)
		{
			if (Mathf.Approximately(averageGrowthRate, 0f))
			{
				return 0f;
			}
			float num = biome.wildPlantRegrowDays / mapRespawnChance;
			float num2 = plantDef.plant.growDays / averageGrowthRate * plantDef.plant.harvestMinGrowth;
			return plantDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * PlantUtility.NutritionFactorFromGrowth(plantDef, plantDef.plant.harvestMinGrowth) / (num + num2) * biome.CommonalityPctOfPlant(plantDef) * 0.85f;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0003A60B File Offset: 0x0003880B
		public static float NutritionConsumedPerDay(Pawn animal)
		{
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animal.def, animal.ageTracker.CurLifeStage);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0003A624 File Offset: 0x00038824
		public static float NutritionConsumedPerDay(ThingDef animalDef)
		{
			LifeStageAge lifeStageAge = animalDef.race.lifeStageAges.Last<LifeStageAge>();
			return SimplifiedPastureNutritionSimulator.NutritionConsumedPerDay(animalDef, lifeStageAge.def);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0003A650 File Offset: 0x00038850
		public static float NutritionConsumedPerDay(ThingDef animalDef, LifeStageDef lifeStageDef)
		{
			HungerCategory cat = HungerCategory.Hungry;
			float hungerRate = Need_Food.BaseHungerRateFactor(lifeStageDef, animalDef);
			return Need_Food.BaseFoodFallPerTickAssumingCategory(cat, hungerRate) * 60000f;
		}

		// Token: 0x04000902 RID: 2306
		public const float UnderEstimateNutritionFactor = 0.85f;
	}
}
