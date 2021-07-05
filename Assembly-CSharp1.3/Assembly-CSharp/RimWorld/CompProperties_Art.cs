using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F4 RID: 2548
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x06003EC8 RID: 16072 RVA: 0x00157429 File Offset: 0x00155629
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x04002193 RID: 8595
		public RulePackDef nameMaker;

		// Token: 0x04002194 RID: 8596
		public RulePackDef descriptionMaker;

		// Token: 0x04002195 RID: 8597
		public QualityCategory minQualityForArtistic;

		// Token: 0x04002196 RID: 8598
		public bool mustBeFullGrave;

		// Token: 0x04002197 RID: 8599
		public bool canBeEnjoyedAsArt;
	}
}
