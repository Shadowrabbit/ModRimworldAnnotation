using System;

namespace RimWorld
{
	// Token: 0x02001217 RID: 4631
	public class StorytellerCompProperties_CategoryIndividualMTBByBiome : StorytellerCompProperties
	{
		// Token: 0x06006556 RID: 25942 RVA: 0x00045552 File Offset: 0x00043752
		public StorytellerCompProperties_CategoryIndividualMTBByBiome()
		{
			this.compClass = typeof(StorytellerComp_CategoryIndividualMTBByBiome);
		}

		// Token: 0x04004370 RID: 17264
		public IncidentCategoryDef category;

		// Token: 0x04004371 RID: 17265
		public bool applyCaravanVisibility;
	}
}
