using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001833 RID: 6195
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		// Token: 0x06008957 RID: 35159 RVA: 0x0005C382 File Offset: 0x0005A582
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		// Token: 0x04005811 RID: 22545
		public SimpleCurve countdownCurveTicks;

		// Token: 0x04005812 RID: 22546
		public string signalTag;
	}
}
