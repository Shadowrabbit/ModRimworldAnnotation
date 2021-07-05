using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200011D RID: 285
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00093DE8 File Offset: 0x00091FE8
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.MeatRaw.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0000C373 File Offset: 0x0000A573
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00093E30 File Offset: 0x00092030
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (thingDef.ingestible != null && thingDef.ingestible.sourceDef != null)
				{
					RaceProperties race = thingDef.ingestible.sourceDef.race;
					if (race != null && race.meatDef != null && !stockpile.GetStoreSettings().AllowedToAccept(race.meatDef))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
