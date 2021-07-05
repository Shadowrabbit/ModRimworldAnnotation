using System;

namespace Verse
{
	// Token: 0x020001CB RID: 459
	[Flags]
	public enum MapMeshFlag
	{
		// Token: 0x04000AF1 RID: 2801
		None = 0,
		// Token: 0x04000AF2 RID: 2802
		Things = 1,
		// Token: 0x04000AF3 RID: 2803
		FogOfWar = 2,
		// Token: 0x04000AF4 RID: 2804
		Buildings = 4,
		// Token: 0x04000AF5 RID: 2805
		GroundGlow = 8,
		// Token: 0x04000AF6 RID: 2806
		Terrain = 16,
		// Token: 0x04000AF7 RID: 2807
		Roofs = 32,
		// Token: 0x04000AF8 RID: 2808
		Snow = 64,
		// Token: 0x04000AF9 RID: 2809
		Zone = 128,
		// Token: 0x04000AFA RID: 2810
		PowerGrid = 256,
		// Token: 0x04000AFB RID: 2811
		BuildingsDamage = 512
	}
}
