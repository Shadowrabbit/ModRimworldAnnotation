using System;

namespace Verse
{
	// Token: 0x0200020B RID: 523
	[Flags]
	public enum RegionType
	{
		// Token: 0x04000BDC RID: 3036
		None = 0,
		// Token: 0x04000BDD RID: 3037
		ImpassableFreeAirExchange = 1,
		// Token: 0x04000BDE RID: 3038
		Normal = 2,
		// Token: 0x04000BDF RID: 3039
		Portal = 4,
		// Token: 0x04000BE0 RID: 3040
		Fence = 8,
		// Token: 0x04000BE1 RID: 3041
		Set_Passable = 14,
		// Token: 0x04000BE2 RID: 3042
		Set_Impassable = 1,
		// Token: 0x04000BE3 RID: 3043
		Set_All = 15
	}
}
