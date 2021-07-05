using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001460 RID: 5216
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x06007D53 RID: 32083 RVA: 0x002C4B9F File Offset: 0x002C2D9F
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDefOf));
		}

		// Token: 0x04004D49 RID: 19785
		public static ShaderTypeDef Cutout;

		// Token: 0x04004D4A RID: 19786
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x04004D4B RID: 19787
		public static ShaderTypeDef Transparent;

		// Token: 0x04004D4C RID: 19788
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04004D4D RID: 19789
		public static ShaderTypeDef EdgeDetect;
	}
}
