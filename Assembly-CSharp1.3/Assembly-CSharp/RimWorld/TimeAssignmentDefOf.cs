using System;

namespace RimWorld
{
	// Token: 0x02001427 RID: 5159
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x06007D1A RID: 32026 RVA: 0x002C47D6 File Offset: 0x002C29D6
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		// Token: 0x040049AF RID: 18863
		public static TimeAssignmentDef Anything;

		// Token: 0x040049B0 RID: 18864
		public static TimeAssignmentDef Work;

		// Token: 0x040049B1 RID: 18865
		public static TimeAssignmentDef Joy;

		// Token: 0x040049B2 RID: 18866
		public static TimeAssignmentDef Sleep;

		// Token: 0x040049B3 RID: 18867
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
