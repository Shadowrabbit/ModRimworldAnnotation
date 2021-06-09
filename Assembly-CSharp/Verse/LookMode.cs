using System;

namespace Verse
{
	// Token: 0x020004B1 RID: 1201
	public enum LookMode : byte
	{
		// Token: 0x04001554 RID: 5460
		Undefined,
		// Token: 0x04001555 RID: 5461
		Value,
		// Token: 0x04001556 RID: 5462
		Deep,
		// Token: 0x04001557 RID: 5463
		Reference,
		// Token: 0x04001558 RID: 5464
		Def,
		// Token: 0x04001559 RID: 5465
		LocalTargetInfo,
		// Token: 0x0400155A RID: 5466
		TargetInfo,
		// Token: 0x0400155B RID: 5467
		GlobalTargetInfo,
		// Token: 0x0400155C RID: 5468
		BodyPart
	}
}
