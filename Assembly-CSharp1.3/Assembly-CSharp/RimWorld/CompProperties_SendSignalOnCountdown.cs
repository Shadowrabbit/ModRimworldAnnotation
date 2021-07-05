using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118E RID: 4494
	public class CompProperties_SendSignalOnCountdown : CompProperties
	{
		// Token: 0x06006C1B RID: 27675 RVA: 0x00243F42 File Offset: 0x00242142
		public CompProperties_SendSignalOnCountdown()
		{
			this.compClass = typeof(CompSendSignalOnCountdown);
		}

		// Token: 0x04003C17 RID: 15383
		public SimpleCurve countdownCurveTicks;

		// Token: 0x04003C18 RID: 15384
		public string signalTag;
	}
}
