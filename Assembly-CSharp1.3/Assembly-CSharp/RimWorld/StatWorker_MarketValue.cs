using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F4 RID: 5364
	public class StatWorker_MarketValue : StatWorker
	{
		// Token: 0x06007FDD RID: 32733 RVA: 0x002D414C File Offset: 0x002D234C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing && req.Thing is Pawn)
			{
				return base.GetValueUnfinalized(StatRequest.For(req.BuildableDef, req.StuffDef, QualityCategory.Normal), applyPostProcess) * PriceUtility.PawnQualityPriceFactor((Pawn)req.Thing, null) + PriceUtility.PawnQualityPriceOffset((Pawn)req.Thing, null);
			}
			float result;
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				result = base.GetValueUnfinalized(req, true);
			}
			else if (req.HasThing && req.Thing.StyleSourcePrecept is Precept_Relic)
			{
				result = StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, ThingDefOf.Steel);
			}
			else
			{
				result = StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, req.StuffDef);
			}
			return result;
		}

		// Token: 0x06007FDE RID: 32734 RVA: 0x002D4218 File Offset: 0x002D2418
		public static float CalculatedBaseMarketValue(BuildableDef def, ThingDef stuffDef)
		{
			float num = 0f;
			RecipeDef recipeDef = StatWorker_MarketValue.CalculableRecipe(def);
			float num2;
			int num3;
			if (recipeDef != null)
			{
				num2 = recipeDef.workAmount;
				num3 = recipeDef.products[0].count;
				if (recipeDef.ingredients != null)
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						IngredientCount ingredientCount = recipeDef.ingredients[i];
						int num4 = ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, recipeDef);
						num += (float)num4 * ingredientCount.FixedIngredient.BaseMarketValue;
					}
				}
			}
			else
			{
				num2 = Mathf.Max(def.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef), def.GetStatValueAbstract(StatDefOf.WorkToBuild, stuffDef));
				num3 = 1;
				if (def.CostList != null)
				{
					for (int j = 0; j < def.CostList.Count; j++)
					{
						ThingDefCountClass thingDefCountClass = def.CostList[j];
						num += (float)thingDefCountClass.count * thingDefCountClass.thingDef.BaseMarketValue;
					}
				}
				if (def.CostStuffCount > 0)
				{
					if (stuffDef != null)
					{
						num += (float)def.CostStuffCount / stuffDef.VolumePerUnit * stuffDef.GetStatValueAbstract(StatDefOf.MarketValue, null);
					}
					else
					{
						num += (float)def.CostStuffCount * 2f;
					}
				}
			}
			if (num2 > 2f)
			{
				num += num2 * 0.0036f;
			}
			return num / (float)num3;
		}

		// Token: 0x06007FDF RID: 32735 RVA: 0x002D436C File Offset: 0x002D256C
		public static RecipeDef CalculableRecipe(BuildableDef def)
		{
			if (def.CostList.NullOrEmpty<ThingDefCountClass>() && def.CostStuffCount <= 0)
			{
				List<RecipeDef> allDefsListForReading = DefDatabase<RecipeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					RecipeDef recipeDef = allDefsListForReading[i];
					if (recipeDef.products != null && recipeDef.products.Count == 1 && recipeDef.products[0].thingDef == def)
					{
						for (int j = 0; j < recipeDef.ingredients.Count; j++)
						{
							if (!recipeDef.ingredients[j].IsFixedIngredient)
							{
								return null;
							}
						}
						return recipeDef;
					}
				}
			}
			return null;
		}

		// Token: 0x06007FE0 RID: 32736 RVA: 0x002D4408 File Offset: 0x002D2608
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			if (req.HasThing && req.Thing is Pawn)
			{
				Pawn pawn = (Pawn)req.Thing;
				StringBuilder stringBuilder = new StringBuilder(base.GetExplanationUnfinalized(req, numberSense));
				PriceUtility.PawnQualityPriceFactor(pawn, stringBuilder);
				PriceUtility.PawnQualityPriceOffset(pawn, stringBuilder);
				return stringBuilder.ToString();
			}
			if (req.StatBases.StatListContains(StatDefOf.MarketValue))
			{
				return base.GetExplanationUnfinalized(req, numberSense);
			}
			ThingDef stuffDef = req.StuffDef;
			return "StatsReport_MarketValueFromStuffsAndWork".TranslateSimple().TrimEnd(new char[]
			{
				'.'
			}) + ": " + StatWorker_MarketValue.CalculatedBaseMarketValue(req.BuildableDef, (!req.HasThing || !(req.Thing.StyleSourcePrecept is Precept_Relic)) ? req.StuffDef : ThingDefOf.Steel).ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense);
		}

		// Token: 0x04004FAC RID: 20396
		public const float ValuePerWork = 0.0036f;

		// Token: 0x04004FAD RID: 20397
		private const float DefaultGuessStuffCost = 2f;
	}
}
