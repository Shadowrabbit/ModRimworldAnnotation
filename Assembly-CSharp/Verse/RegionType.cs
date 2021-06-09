using System;

namespace Verse
{
	// Token: 0x020002E9 RID: 745
	[Flags]
	public enum RegionType
	{
		// Token: 0x04000F12 RID: 3858
		None = 0,
		// Token: 0x04000F13 RID: 3859
		ImpassableFreeAirExchange = 1,
		// Token: 0x04000F14 RID: 3860
		Normal = 2,
		// Token: 0x04000F15 RID: 3861
		Portal = 4,
		// Token: 0x04000F16 RID: 3862
		Set_Passable = 6,
		// Token: 0x04000F17 RID: 3863
		Set_Impassable = 1,
		// Token: 0x04000F18 RID: 3864
		Set_All = 7
	}
}
