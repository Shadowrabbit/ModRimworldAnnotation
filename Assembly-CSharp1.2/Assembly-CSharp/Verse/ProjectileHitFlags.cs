using System;

namespace Verse
{
	// Token: 0x020004FF RID: 1279
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x04001653 RID: 5715
		None = 0,
		// Token: 0x04001654 RID: 5716
		IntendedTarget = 1,
		// Token: 0x04001655 RID: 5717
		NonTargetPawns = 2,
		// Token: 0x04001656 RID: 5718
		NonTargetWorld = 4,
		// Token: 0x04001657 RID: 5719
		All = -1
	}
}
