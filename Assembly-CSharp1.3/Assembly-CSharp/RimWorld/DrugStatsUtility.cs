using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009FC RID: 2556
	public static class DrugStatsUtility
	{
		// Token: 0x06003ED5 RID: 16085 RVA: 0x001575AF File Offset: 0x001557AF
		public static CompProperties_Drug GetDrugComp(ThingDef d)
		{
			return d.GetCompProperties<CompProperties_Drug>();
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x001575B7 File Offset: 0x001557B7
		public static ChemicalDef GetChemical(ThingDef d)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null)
			{
				return null;
			}
			return drugComp.chemical;
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x001575CA File Offset: 0x001557CA
		public static NeedDef GetNeed(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			HediffDef addictionHediff = chemical.addictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			return addictionHediff.causesNeed;
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x001575E8 File Offset: 0x001557E8
		public static HediffDef GetTolerance(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			if (chemical == null)
			{
				return null;
			}
			return chemical.toleranceHediff;
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x001575FC File Offset: 0x001557FC
		public static float GetAddictivenessAtTolerance(ThingDef d, float tolerance)
		{
			float num = DrugStatsUtility.GetDrugComp(d).addictiveness;
			if (DrugStatsUtility.GetTolerance(d) != null && tolerance > 0f)
			{
				num *= DrugStatsUtility.ToleranceToAddictivenessFactorCurve.Evaluate(tolerance);
			}
			return num;
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x00157634 File Offset: 0x00155834
		public static IngestionOutcomeDoer_GiveHediff GetDrugHighGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			using (List<IngestionOutcomeDoer>.Enumerator enumerator = d.ingestible.outcomeDoers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff;
					if ((ingestionOutcomeDoer_GiveHediff = (enumerator.Current as IngestionOutcomeDoer_GiveHediff)) != null && typeof(Hediff_High).IsAssignableFrom(ingestionOutcomeDoer_GiveHediff.hediffDef.hediffClass))
					{
						return ingestionOutcomeDoer_GiveHediff;
					}
				}
			}
			return null;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x001576C8 File Offset: 0x001558C8
		public static IngestionOutcomeDoer_GiveHediff GetToleranceGiver(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return null;
			}
			using (List<IngestionOutcomeDoer>.Enumerator enumerator = d.ingestible.outcomeDoers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff;
					if ((ingestionOutcomeDoer_GiveHediff = (enumerator.Current as IngestionOutcomeDoer_GiveHediff)) != null && ingestionOutcomeDoer_GiveHediff.hediffDef == DrugStatsUtility.GetTolerance(d))
					{
						return ingestionOutcomeDoer_GiveHediff;
					}
				}
			}
			return null;
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x00157750 File Offset: 0x00155950
		public static float GetHighOffsetPerDay(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff drugHighGiver = DrugStatsUtility.GetDrugHighGiver(d);
			if (drugHighGiver == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = drugHighGiver.hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00157788 File Offset: 0x00155988
		public static float GetToleranceGain(ThingDef d)
		{
			if (d.ingestible == null || d.ingestible.outcomeDoers == null)
			{
				return 0f;
			}
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance != null)
			{
				using (List<IngestionOutcomeDoer>.Enumerator enumerator = d.ingestible.outcomeDoers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff;
						if ((ingestionOutcomeDoer_GiveHediff = (enumerator.Current as IngestionOutcomeDoer_GiveHediff)) != null && ingestionOutcomeDoer_GiveHediff.hediffDef == tolerance)
						{
							return ingestionOutcomeDoer_GiveHediff.severity;
						}
					}
				}
			}
			return 0f;
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x00157820 File Offset: 0x00155A20
		public static float GetToleranceOffsetPerDay(ThingDef d)
		{
			HediffDef tolerance = DrugStatsUtility.GetTolerance(d);
			if (tolerance == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = tolerance.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00157854 File Offset: 0x00155A54
		public static float GetAddictionOffsetPerDay(ThingDef d)
		{
			ChemicalDef chemical = DrugStatsUtility.GetChemical(d);
			HediffDef hediffDef = (chemical != null) ? chemical.addictionHediff : null;
			if (hediffDef == null)
			{
				return 0f;
			}
			HediffCompProperties_SeverityPerDay hediffCompProperties_SeverityPerDay = hediffDef.CompProps<HediffCompProperties_SeverityPerDay>();
			if (hediffCompProperties_SeverityPerDay == null)
			{
				return 0f;
			}
			return hediffCompProperties_SeverityPerDay.severityPerDay;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x00157894 File Offset: 0x00155A94
		public static float GetAddictionNeedCostPerDay(ThingDef d)
		{
			NeedDef need = DrugStatsUtility.GetNeed(d);
			if (need != null)
			{
				return d.BaseMarketValue * need.fallPerDay * (1f + (1f - DrugStatsUtility.GetDrugComp(d).needLevelOffset));
			}
			return 0f;
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x001578D8 File Offset: 0x00155AD8
		public static float GetSafeDoseInterval(ThingDef d)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(d);
			if (drugComp == null || !drugComp.Addictive)
			{
				return 0f;
			}
			if (drugComp.addictiveness >= 1f || DrugStatsUtility.GetToleranceGiver(d) == null)
			{
				return -1f;
			}
			float num = Mathf.Abs(DrugStatsUtility.GetToleranceOffsetPerDay(d));
			return Mathf.Max(drugComp.overdoseSeverityOffset.TrueMax, (num > 0f) ? (DrugStatsUtility.GetToleranceGiver(d).severity / num) : -1f);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x00157950 File Offset: 0x00155B50
		public static string GetSafeDoseIntervalReadout(ThingDef d)
		{
			IngestionOutcomeDoer_GiveHediff toleranceGiver = DrugStatsUtility.GetToleranceGiver(d);
			float safeDoseInterval = DrugStatsUtility.GetSafeDoseInterval(d);
			float num = (toleranceGiver != null) ? (DrugStatsUtility.GetDrugComp(d).minToleranceToAddict / toleranceGiver.severity) : 0f;
			string result;
			if (safeDoseInterval == 0f)
			{
				result = "AlwaysSafe".Translate();
			}
			else if (num < 1f)
			{
				result = "NeverSafe".Translate();
			}
			else
			{
				result = "PeriodDays".Translate(safeDoseInterval.ToString("F1"));
			}
			return result;
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x001579DE File Offset: 0x00155BDE
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef def)
		{
			CompProperties_Drug drugComp = DrugStatsUtility.GetDrugComp(def);
			if (drugComp == null)
			{
				yield break;
			}
			IngestionOutcomeDoer_GiveHediff highGiver = DrugStatsUtility.GetDrugHighGiver(def);
			if (highGiver != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighGain".Translate(), highGiver.severity.ToStringPercent(), "Stat_Thing_Drug_HighGainPerDose_Desc".Translate(), 2480, null, null, false);
				float highFall = Mathf.Abs(DrugStatsUtility.GetHighOffsetPerDay(def));
				if (highFall > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighFallRate".Translate(), "PerDay".Translate(highFall.ToStringPercent()), "Stat_Thing_Drug_HighFallPerDay_Desc".Translate(), 2470, null, null, false);
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "HighDuration".Translate(), "PeriodDays".Translate((highGiver.severity / highFall).ToString("F1")), "Stat_Thing_Drug_HighDurationPerDose_Desc".Translate(), 2460, null, null, false);
				}
			}
			if (DrugStatsUtility.GetTolerance(def) != null)
			{
				float toleranceGain = DrugStatsUtility.GetToleranceGain(def);
				if (toleranceGain > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceGain".Translate(), toleranceGain.ToStringPercent(), "Stat_Thing_Drug_ToleranceGainPerDose_Desc".Translate(), 2450, null, null, false);
				}
				float num = Mathf.Abs(DrugStatsUtility.GetToleranceOffsetPerDay(def));
				if (num > 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceFallRate".Translate(), "PerDay".Translate(num.ToStringPercent()), "Stat_Thing_Drug_ToleranceFallPerDay_Desc".Translate(), 2440, null, null, false);
				}
			}
			if (drugComp.Addictive)
			{
				ChemicalDef chemical = DrugStatsUtility.GetChemical(def);
				HediffDef addictionHediff = (chemical != null) ? chemical.addictionHediff : null;
				if (addictionHediff != null)
				{
					float num2 = Mathf.Abs(DrugStatsUtility.GetAddictionOffsetPerDay(def));
					if (num2 > 0f)
					{
						float num3 = addictionHediff.initialSeverity / num2;
						yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionRecoveryTime".Translate(), "PeriodDays".Translate(num3.ToString("F1")), "Stat_Thing_Drug_AddictionRecoveryTime_Desc".Translate(), 2395, null, null, false);
					}
					yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityInitial".Translate(), addictionHediff.initialSeverity.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityInitial_Desc".Translate(), 2427, null, null, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedFallRate".Translate(), "PerDay".Translate(DrugStatsUtility.GetNeed(def).fallPerDay.ToStringPercent()), "Stat_Thing_Drug_AddictionNeedFallRate_Desc".Translate(), 2410, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionCost".Translate(), "PerDay".Translate(DrugStatsUtility.GetAddictionNeedCostPerDay(def).ToStringMoney(null)), "Stat_Thing_Drug_AddictionCost_Desc".Translate(), 2390, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedDoseInterval".Translate(), "PeriodDays".Translate((drugComp.needLevelOffset / DrugStatsUtility.GetNeed(def).fallPerDay).ToString("F1")), "Stat_Thing_Drug_AddictionNeedDoseInterval_Desc".Translate(), 2400, null, null, false);
				if (drugComp.chemical != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Chemical".Translate(), drugComp.chemical.LabelCap, "Stat_Thing_Drug_Chemical_Desc".Translate(), 2490, null, null, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "Addictiveness".Translate(), drugComp.addictiveness.ToStringPercent(), "Stat_Thing_Drug_Addictiveness_Desc".Translate(), 2428, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedOffset".Translate(), drugComp.needLevelOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionNeedOffset_Desc".Translate(), 2420, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "MinimumToleranceForAddiction".Translate(), drugComp.minToleranceToAddict.ToStringPercent(), "Stat_Thing_Drug_MinToleranceForAddiction_Desc".Translate(), 2437, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityPerDose".Translate(), drugComp.existingAddictionSeverityOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityPerDose_Desc".Translate(), 2424, null, null, false);
				addictionHediff = null;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, "RandomODChance".Translate(), drugComp.largeOverdoseChance.ToStringPercent(), "Stat_Thing_Drug_RandomODChance_Desc".Translate(), 2380, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Drug, "SafeDoseInterval".Translate(), DrugStatsUtility.GetSafeDoseIntervalReadout(def), "Stat_Thing_Drug_SafeDoseInterval_Desc".Translate(), 2435, null, null, false);
			yield break;
		}

		// Token: 0x040021A8 RID: 8616
		public static readonly SimpleCurve ToleranceToAddictivenessFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 5f),
				true
			},
			{
				new CurvePoint(0.8f, 15f),
				true
			}
		};
	}
}
