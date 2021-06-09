using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F1 RID: 6385
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x06008D6A RID: 36202 RVA: 0x0005EC18 File Offset: 0x0005CE18
		public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
		{
			List<ThingDef> generatedDefs = new List<ThingDef>();
			int numThingDefsToUse = this.thingDefCountRange.RandomInRange;
			Func<ThingDef, bool> <>9__0;
			int num;
			for (int i = 0; i < numThingDefsToUse; i = num + 1)
			{
				IEnumerable<ThingDef> descendantThingDefs = this.categoryDef.DescendantThingDefs;
				Func<ThingDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef t) => t.tradeability.TraderCanSell() && t.techLevel <= this.maxTechLevelGenerate && !generatedDefs.Contains(t) && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)))));
				}
				ThingDef chosenThingDef;
				if (!descendantThingDefs.Where(predicate).TryRandomElement(out chosenThingDef))
				{
					break;
				}
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef)))
				{
					yield return thing;
				}
				IEnumerator<Thing> enumerator = null;
				generatedDefs.Add(chosenThingDef);
				chosenThingDef = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x0028F2CC File Offset: 0x0028D4CC
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		// Token: 0x04005A45 RID: 23109
		private ThingCategoryDef categoryDef;

		// Token: 0x04005A46 RID: 23110
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04005A47 RID: 23111
		private List<ThingDef> excludedThingDefs;

		// Token: 0x04005A48 RID: 23112
		private List<ThingCategoryDef> excludedCategories;
	}
}
