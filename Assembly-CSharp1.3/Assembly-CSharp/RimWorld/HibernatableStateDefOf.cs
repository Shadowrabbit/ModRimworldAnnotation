using System;

namespace RimWorld
{
	// Token: 0x02001450 RID: 5200
	[DefOf]
	public static class HibernatableStateDefOf
	{
		// Token: 0x06007D43 RID: 32067 RVA: 0x002C4A8F File Offset: 0x002C2C8F
		static HibernatableStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HibernatableStateDefOf));
		}

		// Token: 0x04004CF2 RID: 19698
		public static HibernatableStateDef Running;

		// Token: 0x04004CF3 RID: 19699
		public static HibernatableStateDef Starting;

		// Token: 0x04004CF4 RID: 19700
		public static HibernatableStateDef Hibernating;
	}
}
