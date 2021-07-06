using System;

namespace RimWorld
{
	// Token: 0x02001C8F RID: 7311
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x06009F92 RID: 40850 RVA: 0x0006A61F File Offset: 0x0006881F
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x04006BF7 RID: 27639
		public static HibernatableStateDef Running;

		// Token: 0x04006BF8 RID: 27640
		public static HibernatableStateDef Starting;

		// Token: 0x04006BF9 RID: 27641
		public static HibernatableStateDef Hibernating;
	}
}
