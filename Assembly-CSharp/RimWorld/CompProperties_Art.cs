using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFF RID: 3839
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x06005506 RID: 21766 RVA: 0x0003AF1D File Offset: 0x0003911D
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x04003606 RID: 13830
		public RulePackDef nameMaker;

		// Token: 0x04003607 RID: 13831
		public RulePackDef descriptionMaker;

		// Token: 0x04003608 RID: 13832
		public QualityCategory minQualityForArtistic;

		// Token: 0x04003609 RID: 13833
		public bool mustBeFullGrave;

		// Token: 0x0400360A RID: 13834
		public bool canBeEnjoyedAsArt;
	}
}
