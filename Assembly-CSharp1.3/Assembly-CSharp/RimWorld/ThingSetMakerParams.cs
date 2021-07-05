using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AD8 RID: 2776
	public struct ThingSetMakerParams
	{
		// Token: 0x04002749 RID: 10057
		public TechLevel? techLevel;

		// Token: 0x0400274A RID: 10058
		public IntRange? countRange;

		// Token: 0x0400274B RID: 10059
		public ThingFilter filter;

		// Token: 0x0400274C RID: 10060
		public Predicate<ThingDef> validator;

		// Token: 0x0400274D RID: 10061
		public QualityGenerator? qualityGenerator;

		// Token: 0x0400274E RID: 10062
		public float? maxTotalMass;

		// Token: 0x0400274F RID: 10063
		public float? maxThingMarketValue;

		// Token: 0x04002750 RID: 10064
		public bool? allowNonStackableDuplicates;

		// Token: 0x04002751 RID: 10065
		public float? minSingleItemMarketValuePct;

		// Token: 0x04002752 RID: 10066
		public FloatRange? totalMarketValueRange;

		// Token: 0x04002753 RID: 10067
		public FloatRange? totalNutritionRange;

		// Token: 0x04002754 RID: 10068
		public PodContentsType? podContentsType;

		// Token: 0x04002755 RID: 10069
		public Faction makingFaction;

		// Token: 0x04002756 RID: 10070
		public TraderKindDef traderDef;

		// Token: 0x04002757 RID: 10071
		public int? tile;

		// Token: 0x04002758 RID: 10072
		public Dictionary<string, object> custom;
	}
}
