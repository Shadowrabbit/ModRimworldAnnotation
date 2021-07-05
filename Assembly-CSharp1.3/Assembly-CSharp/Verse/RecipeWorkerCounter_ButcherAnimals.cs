using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B4 RID: 180
	public class RecipeWorkerCounter_ButcherAnimals : RecipeWorkerCounter
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001CD50 File Offset: 0x0001AF50
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

		// Token: 0x06000599 RID: 1433 RVA: 0x0001CD96 File Offset: 0x0001AF96
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.MeatRaw.label;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
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
