using System;

namespace Verse.AI
{
	// Token: 0x02000969 RID: 2409
	public enum JobCondition : byte
	{
		// Token: 0x0400290B RID: 10507
		None,
		// Token: 0x0400290C RID: 10508
		Ongoing,
		// Token: 0x0400290D RID: 10509
		Succeeded,
		// Token: 0x0400290E RID: 10510
		Incompletable,
		// Token: 0x0400290F RID: 10511
		InterruptOptional,
		// Token: 0x04002910 RID: 10512
		InterruptForced,
		// Token: 0x04002911 RID: 10513
		QueuedNoLongerValid,
		// Token: 0x04002912 RID: 10514
		Errored,
		// Token: 0x04002913 RID: 10515
		ErroredPather
	}
}
