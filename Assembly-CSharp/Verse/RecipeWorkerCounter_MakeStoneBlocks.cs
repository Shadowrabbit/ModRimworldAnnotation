using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200011C RID: 284
	public class RecipeWorkerCounter_MakeStoneBlocks : RecipeWorkerCounter
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00093D20 File Offset: 0x00091F20
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

		// Token: 0x060007CA RID: 1994 RVA: 0x0000C35F File Offset: 0x0000A55F
		public override string ProductsDescription(Bill_Production bill)
		{
			return ThingCategoryDefOf.StoneBlocks.label;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00093D68 File Offset: 0x00091F68
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
