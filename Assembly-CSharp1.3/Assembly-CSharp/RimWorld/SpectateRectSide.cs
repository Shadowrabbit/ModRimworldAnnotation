using System;

namespace RimWorld
{
	// Token: 0x020013EE RID: 5102
	[Flags]
	public enum SpectateRectSide
	{
		// Token: 0x040044C9 RID: 17609
		None = 0,
		// Token: 0x040044CA RID: 17610
		Up = 1,
		// Token: 0x040044CB RID: 17611
		Right = 2,
		// Token: 0x040044CC RID: 17612
		Down = 4,
		// Token: 0x040044CD RID: 17613
		Left = 8,
		// Token: 0x040044CE RID: 17614
		Vertical = 5,
		// Token: 0x040044CF RID: 17615
		Horizontal = 10,
		// Token: 0x040044D0 RID: 17616
		All = 15
	}
}
