using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E1 RID: 4321
	public class ThingSetMaker_Nutrition : ThingSetMaker
	{
		// Token: 0x0600675C RID: 26460 RVA: 0x0022ECBA File Offset: 0x0022CEBA
		public ThingSetMaker_Nutrition()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x0022ECD0 File Offset: 0x0022CED0
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max <= 0f)
			{
				return false;
			}
			float maxValue;
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel ?? TechLevel.Undefined, parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1))
				{
					return false;
				}
			}
			return this.GeneratePossibleDefs(parms, out maxValue, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x0022EDDC File Offset: 0x0022CFDC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.GetStatValue(StatDefOf.Nutrition, true), maxMass, false);
			this.nextSeed++;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x0022E937 File Offset: 0x0022CB37
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x0022EE7A File Offset: 0x0022D07A
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalNutrition);
			Rand.PopState();
			return result;
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x0022EE90 File Offset: 0x0022D090
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalNutrition = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalNutritionRange ?? FloatRange.Zero;
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			QualityGenerator qualityGenerator = parms.qualityGenerator ?? QualityGenerator.BaseGen;
			totalNutrition = floatRange.RandomInRange;
			int numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
			int numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
			Func<ThingDef, float> weightSelector = (ThingDef x) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(x, numMeats, numLeathers);
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalNutrition, enumerable, techLevel, qualityGenerator, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), weightSelector, 100, maxMass, true, 0f);
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x0022F040 File Offset: 0x0022D240
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			foreach (ThingDef thingDef in this.AllowedThingDefs(parms))
			{
				if (parms.maxTotalMass != null)
				{
					float? maxTotalMass = parms.maxTotalMass;
					float maxValue = float.MaxValue;
					if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null))
					{
						float minMass = ThingSetMakerUtility.GetMinMass(thingDef, techLevel);
						maxTotalMass = parms.maxTotalMass;
						if (minMass > maxTotalMass.GetValueOrDefault() & maxTotalMass != null)
						{
							continue;
						}
					}
				}
				if (parms.totalNutritionRange == null || parms.totalNutritionRange.Value.max == 3.4028235E+38f || !thingDef.IsNutritionGivingIngestible || thingDef.ingestible.CachedNutrition <= parms.totalNutritionRange.Value.max)
				{
					yield return thingDef;
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04003A50 RID: 14928
		private int nextSeed;
	}
}
