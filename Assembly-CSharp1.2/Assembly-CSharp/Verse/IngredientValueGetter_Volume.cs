using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200016E RID: 366
	public class IngredientValueGetter_Volume : IngredientValueGetter
	{
		// Token: 0x06000948 RID: 2376 RVA: 0x0000D4BE File Offset: 0x0000B6BE
		public override float ValuePerUnitOf(ThingDef t)
		{
			if (t.IsStuff)
			{
				return t.VolumePerUnit;
			}
			return 1f;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00098D3C File Offset: 0x00096F3C
		public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
		{
			if (!ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume) || ing.filter.AllowedThingDefs.Any((ThingDef td) => td.smallVolume && !r.GetPremultipliedSmallIngredients().Contains(td)))
			{
				return "BillRequires".Translate(ing.GetBaseCount(), ing.filter.Summary);
			}
			return "BillRequires".Translate(ing.GetBaseCount() * 10f, ing.filter.Summary);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00098E00 File Offset: 0x00097000
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
