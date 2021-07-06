using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF5 RID: 4085
	public struct ThingSetMakerParams
	{
		// Token: 0x04003B89 RID: 15241
		public TechLevel? techLevel;

		// Token: 0x04003B8A RID: 15242
		public IntRange? countRange;

		// Token: 0x04003B8B RID: 15243
		public ThingFilter filter;

		// Token: 0x04003B8C RID: 15244
		public Predicate<ThingDef> validator;

		// Token: 0x04003B8D RID: 15245
		public QualityGenerator? qualityGenerator;

		// Token: 0x04003B8E RID: 15246
		public float? maxTotalMass;

		// Token: 0x04003B8F RID: 15247
		public float? maxThingMarketValue;

		// Token: 0x04003B90 RID: 15248
		public bool? allowNonStackableDuplicates;

		// Token: 0x04003B91 RID: 15249
		public float? minSingleItemMarketValuePct;

		// Token: 0x04003B92 RID: 15250
		public FloatRange? totalMarketValueRange;

		// Token: 0x04003B93 RID: 15251
		public FloatRange? totalNutritionRange;

		// Token: 0x04003B94 RID: 15252
		public PodContentsType? podContentsType;

		// Token: 0x04003B95 RID: 15253
		public Faction makingFaction;

		// Token: 0x04003B96 RID: 15254
		public TraderKindDef traderDef;

		// Token: 0x04003B97 RID: 15255
		public int? tile;

		// Token: 0x04003B98 RID: 15256
		public Dictionary<string, object> custom;
	}
}
