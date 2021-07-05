using System;

namespace RimWorld
{
	// Token: 0x0200146F RID: 5231
	[DefOf]
	public static class MemeDefOf
	{
		// Token: 0x06007D61 RID: 32097 RVA: 0x002C4C8D File Offset: 0x002C2E8D
		static MemeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeDefOf));
		}

		// Token: 0x04004E0E RID: 19982
		[MayRequireIdeology]
		public static MemeDef Nudism;

		// Token: 0x04004E0F RID: 19983
		[MayRequireIdeology]
		public static MemeDef MaleSupremacy;

		// Token: 0x04004E10 RID: 19984
		[MayRequireIdeology]
		public static MemeDef FemaleSupremacy;

		// Token: 0x04004E11 RID: 19985
		[MayRequireIdeology]
		public static MemeDef Rancher;

		// Token: 0x04004E12 RID: 19986
		[MayRequireIdeology]
		public static MemeDef TreeConnection;

		// Token: 0x04004E13 RID: 19987
		[MayRequireIdeology]
		public static MemeDef Blindsight;

		// Token: 0x04004E14 RID: 19988
		[MayRequireIdeology]
		public static MemeDef Transhumanist;
	}
}
