using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145C RID: 5212
	[DefOf]
	public static class ClamorDefOf
	{
		// Token: 0x06007D4F RID: 32079 RVA: 0x002C4B5B File Offset: 0x002C2D5B
		static ClamorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ClamorDefOf));
		}

		// Token: 0x04004D3D RID: 19773
		public static ClamorDef Movement;

		// Token: 0x04004D3E RID: 19774
		public static ClamorDef Harm;

		// Token: 0x04004D3F RID: 19775
		public static ClamorDef Construction;

		// Token: 0x04004D40 RID: 19776
		public static ClamorDef Impact;

		// Token: 0x04004D41 RID: 19777
		public static ClamorDef Ability;
	}
}
