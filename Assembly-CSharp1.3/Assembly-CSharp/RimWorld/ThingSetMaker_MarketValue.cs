using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E0 RID: 4320
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		// Token: 0x06006752 RID: 26450 RVA: 0x0022E956 File Offset: 0x0022CB56
		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x0022E96C File Offset: 0x0022CB6C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			List<ThingDef> list = this.AllowedThingDefs(parms).ToList<ThingDef>();
			if (!list.Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max <= 0f)
			{
				return false;
			}
			float maxValue;
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(list, parms.techLevel ?? TechLevel.Undefined, parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.min : 1))
				{
					return false;
				}
			}
			return this.GeneratePossibleDefs(parms, out maxValue, this.nextSeed).Any<ThingStuffPairWithQuality>();
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x0022EA78 File Offset: 0x0022CC78
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.MarketValue, maxMass, true);
			this.nextSeed++;
		}

		// Token: 0x06006755 RID: 26453 RVA: 0x0022E937 File Offset: 0x0022CB37
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x0022EB16 File Offset: 0x0022CD16
		private float GetSingleThingValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x0022EB24 File Offset: 0x0022CD24
		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.minRewardCount;
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x0022EB3F File Offset: 0x0022CD3F
		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.stackLimit;
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x0022EB5A File Offset: 0x0022CD5A
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x0022EB70 File Offset: 0x0022CD70
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalMarketValue = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
			FloatRange floatRange = parms.totalMarketValueRange ?? FloatRange.Zero;
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			QualityGenerator qualityGenerator = parms.qualityGenerator ?? QualityGenerator.BaseGen;
			totalMarketValue = floatRange.RandomInRange;
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalMarketValue, enumerable, techLevel, qualityGenerator, new Func<ThingStuffPairWithQuality, float>(this.GetMinValue), new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue), new Func<ThingStuffPairWithQuality, float>(this.GetSingleThingValue), null, 100, maxMass, parms.allowNonStackableDuplicates.GetValueOrDefault(true), totalMarketValue * (parms.minSingleItemMarketValuePct ?? 0f));
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x0022ECA3 File Offset: 0x0022CEA3
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
				if (parms.totalMarketValueRange == null || parms.totalMarketValueRange.Value.max == 3.4028235E+38f || ThingSetMakerUtility.GetMinMarketValue(thingDef, techLevel) <= parms.totalMarketValueRange.Value.max)
				{
					yield return thingDef;
				}
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04003A4F RID: 14927
		private int nextSeed;
	}
}
