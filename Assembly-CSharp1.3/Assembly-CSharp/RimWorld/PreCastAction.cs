using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1D RID: 3357
	public struct PreCastAction
	{
		// Token: 0x04002F5E RID: 12126
		public Action<LocalTargetInfo, LocalTargetInfo> action;

		// Token: 0x04002F5F RID: 12127
		public int ticksAwayFromCast;
	}
}
