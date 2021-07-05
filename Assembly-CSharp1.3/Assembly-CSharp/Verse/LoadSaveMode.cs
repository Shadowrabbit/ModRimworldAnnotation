using System;

namespace Verse
{
	// Token: 0x02000324 RID: 804
	public enum LoadSaveMode : byte
	{
		// Token: 0x04000FF2 RID: 4082
		Inactive,
		// Token: 0x04000FF3 RID: 4083
		Saving,
		// Token: 0x04000FF4 RID: 4084
		LoadingVars,
		// Token: 0x04000FF5 RID: 4085
		ResolvingCrossRefs,
		// Token: 0x04000FF6 RID: 4086
		PostLoadInit
	}
}
