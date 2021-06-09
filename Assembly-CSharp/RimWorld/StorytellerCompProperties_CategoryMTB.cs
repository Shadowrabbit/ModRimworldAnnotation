using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121A RID: 4634
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x06006564 RID: 25956 RVA: 0x000455D5 File Offset: 0x000437D5
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}

		// Token: 0x04004378 RID: 17272
		public float mtbDays = -1f;

		// Token: 0x04004379 RID: 17273
		public SimpleCurve mtbDaysFactorByDaysPassedCurve;

		// Token: 0x0400437A RID: 17274
		public IncidentCategoryDef category;
	}
}
