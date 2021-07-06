using System;

namespace Verse
{
	// Token: 0x020004B0 RID: 1200
	public enum LoadSaveMode : byte
	{
		// Token: 0x0400154E RID: 5454
		Inactive,
		// Token: 0x0400154F RID: 5455
		Saving,
		// Token: 0x04001550 RID: 5456
		LoadingVars,
		// Token: 0x04001551 RID: 5457
		ResolvingCrossRefs,
		// Token: 0x04001552 RID: 5458
		PostLoadInit
	}
}
