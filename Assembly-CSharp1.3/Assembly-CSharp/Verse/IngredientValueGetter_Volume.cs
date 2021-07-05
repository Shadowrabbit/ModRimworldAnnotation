using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000F5 RID: 245
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x000203FF File Offset: 0x0001E5FF
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00020418 File Offset: 0x0001E618
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return "BillRequires".Translate(ing.GetBaseCount(), ing.filter.Summary);
			}
			return "BillRequires".Translate(ing.GetBaseCount() * 10f, ing.filter.Summary);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000204DC File Offset: 0x0001E6DC
		public override string ExtraDescriptionLine(RecipeDef r)
		{
			Func<ThingDef, bool> <>9__1;
			if (r.ingredients.Any(delegate(IngredientCount ing)
			{
				IEnumerable<ThingDef> allowedThingDefs = ing.filter.AllowedThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)));
				}
				return allowedThingDefs.Any(predicate);
			}))
			{
				return "BillRequiresMayVary".Translate(10.ToStringCached());
			}
			return null;
		}
	}
}
