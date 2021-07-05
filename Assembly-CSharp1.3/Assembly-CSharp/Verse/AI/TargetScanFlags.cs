using System;

namespace Verse.AI
{
	// Token: 0x02000651 RID: 1617
	[Flags]
	public enum TargetScanFlags
	{
		// Token: 0x04001BEF RID: 7151
		None = 0,
		// Token: 0x04001BF0 RID: 7152
		NeedLOSToPawns = 1,
		// Token: 0x04001BF1 RID: 7153
		NeedLOSToNonPawns = 2,
		// Token: 0x04001BF2 RID: 7154
		NeedLOSToAll = 3,
		// Token: 0x04001BF3 RID: 7155
		NeedReachable = 4,
		// Token: 0x04001BF4 RID: 7156
		NeedReachableIfCantHitFromMyPos = 8,
		// Token: 0x04001BF5 RID: 7157
		NeedNonBurning = 16,
		// Token: 0x04001BF6 RID: 7158
		NeedThreat = 32,
		// Token: 0x04001BF7 RID: 7159
		NeedActiveThreat = 64,
		// Token: 0x04001BF8 RID: 7160
		LOSBlockableByGas = 128,
		// Token: 0x04001BF9 RID: 7161
		NeedAutoTargetable = 256,
		// Token: 0x04001BFA RID: 7162
		NeedNotUnderThickRoof = 512
	}
}
