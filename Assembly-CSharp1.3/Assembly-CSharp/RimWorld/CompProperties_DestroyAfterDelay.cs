using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001120 RID: 4384
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		// Token: 0x0600694E RID: 26958 RVA: 0x00237EED File Offset: 0x002360ED
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		// Token: 0x04003AEB RID: 15083
		public int delayTicks;

		// Token: 0x04003AEC RID: 15084
		public DestroyMode destroyMode;

		// Token: 0x04003AED RID: 15085
		[MustTranslate]
		public string countdownLabel;
	}
}
