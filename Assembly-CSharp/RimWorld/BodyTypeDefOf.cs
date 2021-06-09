using System;

namespace RimWorld
{
	// Token: 0x02001C9E RID: 7326
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x06009FA1 RID: 40865 RVA: 0x0006A71E File Offset: 0x0006891E
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDefOf));
		}

		// Token: 0x04006C50 RID: 27728
		public static BodyTypeDef Male;

		// Token: 0x04006C51 RID: 27729
		public static BodyTypeDef Female;

		// Token: 0x04006C52 RID: 27730
		public static BodyTypeDef Thin;

		// Token: 0x04006C53 RID: 27731
		public static BodyTypeDef Hulk;

		// Token: 0x04006C54 RID: 27732
		public static BodyTypeDef Fat;
	}
}
