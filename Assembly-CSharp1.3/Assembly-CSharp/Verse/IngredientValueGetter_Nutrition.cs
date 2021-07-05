using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F6 RID: 246
	public class IngredientValueGetter_Nutrition : IngredientValueGetter
	{
		// Token: 0x0600069F RID: 1695 RVA: 0x00020539 File Offset: 0x0001E739
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (!t.IsNutritionGivingIngestible)
			{
				return 0f;
			}
			return t.GetStatValueAbstract(StatDefOf.Nutrition, null);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00020555 File Offset: 0x0001E755
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			return "BillRequiresNutrition".Translate(ing.GetBaseCount()) + " (" + ing.filter.Summary + ")";
		}
	}
}
