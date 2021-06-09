using System;

namespace RimWorld
{
	// Token: 0x02001C62 RID: 7266
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x06009F65 RID: 40805 RVA: 0x0006A322 File Offset: 0x00068522
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x04006925 RID: 26917
		public static TrainableDef Tameness;

		// Token: 0x04006926 RID: 26918
		public static TrainableDef Obedience;

		// Token: 0x04006927 RID: 26919
		public static TrainableDef Release;
	}
}
