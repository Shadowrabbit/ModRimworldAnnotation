using System;

namespace Verse
{
	// Token: 0x020001C9 RID: 457
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04000AE1 RID: 2785
		None = 0,
		// Token: 0x04000AE2 RID: 2786
		Verts = 1,
		// Token: 0x04000AE3 RID: 2787
		Tris = 2,
		// Token: 0x04000AE4 RID: 2788
		Colors = 4,
		// Token: 0x04000AE5 RID: 2789
		UVs = 8,
		// Token: 0x04000AE6 RID: 2790
		All = 127
	}
}
