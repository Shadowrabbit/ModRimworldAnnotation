using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA0 RID: 7328
	[DefOf]
	public static class ShaderTypeDefOf
	{
		// Token: 0x06009FA3 RID: 40867 RVA: 0x0006A740 File Offset: 0x00068940
		static ShaderTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShaderTypeDefOf));
		}

		// Token: 0x04006C56 RID: 27734
		public static ShaderTypeDef Cutout;

		// Token: 0x04006C57 RID: 27735
		public static ShaderTypeDef CutoutComplex;

		// Token: 0x04006C58 RID: 27736
		public static ShaderTypeDef Transparent;

		// Token: 0x04006C59 RID: 27737
		public static ShaderTypeDef MetaOverlay;

		// Token: 0x04006C5A RID: 27738
		public static ShaderTypeDef EdgeDetect;
	}
}
