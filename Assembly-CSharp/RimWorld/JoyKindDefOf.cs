using System;

namespace RimWorld
{
	// Token: 0x02001C54 RID: 7252
	[DefOf]
	public static class JoyKindDefOf
	{
		// Token: 0x06009F57 RID: 40791 RVA: 0x0006A234 File Offset: 0x00068434
		static JoyKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(JoyKindDefOf));
		}

		// Token: 0x04006877 RID: 26743
		public static JoyKindDef Meditative;

		// Token: 0x04006878 RID: 26744
		public static JoyKindDef Social;

		// Token: 0x04006879 RID: 26745
		public static JoyKindDef Gluttonous;
	}
}
