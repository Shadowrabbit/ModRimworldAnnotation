using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200176D RID: 5997
	public static class ThingSetMakerByTotalStatUtility
	{
		// Token: 0x06008436 RID: 33846 RVA: 0x00272288 File Offset: 0x00270488
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.4028235E+38f)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, weightSelector, tries, maxMass, true);
		}

		// Token: 0x06008437 RID: 33847 RVA: 0x002722AC File Offset: 0x002704AC
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.4028235E+38f, bool allowNonStackableDuplicates = true)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp2(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, weightSelector, tries, maxMass, allowNonStackableDuplicates, 0f);
		}

		// Token: 0x06008438 RID: 33848 RVA: 0x002722D8 File Offset: 0x002704D8
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp2(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.4028235E+38f, bool allowNonStackableDuplicates = true, float minSingleItemValue = 0f)
		{
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue_NewTmp3(countRange, totalValue, allowed, techLevel, qualityGenerator, getMinValue, getMaxValue, getMinValue, weightSelector, tries, maxMass, allowNonStackableDuplicates, minSingleItemValue);
		}

		// Token: 0x06008439 RID: 33849 RVA: 0x00272300 File Offset: 0x00270500
		public static List<ThingStuffPairWithQuality> GenerateDefsWithPossibleTotalValue_NewTmp3(IntRange countRange, float totalValue, IEnumerable<ThingDef> allowed, TechLevel techLevel, QualityGenerator qualityGenerator, Func<ThingStuffPairWithQuality, float> getMinValue, Func<ThingStuffPairWithQuality, float> getMaxValue, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingDef, float> weightSelector = null, int tries = 100, float maxMass = 3.4028235E+38f, bool allowNonStackableDuplicates = true, float minSingleItemValue = 0f)
		{
			ThingSetMakerByTotalStatUtility.<>c__DisplayClass8_0 CS$<>8__locals1 = new ThingSetMakerByTotalStatUtility.<>c__DisplayClass8_0();
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
			CS$<>8__locals1.trashThreshold = Mathf.Max(ThingSetMakerByTotalStatUtility.GetTrashThreshold(CS$<>8__locals1.countRange, CS$<>8__locals1.totalValue, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1)), minSingleItemValue);
			ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.RemoveAll((ThingStuffPairWithQuality x) => base.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1(x) < CS$<>8__locals1.trashThreshold);
			if (!ThingSetMakerByTotalStatUtility.allowedThingStuffPairs.Any<ThingStuffPairWithQuality>())
			{
				return CS$<>8__locals1.chosen;
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			foreach (ThingStuffPairWithQuality thingStuffPairWithQuality in ThingSetMakerByTotalStatUtility.allowedThingStuffPairs)
			{
				num = Mathf.Min(num, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(thingStuffPairWithQuality));
				num2 = Mathf.Max(num2, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1(thingStuffPairWithQuality));
				if (thingStuffPairWithQuality.thing.category != ThingCategory.Pawn)
				{
					num3 = Mathf.Min(num3, ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0)));
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
							float nonTrashMass = ThingSetMakerByTotalStatUtility.GetNonTrashMass(candidate, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0));
							if (CS$<>8__locals1.minMassSoFar + nonTrashMass > CS$<>8__locals1.maxMass || (CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.min && CS$<>8__locals1.minMassSoFar + num3 * (float)(CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count - 1) + nonTrashMass > CS$<>8__locals1.maxMass))
							{
								goto IL_405;
							}
						}
						if (CS$<>8__locals1.totalMinValueSoFar + Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(candidate), CS$<>8__locals1.trashThreshold) <= CS$<>8__locals1.totalValue && (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.min || CS$<>8__locals1.totalMinValueSoFar + num * (float)(CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count - 1) + Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(candidate), CS$<>8__locals1.trashThreshold) <= CS$<>8__locals1.totalValue))
						{
							ThingSetMakerByTotalStatUtility.candidatesTmp.Add(candidate);
						}
					}
					IL_405:;
				}
				if (CS$<>8__locals1.countRange.max != 2147483647 && CS$<>8__locals1.totalMaxValueSoFar < CS$<>8__locals1.totalValue * 0.5f)
				{
					ThingSetMakerByTotalStatUtility.candidatesTmpNew.Clear();
					for (int j = 0; j < ThingSetMakerByTotalStatUtility.candidatesTmp.Count; j++)
					{
						ThingStuffPairWithQuality thingStuffPairWithQuality2 = ThingSetMakerByTotalStatUtility.candidatesTmp[j];
						if (CS$<>8__locals1.totalMaxValueSoFar + num2 * (float)(CS$<>8__locals1.countRange.max - CS$<>8__locals1.chosen.Count - 1) + CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1(thingStuffPairWithQuality2) >= CS$<>8__locals1.totalValue * 0.5f)
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
					maxCandidateMinValue = Mathf.Max(maxCandidateMinValue, Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(t), CS$<>8__locals1.trashThreshold));
				}
				ThingStuffPairWithQuality thingStuffPairWithQuality3;
				if (!ThingSetMakerByTotalStatUtility.candidatesTmp.TryRandomElementByWeight(delegate(ThingStuffPairWithQuality x)
				{
					float a = 1f;
					if (CS$<>8__locals1.countRange.max != 2147483647 && CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.max && CS$<>8__locals1.totalValue >= CS$<>8__locals1.totalMaxValueSoFar)
					{
						int num5 = CS$<>8__locals1.countRange.max - CS$<>8__locals1.chosen.Count;
						float b = (CS$<>8__locals1.totalValue - CS$<>8__locals1.totalMaxValueSoFar) / (float)num5;
						a = Mathf.InverseLerp(0f, b, CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1(x));
					}
					float b2 = 1f;
					if (CS$<>8__locals1.chosen.Count < CS$<>8__locals1.countRange.min && CS$<>8__locals1.totalValue >= CS$<>8__locals1.totalMinValueSoFar)
					{
						int num6 = CS$<>8__locals1.countRange.min - CS$<>8__locals1.chosen.Count;
						float num7 = (CS$<>8__locals1.totalValue - CS$<>8__locals1.totalMinValueSoFar) / (float)num6;
						float num8 = Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(x), CS$<>8__locals1.trashThreshold);
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
						float num11 = Mathf.InverseLerp(0f, maxCandidateMinValue * 0.85f, ThingSetMakerByTotalStatUtility.GetMaxValueWithMaxMass(x, CS$<>8__locals1.minMassSoFar, CS$<>8__locals1.maxMass, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0), new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1)) * (float)num10);
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
					goto IL_6C7;
				}
				CS$<>8__locals1.chosen.Add(thingStuffPairWithQuality3);
				CS$<>8__locals1.totalMinValueSoFar += Mathf.Max(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0(thingStuffPairWithQuality3), CS$<>8__locals1.trashThreshold);
				CS$<>8__locals1.totalMaxValueSoFar += CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MaxValue|1(thingStuffPairWithQuality3);
				if (thingStuffPairWithQuality3.thing.category != ThingCategory.Pawn)
				{
					CS$<>8__locals1.minMassSoFar += ThingSetMakerByTotalStatUtility.GetNonTrashMass(thingStuffPairWithQuality3, CS$<>8__locals1.trashThreshold, new Func<ThingStuffPairWithQuality, float>(CS$<>8__locals1.<GenerateDefsWithPossibleTotalValue_NewTmp3>g__MinValue|0));
				}
				if (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.max || (CS$<>8__locals1.chosen.Count >= CS$<>8__locals1.countRange.min && CS$<>8__locals1.totalMaxValueSoFar >= CS$<>8__locals1.totalValue * 0.9f))
				{
					goto IL_6C7;
				}
			}
			Log.Error("Too many iterations.", false);
			IL_6C7:
			return CS$<>8__locals1.chosen;
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x000589BF File Offset: 0x00056BBF
		[Obsolete]
		public static void IncreaseStackCountsToTotalValue(List<Thing> things, float totalValue, Func<Thing, float> getValue, float maxMass = 3.4028235E+38f)
		{
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue_NewTemp(things, totalValue, getValue, maxMass, false);
		}

		// Token: 0x0600843B RID: 33851 RVA: 0x002729EC File Offset: 0x00270BEC
		public static void IncreaseStackCountsToTotalValue_NewTemp(List<Thing> things, float totalValue, Func<Thing, float> getValue, float maxMass = 3.4028235E+38f, bool satisfyMinRewardCount = false)
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

		// Token: 0x0600843C RID: 33852 RVA: 0x00272B20 File Offset: 0x00270D20
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

		// Token: 0x0600843D RID: 33853 RVA: 0x00272CC4 File Offset: 0x00270EC4
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

		// Token: 0x0600843E RID: 33854 RVA: 0x00272DFC File Offset: 0x00270FFC
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

		// Token: 0x0600843F RID: 33855 RVA: 0x00272EE4 File Offset: 0x002710E4
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

		// Token: 0x06008440 RID: 33856 RVA: 0x00272FD8 File Offset: 0x002711D8
		private static float GetTrashThreshold(IntRange countRange, float totalValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			float num = GenMath.Median<ThingStuffPairWithQuality>(ThingSetMakerByTotalStatUtility.allowedThingStuffPairs, getMaxValue, 0f, 0.5f);
			int num2 = Mathf.Clamp(Mathf.CeilToInt(totalValue / num), countRange.min, countRange.max);
			return totalValue / (float)num2 * 0.2f;
		}

		// Token: 0x06008441 RID: 33857 RVA: 0x00273020 File Offset: 0x00271220
		private static float GetNonTrashMass(ThingStuffPairWithQuality t, float trashThreshold, Func<ThingStuffPairWithQuality, float> getSingleThingValue)
		{
			int num = Mathf.Clamp(Mathf.CeilToInt(trashThreshold / getSingleThingValue(t)), 1, t.thing.stackLimit);
			return t.GetStatValue(StatDefOf.Mass) * (float)num;
		}

		// Token: 0x06008442 RID: 33858 RVA: 0x0027305C File Offset: 0x0027125C
		private static float GetMaxValueWithMaxMass(ThingStuffPairWithQuality t, float massSoFar, float maxMass, Func<ThingStuffPairWithQuality, float> getSingleThingValue, Func<ThingStuffPairWithQuality, float> getMaxValue)
		{
			if (maxMass == 3.4028235E+38f)
			{
				return getMaxValue(t);
			}
			int num = Mathf.Clamp(Mathf.FloorToInt((maxMass - massSoFar) / t.GetStatValue(StatDefOf.Mass)), 1, t.thing.stackLimit);
			return getSingleThingValue(t) * (float)num;
		}

		// Token: 0x040055AE RID: 21934
		private static List<ThingStuffPairWithQuality> allowedThingStuffPairs = new List<ThingStuffPairWithQuality>();

		// Token: 0x040055AF RID: 21935
		private static List<ThingStuffPairWithQuality> candidatesTmp = new List<ThingStuffPairWithQuality>();

		// Token: 0x040055B0 RID: 21936
		private static List<ThingStuffPairWithQuality> candidatesTmpNew = new List<ThingStuffPairWithQuality>();

		// Token: 0x040055B1 RID: 21937
		private static Dictionary<ThingStuffPairWithQuality, float> minValuesTmp = new Dictionary<ThingStuffPairWithQuality, float>();

		// Token: 0x040055B2 RID: 21938
		private static Dictionary<ThingStuffPairWithQuality, float> maxValuesTmp = new Dictionary<ThingStuffPairWithQuality, float>();
	}
}
