using System;

namespace RimWorld
{
	// Token: 0x0200104A RID: 4170
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x040037F0 RID: 14320
		None = 0,
		// Token: 0x040037F1 RID: 14321
		NeedsPower = 1,
		// Token: 0x040037F2 RID: 14322
		PowerOff = 2,
		// Token: 0x040037F3 RID: 14323
		BurningWick = 4,
		// Token: 0x040037F4 RID: 14324
		Forbidden = 8,
		// Token: 0x040037F5 RID: 14325
		ForbiddenBig = 16,
		// Token: 0x040037F6 RID: 14326
		QuestionMark = 32,
		// Token: 0x040037F7 RID: 14327
		BrokenDown = 64,
		// Token: 0x040037F8 RID: 14328
		OutOfFuel = 128,
		// Token: 0x040037F9 RID: 14329
		ForbiddenRefuel = 256
	}
}
