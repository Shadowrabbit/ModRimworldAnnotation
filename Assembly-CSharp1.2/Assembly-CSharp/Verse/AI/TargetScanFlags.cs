using System;

namespace Verse.AI
{
	// Token: 0x02000AAE RID: 2734
	[Flags]
	public enum TargetScanFlags
	{
		// Token: 0x04002C74 RID: 11380
		None = 0,
		// Token: 0x04002C75 RID: 11381
		NeedLOSToPawns = 1,
		// Token: 0x04002C76 RID: 11382
		NeedLOSToNonPawns = 2,
		// Token: 0x04002C77 RID: 11383
		NeedLOSToAll = 3,
		// Token: 0x04002C78 RID: 11384
		NeedReachable = 4,
		// Token: 0x04002C79 RID: 11385
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x04002C7A RID: 11386
		NeedNonBurning = 16,
		// Token: 0x04002C7B RID: 11387
		NeedThreat = 32,
		// Token: 0x04002C7C RID: 11388
		NeedActiveThreat = 64,
		// Token: 0x04002C7D RID: 11389
		LOSBlockableByGas = 128,
		// Token: 0x04002C7E RID: 11390
		NeedAutoTargetable = 256,
		// Token: 0x04002C7F RID: 11391
		NeedNotUnderThickRoof = 512
	}
}
