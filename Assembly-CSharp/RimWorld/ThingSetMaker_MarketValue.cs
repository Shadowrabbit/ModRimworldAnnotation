using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174C RID: 5964
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		// Token: 0x0600837E RID: 33662 RVA: 0x00058443 File Offset: 0x00056643
		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		// Token: 0x0600837F RID: 33663 RVA: 0x0026F868 File Offset: 0x0026DA68
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

		// Token: 0x06008380 RID: 33664 RVA: 0x0026F974 File Offset: 0x0026DB74
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float maxMass = parms.maxTotalMass ?? float.MaxValue;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue_NewTemp(outThings, totalValue, (Thing x) => x.MarketValue, maxMass, true);
			this.nextSeed++;
		}

		// Token: 0x06008381 RID: 33665 RVA: 0x000583DE File Offset: 0x000565DE
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06008382 RID: 33666 RVA: 0x00058456 File Offset: 0x00056656
		private float GetSingleThingValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		// Token: 0x06008383 RID: 33667 RVA: 0x00058464 File Offset: 0x00056664
		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.minRewardCount;
		}

		// Token: 0x06008384 RID: 33668 RVA: 0x0005847F File Offset: 0x0005667F
		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.stackLimit;
		}

		// Token: 0x06008385 RID: 33669 RVA: 0x0005849A File Offset: 0x0005669A
		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		// Token: 0x06008386 RID: 33670 RVA: 0x0026FA14 File Offset: 0x0026DC14
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
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp3(countRange, totalMarketValue, enumerable, techLevel, qualityGenerator, new Func<ThingStuffPairWithQuality, float>(this.GetMinValue), new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue), new Func<ThingStuffPairWithQuality, float>(this.GetSingleThingValue), null, 100, maxMass, parms.allowNonStackableDuplicates.GetValueOrDefault(true), totalMarketValue * (parms.minSingleItemMarketValuePct ?? 0f));
		}

		// Token: 0x06008387 RID: 33671 RVA: 0x000584AF File Offset: 0x000566AF
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

		// Token: 0x0400553E RID: 21822
		private int nextSeed;
	}
}
