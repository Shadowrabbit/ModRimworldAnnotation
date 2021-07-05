using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EB RID: 4331
	public static class ThingSetMakerByTotalStatUtility
	{
		// Token: 0x06006799 RID: 26521 RVA: 0x00230644 File Offset: 0x0022E844
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.4028235E+38f, bool allowNonStackableDuplicates = true, float minSingleItemValue = 0f)
		{
			ThingSetMakerByTotalStatUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new ThingSetMakerByTotalStatUtility.<>c__DisplayClass5_0();
			CS$<>8__locals1.getMinValue = getMinValue;
			CS$<>8__locals1.getMaxValue = getMaxValue;
			CS$<>8__locals1.countRange = countRange;
			CS$<>8__locals1.totalValue = totalValue;
			CS$<>8__locals1.weightSelector = weightSelector;
			CS$<>8__locals1.maxMass = maxMass;
			CS$<>8__locals1.techLevel = techLevel;
			ThingSetMakerByTotalStatUtility.minValuesTmp.Clear();
			ThingSetMakerByTotalStatUtility.maxValuesTmp.Clear();
			CS$<>8__locals1.chosen = new List<ThingStuffPairWithQuality>();
			if (CS$<>8__locals1.countRange.max <= 0)
			{
				return CS$<>8__locals1.chosen;
			}
			if (CS$<>8__locals1.countRange.min < 1)
			{
				CS$<>8__locals1.countRange.min = 1;
			}
			ThingSetMakerByTotalStatUtility.CalculateAllowedThingStuffPairs(allowed, CS$<>8__locals1.techLevel, qualityGenerator);
			CS$<>8__locals1.trashThreshold = Mathf.Max(ThingSetMakerByTotalStatUtility.GetTrashThreshold(CS$<>8__locals1.countRange, CS$<>8__locals1.totalValue, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1)), minSingleItemValue);
			ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.RemoveAll((ThingStuffPairWithQuality x) => base.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1(x) < CS$<>8__locals1.trashThreshold);
			if (!ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Any<ThingStuffPairWithQuality>())
			{
				return CS$<>8__locals1.chosen;
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			foreach (ThingStuffPairWithQuality thingStuffPairWithQuality in ThingSetMakerByTotalStatUtility.allowedThingStuffPairs)
			{
				num = Mathf.Min(num, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(thingStuffPairWithQuality));
				num2 = Mathf.Max(num2, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1(thingStuffPairWithQuality));
				if (thingStuffPairWithQuality.thing.category != ThingCategory.Pawn)
				{
					num3 = Mathf.Min(num3, ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0)));
				}
			}
			num = Mathf.Max(num, CS$<>8__locals1.trashThreshold);
			CS$<>8__locals1.totalMinValueSoFar = 0f;
			CS$<>8__locals1.totalMaxValueSoFar = 0f;
			CS$<>8__locals1.minMassSoFar = 0f;
			int num4 = 0;
			for (;;)
			{
				num4++;
				if (num4 > 10000)
				{
					break;
				}
				ThingSetMakerByTotalStatUtility.candidatesTmp.Clear();
				for (int i = 0; i < ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Count; i++)
				{
					ThingStuffPairWithQuality candidate = ThingSetMakerByTotalStatUtility.allowedThingStuffPairs[i];
					if (allowNonStackableDuplicates || candidate.thing.stackLimit != 1 || !CS$<>8__locals1.chosen.Any((ThingStuffPairWithQuality c) => c.thing == candidate.thing))
					{
						if (CS$<>8__locals1.maxMass != 3.4028235E+38f && candidate.thing.category != ThingCategory.Pawn)
						{
							float nonTrashMass = ThingSetMakerByTotalStatUtility.GetNonTrashMass(candidate, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0));
							if (CS$<>8__locals1.minMassSoFar + nonTrashMass > CS$<>8__locals1.maxMass || (CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.min && CS$<>8__locals1.minMassSoFar + num3 * (float)(CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count - 1) + nonTrashMass > CS$<>8__locals1.maxMass))
							{
								goto IL_404;
							}
						}
						if (CS$<>8__locals1.totalMinValueSoFar + Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(candidate), CS$<>8__locals1.trashThreshold) <= CS$<>8__locals1.totalValue && (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.min || CS$<>8__locals1.totalMinValueSoFar + num * (float)(CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count - 1) + Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(candidate), CS$<>8__locals1.trashThreshold) <= CS$<>8__locals1.totalValue))
						{
							ThingSetMakerByTotalStatUtility.candidatesTmp.Add(candidate);
						}
					}
					IL_404:;
				}
				if (CS$<>8__locals1.countRange.max != 2147483647 && CS$<>8__locals1.totalMaxValueSoFar < CS$<>8__locals1.totalValue * 0.5f)
				{
					ThingSetMakerByTotalStatUtility.candidatesTmpNew.Clear();
					for (int j = 0; j < ThingSetMakerByTotalStatUtility.candidatesTmp.Count; j++)
					{
						ThingStuffPairWithQuality thingStuffPairWithQuality2 = ThingSetMakerByTotalStatUtility.candidatesTmp[j];
						if (CS$<>8__locals1.totalMaxValueSoFar + num2 * (float)(CS$<>8__locals1.countRange.max - CS$<>8__locals1.chosen.Count - 1) + CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1(thingStuffPairWithQuality2) >= CS$<>8__locals1.totalValue * 0.5f)
						{
							ThingSetMakerByTotalStatUtility.candidatesTmpNew.Add(thingStuffPairWithQuality2);
						}
					}
					if (ThingSetMakerByTotalStatUtility.candidatesTmpNew.Any<ThingStuffPairWithQuality>())
					{
						ThingSetMakerByTotalStatUtility.candidatesTmp.Clear();
						ThingSetMakerByTotalStatUtility.candidatesTmp.AddRange(ThingSetMakerByTotalStatUtility.candidatesTmpNew);
					}
				}
				float maxCandidateMinValue = float.MinValue;
				for (int k = 0; k < ThingSetMakerByTotalStatUtility.candidatesTmp.Count; k++)
				{
					ThingStuffPairWithQuality t = ThingSetMakerByTotalStatUtility.candidatesTmp[k];
					maxCandidateMinValue = Mathf.Max(maxCandidateMinValue, Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(t), CS$<>8__locals1.trashThreshold));
				}
				ThingStuffPairWithQuality thingStuffPairWithQuality3;
				if (!ThingSetMakerByTotalStatUtility.candidatesTmp.TryRandomElementByWeight(delegate(ThingStuffPairWithQuality x)
				{
					float a = 1f;
					if (CS$<>8__locals1.countRange.max != 2147483647 && CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.max && CS$<>8__locals1.totalValue >= CS$<>8__locals1.totalMaxValueSoFar)
					{
						int num5 = CS$<>8__locals1.countRange.max - CS$<>8__locals1.chosen.Count;
						float b = (CS$<>8__locals1.totalValue - CS$<>8__locals1.totalMaxValueSoFar) / (float)num5;
						a = Mathf.InverseLerp(0f, b, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1(x));
					}
					float b2 = 1f;
					if (CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.min && CS$<>8__locals1.totalValue >= CS$<>8__locals1.totalMinValueSoFar)
					{
						int num6 = CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count;
						float num7 = (CS$<>8__locals1.totalValue - CS$<>8__locals1.totalMinValueSoFar) / (float)num6;
						float num8 = Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(x), CS$<>8__locals1.trashThreshold);
						if (num8 > num7)
						{
							b2 = num7 / num8;
						}
					}
					float num9 = Mathf.Max(Mathf.Min(a, b2), 1E-05f);
					if (CS$<>8__locals1.weightSelector != null)
					{
						num9 *= CS$<>8__locals1.weightSelector(x.thing);
					}
					if (CS$<>8__locals1.totalValue > CS$<>8__locals1.totalMaxValueSoFar)
					{
						int num10 = Mathf.Max(CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count, 1);
						float num11 = Mathf.InverseLerp(0f, maxCandidateMinValue * 0.85f, ThingSetMakerByTotalStatUtility.GetMaxValueWithMaxMass(x, CS$<>8__locals1.minMassSoFar, CS$<>8__locals1.maxMass, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0), new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1)) * (float)num10);
						num9 *= num11 * num11;
					}
					if (PawnWeaponGenerator.IsDerpWeapon(x.thing, x.stuff))
					{
						num9 *= 0.1f;
					}
					if (CS$<>8__locals1.techLevel != TechLevel.Undefined)
					{
						TechLevel techLevel2 = x.thing.techLevel;
						if (techLevel2 < CS$<>8__locals1.techLevel && techLevel2 <= TechLevel.Neolithic && (x.thing.IsApparel || x.thing.IsWeapon))
						{
							num9 *= 0.1f;
						}
					}
					return num9;
				}, out thingStuffPairWithQuality3))
				{
					goto IL_6C6;
				}
				CS$<>8__locals1.chosen.Add(thingStuffPairWithQuality3);
				CS$<>8__locals1.totalMinValueSoFar += Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0(thingStuffPairWithQuality3), CS$<>8__locals1.trashThreshold);
				CS$<>8__locals1.totalMaxValueSoFar += CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MaxValue|1(thingStuffPairWithQuality3);
				if (thingStuffPairWithQuality3.thing.category != ThingCategory.Pawn)
				{
					CS$<>8__locals1.minMassSoFar += ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality3, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue>g__MinValue|0));
				}
				if (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.max || (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.min && CS$<>8__locals1.totalMaxValueSoFar >= CS$<>8__locals1.totalValue * 0.9f))
				{
					goto IL_6C6;
				}
			}
			Log.Error("Too many iterations.");
			IL_6C6:
			return CS$<>8__locals1.chosen;
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x00230D30 File Offset: 0x0022EF30
		public static void IncreaseStackCountsToTotalValue(List<Thing> things, float totalValue, Func<Thing, float> getValue, float maxMass = 3.4028235E+38f, bool satisfyMinRewardCount = false)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < things.Count; i++)
			{
				num += getValue(things[i]) * (float)things[i].stackCount;
				if (!(things[i] is Pawn))
				{
					num2 += things[i].GetStatValue(StatDefOf.Mass, true) * (float)things[i].stackCount;
				}
			}
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			things.SortByDescending((Thing x) => getValue(x) / x.GetStatValue(StatDefOf.Mass, true));
			ThingSetMakerByTotalStatUtility.DistributeEvenly(things, num + (totalValue - num) * 0.1f, ref num, ref num2, getValue, (maxMass == float.MaxValue) ? float.MaxValue : (num2 + (maxMass - num2) * 0.1f), false);
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			if (satisfyMinRewardCount)
			{
				ThingSetMakerByTotalStatUtility.SatisfyMinRewardCount(things, totalValue, ref num, ref num2, getValue, maxMass);
				if (num >= totalValue || num2 >= maxMass)
				{
					return;
				}
			}
			ThingSetMakerByTotalStatUtility.DistributeEvenly(things, totalValue, ref num, ref num2, getValue, maxMass, true);
			if (num >= totalValue || num2 >= maxMass)
			{
				return;
			}
			ThingSetMakerByTotalStatUtility.GiveRemainingValueToAnything(things, totalValue, ref num, ref num2, getValue, maxMass);
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x00230E64 File Offset: 0x0022F064
		private static void DistributeEvenly(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass, bool useValueMassRatio = false)
		{
			float num = (totalValue - currentTotalValue) / (float)things.Count;
			float num2 = maxMass - currentTotalMass;
			float num3 = (maxMass == float.MaxValue) ? float.MaxValue : (num2 / (float)things.Count);
			float num4 = 0f;
			if (useValueMassRatio)
			{
				for (int i = 0; i < things.Count; i++)
				{
					num4 += getValue(things[i]) / things[i].GetStatValue(StatDefOf.Mass, true);
				}
			}
			for (int j = 0; j < things.Count; j++)
			{
				float num5 = getValue(things[j]);
				int num6 = Mathf.Min(Mathf.FloorToInt(num / num5), things[j].def.stackLimit - things[j].stackCount);
				if (maxMass != 3.4028235E+38f && !(things[j] is Pawn))
				{
					float b;
					if (useValueMassRatio)
					{
						b = num2 * (getValue(things[j]) / things[j].GetStatValue(StatDefOf.Mass, true) / num4);
					}
					else
					{
						b = num3;
					}
					num6 = Mathf.Min(num6, Mathf.FloorToInt(Mathf.Min(maxMass - currentTotalMass, b) / things[j].GetStatValue(StatDefOf.Mass, true)));
				}
				if (num6 > 0)
				{
					things[j].stackCount += num6;
					currentTotalValue += num5 * (float)num6;
					if (!(things[j] is Pawn))
					{
						currentTotalMass += things[j].GetStatValue(StatDefOf.Mass, true) * (float)num6;
					}
				}
			}
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x00231008 File Offset: 0x0022F208
		private static void SatisfyMinRewardCount(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].stackCount < things[i].def.minRewardCount)
				{
					float num = getValue(things[i]);
					int num2 = Mathf.FloorToInt((totalValue - currentTotalValue) / num);
					int num3 = Mathf.Min(new int[]
					{
						num2,
						things[i].def.stackLimit - things[i].stackCount,
						things[i].def.minRewardCount - things[i].stackCount
					});
					if (maxMass != 3.4028235E+38f && !(things[i] is Pawn))
					{
						num3 = Mathf.Min(num3, Mathf.FloorToInt((maxMass - currentTotalMass) / things[i].GetStatValue(StatDefOf.Mass, true)));
					}
					if (num3 > 0)
					{
						things[i].stackCount += num3;
						currentTotalValue += num * (float)num3;
						if (!(things[i] is Pawn))
						{
							currentTotalMass += things[i].GetStatValue(StatDefOf.Mass, true) * (float)num3;
						}
					}
				}
			}
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x00231140 File Offset: 0x0022F340
		private static void GiveRemainingValueToAnything(List<Thing> things, float totalValue, ref float currentTotalValue, ref float currentTotalMass, Func<Thing, float> getValue, float maxMass)
		{
			for (int i = 0; i < things.Count; i++)
			{
				float num = getValue(things[i]);
				int num2 = Mathf.Min(Mathf.FloorToInt((totalValue - currentTotalValue) / num), things[i].def.stackLimit - things[i].stackCount);
				if (maxMass != 3.4028235E+38f && !(things[i] is Pawn))
				{
					num2 = Mathf.Min(num2, Mathf.FloorToInt((maxMass - currentTotalMass) / things[i].GetStatValue(StatDefOf.Mass, true)));
				}
				if (num2 > 0)
				{
					things[i].stackCount += num2;
					currentTotalValue += num * (float)num2;
					if (!(things[i] is Pawn))
					{
						currentTotalMass += things[i].GetStatValue(StatDefOf.Mass, true) * (float)num2;
					}
				}
			}
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x00231228 File Offset: 0x0022F428
		private static void CalculateAllowedThingStuffPairs(IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator)
		{
			ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Clear();
			using (IEnumerator<ThingDef> enumerator = allowed.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef td = enumerator.Current;
					Predicate<ThingDef> <>9__0;
					for (int i = 0; i < 5; i++)
					{
						ThingDef td2 = td;
						Predicate<ThingDef> validator;
						if ((validator = <>9__0) == null)
						{
							validator = (<>9__0 = ((ThingDef x) => !ThingSetMakerUtility.IsDerpAndDisallowed(td, x, new QualityGenerator?(qualityGenerator))));
						}
						ThingDef stuff;
						if (GenStuff.TryRandomStuffFor(td2, out stuff, techLevel, validator))
						{
							QualityCategory quality = td.HasComp(typeof(CompQuality)) ? QualityUtility.GenerateQuality(qualityGenerator) : QualityCategory.Normal;
							ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Add(new ThingStuffPairWithQuality(td, stuff, quality));
						}
					}
				}
			}
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0023131C File Offset: 0x0022F51C
		private static float GetTrashThreshold(IntRange countRange, float totalValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			float num = GenMath.Median<ThingStuffPairWithQuality>(ThingSetMakerByTotalStatUtility.allowedThingStuffPairs, getMaxValue, 0f, 0.5f);
			int num2 = Mathf.Clamp(Mathf.CeilToInt(totalValue / num), countRange.min, countRange.max);
			return totalValue / (float)num2 * 0.2f;
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x00231364 File Offset: 0x0022F564
		private static float GetNonTrashMass(ThingStuffPairWithQuality t, float trashThreshold, Func<ThingStuffPairWithQuality, float> getSingleThingValue)
		{
			int num = Mathf.Clamp(Mathf.CeilToInt(trashThreshold / getSingleThingValue(t)), 1, t.thing.stackLimit);
			return t.GetStatValue(StatDefOf.Mass) * (float)num;
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x002313A0 File Offset: 0x0022F5A0
		private static float GetMaxValueWithMaxMass(ThingStuffPairWithQuality t, float massSoFar, float maxMass, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			if (maxMass == 3.4028235E+38f)
			{
				return getMaxValue(t);
			}
			int num = Mathf.Clamp(Mathf.FloorToInt((maxMass - massSoFar) / t.GetStatValue(StatDefOf.Mass)), 1, t.thing.stackLimit);
			return getSingleThingValue(t) * (float)num;
		}

		// Token: 0x04003A64 RID: 14948
		private static List<ThingStuffPairWithQuality> allowedThingStuffPairs = new List<ThingStuffPairWithQuality>();

		// Token: 0x04003A65 RID: 14949
		private static List<ThingStuffPairWithQuality> candidatesTmp = new List<ThingStuffPairWithQuality>();

		// Token: 0x04003A66 RID: 14950
		private static List<ThingStuffPairWithQuality> candidatesTmpNew = new List<ThingStuffPairWithQuality>();

		// Token: 0x04003A67 RID: 14951
		private static Dictionary<ThingStuffPairWithQuality, float> minValuesTmp = new Dictionary<ThingStuffPairWithQuality, float>();

		// Token: 0x04003A68 RID: 14952
		private static Dictionary<ThingStuffPairWithQuality, float> maxValuesTmp = new Dictionary<ThingStuffPairWithQuality, float>();
	}
}
