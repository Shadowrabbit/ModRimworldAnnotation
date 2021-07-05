using System;

namespace Verse
{
	// Token: 0x020003BE RID: 958
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x040011B4 RID: 4532
		Invalid = 0,
		// Token: 0x040011B5 RID: 4533
		Entry = 1,
		// Token: 0x040011B6 RID: 4534
		Playing = 2,
		// Token: 0x040011B7 RID: 4535
		WorldRenderedNow = 4,
		// Token: 0x040011B8 RID: 4536
		IsCurrentlyOnMap = 8,
		// Token: 0x040011B9 RID: 4537
		HasGameCondition = 16,
		// Token: 0x040011BA RID: 4538
		PlayingOnMap = 10,
		// Token: 0x040011BB RID: 4539
		PlayingOnWorld = 6
	}
}
