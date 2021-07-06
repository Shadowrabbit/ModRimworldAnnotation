using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000172 RID: 370
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x0000D528 File Offset: 0x0000B728
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0000D544 File Offset: 0x0000B744
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(ing.GetBaseCount()) + " (" + ing.filter.Summary + ")";
		}
	}
}
