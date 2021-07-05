using System;

namespace RimWorld
{
	// Token: 0x02001422 RID: 5154
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x06007D15 RID: 32021 RVA: 0x002C4781 File Offset: 0x002C2981
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x040049A3 RID: 18851
		public static TrainableDef Tameness;

		// Token: 0x040049A4 RID: 18852
		public static TrainableDef Obedience;

		// Token: 0x040049A5 RID: 18853
		public static TrainableDef Release;
	}
}
