using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001234 RID: 4660
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x060065CF RID: 26063 RVA: 0x001F73E4 File Offset: 0x001F55E4
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x040043C8 RID: 17352
		public float mtbDays;

		// Token: 0x040043C9 RID: 17353
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x040043CA RID: 17354
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x040043CB RID: 17355
		public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

		// Token: 0x040043CC RID: 17356
		public bool skipThreatBigIfRaidBeacon;
	}
}
