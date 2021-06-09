using System;

namespace Verse
{
	// Token: 0x020006C0 RID: 1728
	[Flags]
	public enum AllowedGameStates
	{
		// Token: 0x04001E29 RID: 7721
		Invalid = 0,
		// Token: 0x04001E2A RID: 7722
		Entry = 1,
		// Token: 0x04001E2B RID: 7723
		Playing = 2,
		// Token: 0x04001E2C RID: 7724
		WorldRenderedNow = 4,
		// Token: 0x04001E2D RID: 7725
		IsCurrentlyOnMap = 8,
		// Token: 0x04001E2E RID: 7726
		HasGameCondition = 16,
		// Token: 0x04001E2F RID: 7727
		PlayingOnMap = 10,
		// Token: 0x04001E30 RID: 7728
		PlayingOnWorld = 6
	}
}
