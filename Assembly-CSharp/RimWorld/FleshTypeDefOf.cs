using System;

namespace RimWorld
{
	// Token: 0x02001C85 RID: 7301
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x06009F88 RID: 40840 RVA: 0x0006A575 File Offset: 0x00068775
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x04006BC7 RID: 27591
		public static FleshTypeDef Normal;

		// Token: 0x04006BC8 RID: 27592
		public static FleshTypeDef Mechanoid;

		// Token: 0x04006BC9 RID: 27593
		public static FleshTypeDef Insectoid;
	}
}
