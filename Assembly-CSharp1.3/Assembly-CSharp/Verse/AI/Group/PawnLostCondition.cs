using System;

namespace Verse.AI.Group
{
	// Token: 0x0200065F RID: 1631
	public enum PawnLostCondition : byte
	{
		// Token: 0x04001C5F RID: 7263
		Undefined,
		// Token: 0x04001C60 RID: 7264
		Vanished,
		// Token: 0x04001C61 RID: 7265
		IncappedOrKilled,
		// Token: 0x04001C62 RID: 7266
		MadePrisoner,
		// Token: 0x04001C63 RID: 7267
		ChangedFaction,
		// Token: 0x04001C64 RID: 7268
		ExitedMap,
		// Token: 0x04001C65 RID: 7269
		LeftVoluntarily,
		// Token: 0x04001C66 RID: 7270
		Drafted,
		// Token: 0x04001C67 RID: 7271
		ForcedToJoinOtherLord,
		// Token: 0x04001C68 RID: 7272
		ForcedByPlayerAction,
		// Token: 0x04001C69 RID: 7273
		ForcedByQuest,
		// Token: 0x04001C6A RID: 7274
		NoLongerEnteringTransportPods,
		// Token: 0x04001C6B RID: 7275
		MadeSlave,
		// Token: 0x04001C6C RID: 7276
		InMentalState
	}
}
