﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000850 RID: 2128
	public static class GenRecipe
	{
		// Token: 0x06003557 RID: 13655 RVA: 0x00029803 File Offset: 0x00027A03
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver)
		{
			float efficiency;
			if (recipeDef.efficiencyStat == null)
			{
				efficiency = 1f;
			}
			else
			{
				efficiency = worker.GetStatValue(recipeDef.efficiencyStat, true);
			}
			if (recipeDef.workTableEfficiencyStat != null)
			{
				Building_WorkTable building_WorkTable = billGiver as Building_WorkTable;
				if (building_WorkTable != null)
				{
					efficiency *= building_WorkTable.GetStatValue(recipeDef.workTableEfficiencyStat, true);
				}
			}
			if (recipeDef.products != null)
			{
				int num;
				for (int i = 0; i < recipeDef.products.Count; i = num + 1)
				{
					ThingDefCountClass thingDefCountClass = recipeDef.products[i];
					ThingDef stuff;
					if (thingDefCountClass.thingDef.MadeFromStuff)
					{
						stuff = dominantIngredient.def;
					}
					else
					{
						stuff = null;
					}
					Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, stuff);
					thing.stackCount = Mathf.CeilToInt((float)thingDefCountClass.count * efficiency);
					if (dominantIngredient != null && recipeDef.useIngredientsForColor)
					{
						thing.SetColor(dominantIngredient.DrawColor, false);
					}
					CompIngredients compIngredients = thing.TryGetComp<CompIngredients>();
					if (compIngredients != null)
					{
						for (int k = 0; k < ingredients.Count; k++)
						{
							compIngredients.RegisterIngredient(ingredients[k].def);
						}
					}
					CompFoodPoisonable compFoodPoisonable = thing.TryGetComp<CompFoodPoisonable>();
					if (compFoodPoisonable != null)
					{
						Room room = worker.GetRoom(RegionType.Set_Passable);
						if (Rand.Chance((room != null) ? room.GetStat(RoomStatDefOf.FoodPoisonChance) : RoomStatDefOf.FoodPoisonChance.roomlessScore))
						{
							compFoodPoisonable.SetPoisoned(FoodPoisonCause.FilthyKitchen);
						}
						else if (Rand.Chance(worker.GetStatValue(StatDefOf.FoodPoisonChance, true)))
						{
							compFoodPoisonable.SetPoisoned(FoodPoisonCause.IncompetentCook);
						}
					}
					yield return GenRecipe.PostProcessProduct(thing, recipeDef, worker);
					num = i;
				}
			}
			if (recipeDef.specialProducts != null)
			{
				int num;
				for (int i = 0; i < recipeDef.specialProducts.Count; i = num + 1)
				{
					for (int j = 0; j < ingredients.Count; j = num + 1)
					{
						Thing thing2 = ingredients[j];
						SpecialProductType specialProductType = recipeDef.specialProducts[i];
						if (specialProductType != SpecialProductType.Butchery)
						{
							if (specialProductType == SpecialProductType.Smelted)
							{
								foreach (Thing product in thing2.SmeltProducts(efficiency))
								{
									yield return GenRecipe.PostProcessProduct(product, recipeDef, worker);
								}
								IEnumerator<Thing> enumerator = null;
							}
						}
						else
						{
							foreach (Thing product2 in thing2.ButcherProducts(worker, efficiency))
							{
								yield return GenRecipe.PostProcessProduct(product2, recipeDef, worker);
							}
							IEnumerator<Thing> enumerator = null;
						}
						num = j;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x00157AF0 File Offset: 0x00155CF0
		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(recipeDef + " needs workSkill because it creates a product with a quality.", false);
				}
				QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, recipeDef.workSkill);
				compQuality.SetQuality(q, ArtGenerationContext.Colony);
				QualityUtility.SendCraftNotification(product, worker);
			}
			CompArt compArt = product.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.JustCreatedBy(worker);
				if (compQuality != null && compQuality.Quality >= QualityCategory.Excellent)
				{
					TaleRecorder.RecordTale(TaleDefOf.CraftedArt, new object[]
					{
						worker,
						product
					});
				}
			}
			if (product.def.Minifiable)
			{
				product = product.MakeMinified();
			}
			return product;
		}
	}
}
