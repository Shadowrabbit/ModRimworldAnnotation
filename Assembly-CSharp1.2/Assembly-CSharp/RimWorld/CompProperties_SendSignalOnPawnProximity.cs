using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001836 RID: 6198
	public class CompProperties_SendSignalOnPawnProximity : CompProperties
	{
		// Token: 0x0600896B RID: 35179 RVA: 0x0005C470 File Offset: 0x0005A670
		public CompProperties_SendSignalOnPawnProximity()
		{
			this.compClass = typeof(CompSendSignalOnPawnProximity);
		}

		// Token: 0x0400581A RID: 22554
		public bool triggerOnPawnInRoom;

		// Token: 0x0400581B RID: 22555
		public float radius;

		// Token: 0x0400581C RID: 22556
		public int enableAfterTicks;

		// Token: 0x0400581D RID: 22557
		public bool onlyHumanlike;

		// Token: 0x0400581E RID: 22558
		public string signalTag;
	}
}
