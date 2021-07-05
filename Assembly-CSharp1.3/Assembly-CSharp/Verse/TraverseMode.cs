using System;

namespace Verse
{
	// Token: 0x02000067 RID: 103
	public enum TraverseMode : byte
	{
		// Token: 0x04000148 RID: 328
		ByPawn,
		// Token: 0x04000149 RID: 329
		PassDoors,
		// Token: 0x0400014A RID: 330
		NoPassClosedDoors,
		// Token: 0x0400014B RID: 331
		PassAllDestroyableThings,
		// Token: 0x0400014C RID: 332
		NoPassClosedDoorsOrWater,
		// Token: 0x0400014D RID: 333
		PassAllDestroyableThingsNotWater
	}
}
