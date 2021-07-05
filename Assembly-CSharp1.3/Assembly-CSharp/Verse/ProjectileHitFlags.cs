using System;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	[Flags]
	public enum ProjectileHitFlags
	{
		// Token: 0x040010D2 RID: 4306
		None = 0,
		// Token: 0x040010D3 RID: 4307
		IntendedTarget = 1,
		// Token: 0x040010D4 RID: 4308
		NonTargetPawns = 2,
		// Token: 0x040010D5 RID: 4309
		NonTargetWorld = 4,
		// Token: 0x040010D6 RID: 4310
		All = -1
	}
}
