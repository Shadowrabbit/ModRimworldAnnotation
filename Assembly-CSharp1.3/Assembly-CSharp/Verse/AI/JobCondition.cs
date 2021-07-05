using System;

namespace Verse.AI
{
	// Token: 0x0200058D RID: 1421
	public enum JobCondition : byte
	{
		// Token: 0x04001A06 RID: 6662
		None,
		// Token: 0x04001A07 RID: 6663
		Ongoing,
		// Token: 0x04001A08 RID: 6664
		Succeeded,
		// Token: 0x04001A09 RID: 6665
		Incompletable,
		// Token: 0x04001A0A RID: 6666
		InterruptOptional,
		// Token: 0x04001A0B RID: 6667
		InterruptForced,
		// Token: 0x04001A0C RID: 6668
		QueuedNoLongerValid,
		// Token: 0x04001A0D RID: 6669
		Errored,
		// Token: 0x04001A0E RID: 6670
		ErroredPather
	}
}
