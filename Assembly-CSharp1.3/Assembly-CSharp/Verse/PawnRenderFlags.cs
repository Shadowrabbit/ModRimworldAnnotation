using System;

namespace Verse
{
	// Token: 0x02000265 RID: 613
	[Flags]
	public enum PawnRenderFlags : uint
	{
		// Token: 0x04000D4D RID: 3405
		None = 0U,
		// Token: 0x04000D4E RID: 3406
		Portrait = 1U,
		// Token: 0x04000D4F RID: 3407
		HeadStump = 2U,
		// Token: 0x04000D50 RID: 3408
		Invisible = 4U,
		// Token: 0x04000D51 RID: 3409
		DrawNow = 8U,
		// Token: 0x04000D52 RID: 3410
		Cache = 16U,
		// Token: 0x04000D53 RID: 3411
		Headgear = 32U,
		// Token: 0x04000D54 RID: 3412
		Clothes = 64U,
		// Token: 0x04000D55 RID: 3413
		NeverAimWeapon = 128U,
		// Token: 0x04000D56 RID: 3414
		StylingStation = 256U
	}
}
