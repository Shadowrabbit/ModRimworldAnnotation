using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C39 RID: 3129
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x06004980 RID: 18816 RVA: 0x0018525E File Offset: 0x0018345E
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}

		// Token: 0x04002CA9 RID: 11433
		public float mtbDays = -1f;

		// Token: 0x04002CAA RID: 11434
		public SimpleCurve mtbDaysFactorByDaysPassedCurve;

		// Token: 0x04002CAB RID: 11435
		public IncidentCategoryDef category;
	}
}
