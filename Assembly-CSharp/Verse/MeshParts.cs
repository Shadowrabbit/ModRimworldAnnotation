using System;

namespace Verse
{
	// Token: 0x0200028A RID: 650
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x04000DC0 RID: 3520
		None = 0,
		// Token: 0x04000DC1 RID: 3521
		Verts = 1,
		// Token: 0x04000DC2 RID: 3522
		Tris = 2,
		// Token: 0x04000DC3 RID: 3523
		Colors = 4,
		// Token: 0x04000DC4 RID: 3524
		UVs = 8,
		// Token: 0x04000DC5 RID: 3525
		All = 127
	}
}
