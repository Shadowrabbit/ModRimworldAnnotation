using System;

namespace RimWorld
{
	// Token: 0x02000C37 RID: 3127
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x0600497B RID: 18811 RVA: 0x00185205 File Offset: 0x00183405
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x04002CA7 RID: 11431
		public IncidentCategoryDef category;

		// Token: 0x04002CA8 RID: 11432
		public bool applyCaravanVisibility;
	}
}
