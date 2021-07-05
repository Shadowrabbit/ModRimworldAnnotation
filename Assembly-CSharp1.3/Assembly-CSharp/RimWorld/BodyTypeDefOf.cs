using System;

namespace RimWorld
{
	// Token: 0x0200145E RID: 5214
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x06007D51 RID: 32081 RVA: 0x002C4B7D File Offset: 0x002C2D7D
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDefOf));
		}

		// Token: 0x04004D43 RID: 19779
		public static BodyTypeDef Male;

		// Token: 0x04004D44 RID: 19780
		public static BodyTypeDef Female;

		// Token: 0x04004D45 RID: 19781
		public static BodyTypeDef Thin;

		// Token: 0x04004D46 RID: 19782
		public static BodyTypeDef Hulk;

		// Token: 0x04004D47 RID: 19783
		public static BodyTypeDef Fat;
	}
}
