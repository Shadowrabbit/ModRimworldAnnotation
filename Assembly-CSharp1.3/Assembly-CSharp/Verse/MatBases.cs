using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004CF RID: 1231
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04001744 RID: 5956
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04001745 RID: 5957
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04001746 RID: 5958
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04001747 RID: 5959
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04001748 RID: 5960
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04001749 RID: 5961
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x0400174A RID: 5962
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
