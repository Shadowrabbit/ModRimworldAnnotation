using System;

namespace RimWorld
{
	// Token: 0x02001676 RID: 5750
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x040051C1 RID: 20929
		NeedsPower = 1,
		// Token: 0x040051C2 RID: 20930
		PowerOff = 2,
		// Token: 0x040051C3 RID: 20931
		BurningWick = 4,
		// Token: 0x040051C4 RID: 20932
		Forbidden = 8,
		// Token: 0x040051C5 RID: 20933
		ForbiddenBig = 16,
		// Token: 0x040051C6 RID: 20934
		QuestionMark = 32,
		// Token: 0x040051C7 RID: 20935
		BrokenDown = 64,
		// Token: 0x040051C8 RID: 20936
		OutOfFuel = 128,
		// Token: 0x040051C9 RID: 20937
		ForbiddenRefuel = 256
	}
}
