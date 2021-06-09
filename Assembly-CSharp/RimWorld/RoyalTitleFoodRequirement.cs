using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D96 RID: 7574
	public struct RoyalTitleFoodRequirement
	{
		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x0600A47D RID: 42109 RVA: 0x0006D112 File Offset: 0x0006B312
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		// Token: 0x0600A47E RID: 42110 RVA: 0x002FE308 File Offset: 0x002FC508
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		// Token: 0x04006F83 RID: 28547
		public FoodPreferability minQuality;

		// Token: 0x04006F84 RID: 28548
		public FoodTypeFlags allowedTypes;

		// Token: 0x04006F85 RID: 28549
		public List<ThingDef> allowedDefs;
	}
}
