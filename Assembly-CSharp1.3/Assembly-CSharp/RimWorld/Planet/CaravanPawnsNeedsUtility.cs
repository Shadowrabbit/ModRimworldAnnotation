using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AB RID: 6059
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x06008C79 RID: 35961 RVA: 0x00326EF0 File Offset: 0x003250F0
		public static bool CanEatForNutritionEver(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.WillEat(food, null, false) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler()) && food.ingestible.canAutoSelectAsFoodForCaravan && (!food.IsDrug || new HistoryEvent(HistoryEventDefOf.IngestedDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo()) && (!food.IsNonMedicalDrug || new HistoryEvent(HistoryEventDefOf.IngestedRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo()) && (!food.IsDrug || food.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo());
		}

		// Token: 0x06008C7A RID: 35962 RVA: 0x00326FC3 File Offset: 0x003251C3
		public static bool CanEatForNutritionNow(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEatForNutritionEver(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x06008C7B RID: 35963 RVA: 0x00327001 File Offset: 0x00325201
		public static bool CanEatForNutritionNow(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanEatForNutritionNow(food.def, pawn);
		}

		// Token: 0x06008C7C RID: 35964 RVA: 0x00327020 File Offset: 0x00325220
		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true));
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable != null) ? compRottable.TicksUntilRotAtCurrentTemp : int.MaxValue;
				float a2 = 1f - (float)Mathf.Min(a, 3600000) / 3600000f;
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		// Token: 0x06008C7D RID: 35965 RVA: 0x00327094 File Offset: 0x00325294
		public static float GetFoodScore(ThingDef food, Pawn pawn, float singleFoodNutrition)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return (float)food.ingestible.preferability;
			}
			float num = 0f;
			if (food == ThingDefOf.Kibble || food == ThingDefOf.Hay)
			{
				num = 5f;
			}
			else if (food.ingestible.preferability == FoodPreferability.DesperateOnlyForHumanlikes)
			{
				num = 4f;
			}
			else if (food.ingestible.preferability == FoodPreferability.RawBad)
			{
				num = 3f;
			}
			else if (food.ingestible.preferability == FoodPreferability.RawTasty)
			{
				num = 2f;
			}
			else if (food.ingestible.preferability < FoodPreferability.MealAwful)
			{
				num = 1f;
			}
			return num + Mathf.Min(singleFoodNutrition / 100f, 0.999f);
		}
	}
}
