using System;

namespace RimWorld
{
	// Token: 0x02000A1E RID: 2590
	[Flags]
	public enum FilthSourceFlags
	{
		// Token: 0x0400222B RID: 8747
		None = 0,
		// Token: 0x0400222C RID: 8748
		Terrain = 1,
		// Token: 0x0400222D RID: 8749
		Natural = 2,
		// Token: 0x0400222E RID: 8750
		Unnatural = 4,
		// Token: 0x0400222F RID: 8751
		Pawn = 8,
		// Token: 0x04002230 RID: 8752
		Any = 15
	}
}
