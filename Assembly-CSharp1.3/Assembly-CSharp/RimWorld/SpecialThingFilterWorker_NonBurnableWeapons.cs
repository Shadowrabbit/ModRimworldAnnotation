using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B4 RID: 5300
	public class SpecialThingFilterWorker_NonBurnableWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06007EB1 RID: 32433 RVA: 0x002CE681 File Offset: 0x002CC881
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x002CE6C4 File Offset: 0x002CC8C4
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

		// Token: 0x06007EB3 RID: 32435 RVA: 0x002CE722 File Offset: 0x002CC922
		public override bool AlwaysMatches(ThingDef def)
		{
			return this.CanEverMatch(def) && !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
