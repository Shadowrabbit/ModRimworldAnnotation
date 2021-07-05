using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001190 RID: 4496
	public class CompProperties_SendSignalOnPawnProximity : CompProperties
	{
		// Token: 0x06006C27 RID: 27687 RVA: 0x002441D4 File Offset: 0x002423D4
		public CompProperties_SendSignalOnPawnProximity()
		{
			this.compClass = typeof(CompSendSignalOnPawnProximity);
		}

		// Token: 0x04003C1C RID: 15388
		public bool triggerOnPawnInRoom;

		// Token: 0x04003C1D RID: 15389
		public float radius;

		// Token: 0x04003C1E RID: 15390
		public int enableAfterTicks;

		// Token: 0x04003C1F RID: 15391
		public bool onlyHumanlike;

		// Token: 0x04003C20 RID: 15392
		public string signalTag;
	}
}
