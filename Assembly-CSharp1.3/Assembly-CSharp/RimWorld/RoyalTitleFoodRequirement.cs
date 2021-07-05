using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001522 RID: 5410
	public struct RoyalTitleFoodRequirement
	{
		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x060080B0 RID: 32944 RVA: 0x002D9811 File Offset: 0x002D7A11
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		// Token: 0x060080B1 RID: 32945 RVA: 0x002D981C File Offset: 0x002D7A1C
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		// Token: 0x04005027 RID: 20519
		public FoodPreferability minQuality;

		// Token: 0x04005028 RID: 20520
		public FoodTypeFlags allowedTypes;

		// Token: 0x04005029 RID: 20521
		public List<ThingDef> allowedDefs;
	}
}
