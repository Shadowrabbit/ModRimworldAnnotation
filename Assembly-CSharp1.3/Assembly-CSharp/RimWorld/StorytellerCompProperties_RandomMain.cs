using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C48 RID: 3144
	public class StorytellerCompProperties_RandomMain : StorytellerCompProperties
	{
		// Token: 0x060049A9 RID: 18857 RVA: 0x00185790 File Offset: 0x00183990
		public StorytellerCompProperties_RandomMain()
		{
			this.compClass = typeof(StorytellerComp_RandomMain);
		}

		// Token: 0x04002CC2 RID: 11458
		public float mtbDays;

		// Token: 0x04002CC3 RID: 11459
		public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

		// Token: 0x04002CC4 RID: 11460
		public float maxThreatBigIntervalDays = 99999f;

		// Token: 0x04002CC5 RID: 11461
		public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

		// Token: 0x04002CC6 RID: 11462
		public bool skipThreatBigIfRaidBeacon;
	}
}
