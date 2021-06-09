using System;

namespace Verse
{
	// Token: 0x020000B4 RID: 180
	public enum TraverseMode : byte
	{
		// Token: 0x040002C3 RID: 707
		ByPawn,
		// Token: 0x040002C4 RID: 708
		PassDoors,
		// Token: 0x040002C5 RID: 709
		NoPassClosedDoors,
		// Token: 0x040002C6 RID: 710
		PassAllDestroyableThings,
		// Token: 0x040002C7 RID: 711
		NoPassClosedDoorsOrWater,
		// Token: 0x040002C8 RID: 712
		PassAllDestroyableThingsNotWater
	}
}
