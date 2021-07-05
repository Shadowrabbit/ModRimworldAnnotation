using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001218 RID: 4632
	public class StockGenerator_Category : StockGenerator
	{
		// Token: 0x06006F3D RID: 28477 RVA: 0x002524A1 File Offset: 0x002506A1
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
				foreach (Thing thing in StockGeneratorUtility.TryMakeForStock(chosenThingDef, base.RandomCountOf(chosenThingDef), faction))
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

		// Token: 0x06006F3E RID: 28478 RVA: 0x002524B8 File Offset: 0x002506B8
		public override bool HandlesThingDef(ThingDef t)
		{
			return this.categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != Tradeability.None && t.techLevel <= this.maxTechLevelBuy && (this.excludedThingDefs == null || !this.excludedThingDefs.Contains(t)) && (this.excludedCategories == null || !this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t)));
		}

		// Token: 0x04003D7C RID: 15740
		private ThingCategoryDef categoryDef;

		// Token: 0x04003D7D RID: 15741
		private IntRange thingDefCountRange = IntRange.one;

		// Token: 0x04003D7E RID: 15742
		private List<ThingDef> excludedThingDefs;

		// Token: 0x04003D7F RID: 15743
		private List<ThingCategoryDef> excludedCategories;
	}
}
