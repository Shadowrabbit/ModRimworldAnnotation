using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B2 RID: 6066
	public class CompProperties_DestroyAfterDelay : CompProperties
	{
		// Token: 0x06008620 RID: 34336 RVA: 0x00059FF0 File Offset: 0x000581F0
		public CompProperties_DestroyAfterDelay()
		{
			this.compClass = typeof(CompDestroyAfterDelay);
		}

		// Token: 0x04005670 RID: 22128
		public int delayTicks;

		// Token: 0x04005671 RID: 22129
		public DestroyMode destroyMode;

		// Token: 0x04005672 RID: 22130
		[MustTranslate]
		public string countdownLabel;
	}
}
