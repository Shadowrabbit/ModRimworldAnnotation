using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AC3 RID: 2755
	public enum PawnLostCondition : byte
	{
		// Token: 0x04002CE7 RID: 11495
		Undefined,
		// Token: 0x04002CE8 RID: 11496
		Vanished,
		// Token: 0x04002CE9 RID: 11497
		IncappedOrKilled,
		// Token: 0x04002CEA RID: 11498
		MadePrisoner,
		// Token: 0x04002CEB RID: 11499
		ChangedFaction,
		// Token: 0x04002CEC RID: 11500
		ExitedMap,
		// Token: 0x04002CED RID: 11501
		LeftVoluntarily,
		// Token: 0x04002CEE RID: 11502
		Drafted,
		// Token: 0x04002CEF RID: 11503
		ForcedToJoinOtherLord,
		// Token: 0x04002CF0 RID: 11504
		ForcedByPlayerAction,
		// Token: 0x04002CF1 RID: 11505
		ForcedByQuest,
		// Token: 0x04002CF2 RID: 11506
		NoLongerEnteringTransportPods
	}
}
