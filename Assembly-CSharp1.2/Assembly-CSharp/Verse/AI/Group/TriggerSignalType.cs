using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B16 RID: 2838
	public enum TriggerSignalType : byte
	{
		// Token: 0x04002D72 RID: 11634
		Undefined,
		// Token: 0x04002D73 RID: 11635
		Tick,
		// Token: 0x04002D74 RID: 11636
		Memo,
		// Token: 0x04002D75 RID: 11637
		PawnDamaged,
		// Token: 0x04002D76 RID: 11638
		PawnArrestAttempted,
		// Token: 0x04002D77 RID: 11639
		PawnLost,
		// Token: 0x04002D78 RID: 11640
		BuildingDamaged,
		// Token: 0x04002D79 RID: 11641
		BuildingLost,
		// Token: 0x04002D7A RID: 11642
		FactionRelationsChanged,
		// Token: 0x04002D7B RID: 11643
		DormancyWakeup,
		// Token: 0x04002D7C RID: 11644
		Clamor,
		// Token: 0x04002D7D RID: 11645
		MechClusterDefeated,
		// Token: 0x04002D7E RID: 11646
		Signal
	}
}
