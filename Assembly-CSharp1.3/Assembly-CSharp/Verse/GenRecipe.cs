using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004BA RID: 1210
	public static class GenRecipe
	{
		// Token: 0x0600250B RID: 9483 RVA: 0x000E68E7 File Offset: 0x000E4AE7
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver, Precept_ThingStyle precept = null)
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
						Room room = worker.GetRoom(RegionType.Set_All);
						if (Rand.Chance((room != null) ? room.GetStat(RoomStatDefOf.FoodPoisonChance) : RoomStatDefOf.FoodPoisonChance.roomlessScore))
						{
							compFoodPoisonable.SetPoisoned(FoodPoisonCause.FilthyKitchen);
						}
						else if (Rand.Chance(worker.GetStatValue(StatDefOf.FoodPoisonChance, true)))
						{
							compFoodPoisonable.SetPoisoned(FoodPoisonCause.IncompetentCook);
						}
					}
					yield return GenRecipe.PostProcessProduct(thing, recipeDef, worker, precept);
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
									yield return GenRecipe.PostProcessProduct(product, recipeDef, worker, precept);
								}
								IEnumerator<Thing> enumerator = null;
							}
						}
						else
						{
							foreach (Thing product2 in thing2.ButcherProducts(worker, efficiency))
							{
								yield return GenRecipe.PostProcessProduct(product2, recipeDef, worker, precept);
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

		// Token: 0x0600250C RID: 9484 RVA: 0x000E691C File Offset: 0x000E4B1C
		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker, Precept_ThingStyle precept = null)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(recipeDef + " needs workSkill because it creates a product with a quality.");
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
			if (worker.Ideo != null)
			{
				product.StyleDef = worker.Ideo.GetStyleFor(product.def);
			}
			if (precept != null)
			{
				product.StyleSourcePrecept = precept;
			}
			if (product.def.Minifiable)
			{
				product = product.MakeMinified();
			}
			return product;
		}
	}
}
