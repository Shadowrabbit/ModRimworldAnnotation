using System;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04000DCF RID: 3535
		None = 0,
		// Token: 0x04000DD0 RID: 3536
		Things = 1,
		// Token: 0x04000DD1 RID: 3537
		FogOfWar = 2,
		// Token: 0x04000DD2 RID: 3538
		Buildings = 4,
		// Token: 0x04000DD3 RID: 3539
		GroundGlow = 8,
		// Token: 0x04000DD4 RID: 3540
		Terrain = 16,
		// Token: 0x04000DD5 RID: 3541
		Roofs = 32,
		// Token: 0x04000DD6 RID: 3542
		Snow = 64,
		// Token: 0x04000DD7 RID: 3543
		Zone = 128,
		// Token: 0x04000DD8 RID: 3544
		PowerGrid = 256,
		// Token: 0x04000DD9 RID: 3545
		BuildingsDamage = 512
	}
}
