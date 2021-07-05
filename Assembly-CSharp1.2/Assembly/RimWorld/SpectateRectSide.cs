using System;

namespace RimWorld
{
	// Token: 0x02001C29 RID: 7209
	[Flags]
	public enum SpectateRectSide
	{
		// Token: 0x04006515 RID: 25877
		None = 0,
		// Token: 0x04006516 RID: 25878
		Up = 1,
		// Token: 0x04006517 RID: 25879
		Right = 2,
		// Token: 0x04006518 RID: 25880
		Down = 4,
		// Token: 0x04006519 RID: 25881
		Left = 8,
		// Token: 0x0400651A RID: 25882
		Vertical = 5,
		// Token: 0x0400651B RID: 25883
		Horizontal = 10,
		// Token: 0x0400651C RID: 25884
		All = 15
	}
}
