using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1A RID: 7450
	public class SpecialThingFilterWorker_NonBurnableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0600A209 RID: 41481 RVA: 0x0006BB16 File Offset: 0x00069D16
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x0600A20A RID: 41482 RVA: 0x002F4B48 File Offset: 0x002F2D48
		public override bool CanEverMatch(ThingDef def)
		{
			if (!def.IsWeapon)
			{
				return false;
			}
			if (!def.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				for (int i = 0; i < def.thingCategories.Count; i++)
				{
					for (ThingCategoryDef thingCategoryDef = def.thingCategories[i]; thingCategoryDef != null; thingCategoryDef = thingCategoryDef.parent)
					{
						if (thingCategoryDef == ThingCategoryDefOf.Weapons)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600A20B RID: 41483 RVA: 0x0006BB58 File Offset: 0x00069D58
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
