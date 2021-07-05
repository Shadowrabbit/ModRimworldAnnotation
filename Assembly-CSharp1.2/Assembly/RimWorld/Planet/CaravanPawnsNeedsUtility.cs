using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020FD RID: 8445
	public static class CaravanPawnsNeedsUtility
	{
		// Token: 0x0600B35F RID: 45919 RVA: 0x0007486B File Offset: 0x00072A6B
		public static bool CanEatForNutritionEver(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.WillEat(food, null, false) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler());
		}

		// Token: 0x0600B360 RID: 45920 RVA: 0x000748A3 File Offset: 0x00072AA3
		public static bool CanEatForNutritionNow(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEatForNutritionEver(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		// Token: 0x0600B361 RID: 45921 RVA: 0x000748E1 File Offset: 0x00072AE1
		public static bool CanEatForNutritionNow(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanEatForNutritionNow(food.def, pawn);
		}

		// Token: 0x0600B362 RID: 45922 RVA: 0x0033FCFC File Offset: 0x0033DEFC
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

		// Token: 0x0600B363 RID: 45923 RVA: 0x0033FD70 File Offset: 0x0033DF70
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
