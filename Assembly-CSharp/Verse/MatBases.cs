using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000879 RID: 2169
	[StaticConstructorOnStartup]
	public static class MatBases
	{
		// Token: 0x04002584 RID: 9604
		public static readonly Material LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay", -1);

		// Token: 0x04002585 RID: 9605
		public static readonly Material SunShadow = MatLoader.LoadMat("Lighting/SunShadow", -1);

		// Token: 0x04002586 RID: 9606
		public static readonly Material SunShadowFade = MatBases.SunShadow;

		// Token: 0x04002587 RID: 9607
		public static readonly Material EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow", -1);

		// Token: 0x04002588 RID: 9608
		public static readonly Material IndoorMask = MatLoader.LoadMat("Misc/IndoorMask", -1);

		// Token: 0x04002589 RID: 9609
		public static readonly Material FogOfWar = MatLoader.LoadMat("Misc/FogOfWar", -1);

		// Token: 0x0400258A RID: 9610
		public static readonly Material Snow = MatLoader.LoadMat("Misc/Snow", -1);
	}
}
