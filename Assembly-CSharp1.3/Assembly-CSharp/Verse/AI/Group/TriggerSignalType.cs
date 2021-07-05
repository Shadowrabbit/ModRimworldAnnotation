using System;

namespace Verse.AI.Group
{
	// Token: 0x0200068A RID: 1674
	public enum TriggerSignalType : byte
	{
		// Token: 0x04001CBE RID: 7358
		Undefined,
		// Token: 0x04001CBF RID: 7359
		Tick,
		// Token: 0x04001CC0 RID: 7360
		Memo,
		// Token: 0x04001CC1 RID: 7361
		PawnDamaged,
		// Token: 0x04001CC2 RID: 7362
		PawnArrestAttempted,
		// Token: 0x04001CC3 RID: 7363
		PawnLost,
		// Token: 0x04001CC4 RID: 7364
		BuildingDamaged,
		// Token: 0x04001CC5 RID: 7365
		BuildingLost,
		// Token: 0x04001CC6 RID: 7366
		FactionRelationsChanged,
		// Token: 0x04001CC7 RID: 7367
		DormancyWakeup,
		// Token: 0x04001CC8 RID: 7368
		Clamor,
		// Token: 0x04001CC9 RID: 7369
		MechClusterDefeated,
		// Token: 0x04001CCA RID: 7370
		Signal
	}
}
