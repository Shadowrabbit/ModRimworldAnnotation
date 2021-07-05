using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B3 RID: 179
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001CC74 File Offset: 0x0001AE74
		public override int CountProducts(Bill_Production bill)
		{
			int num = 0;
			List<ThingDef> childThingDefs = ThingCategoryDefOf.StoneBlocks.childThingDefs;
			for (int i = 0; i < childThingDefs.Count; i++)
			{
				num += bill.Map.resourceCounter.GetCount(childThingDefs[i]);
			}
			return num;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001CCBA File Offset: 0x0001AEBA
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		public override bool CanPossiblyStoreInStockpile(Bill_Production bill, Zone_Stockpile stockpile)
		{
			foreach (ThingDef thingDef in bill.ingredientFilter.AllowedThingDefs)
			{
				if (!thingDef.butcherProducts.NullOrEmpty<ThingDefCountClass>())
				{
					ThingDef thingDef2 = thingDef.butcherProducts[0].thingDef;
					if (!stockpile.GetStoreSettings().AllowedToAccept(thingDef2))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
