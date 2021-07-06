using System;

namespace RimWorld
{
	// Token: 0x02001C67 RID: 7271
	[DefOf]
	public static class TimeAssignmentDefOf
	{
		// Token: 0x06009F6A RID: 40810 RVA: 0x0006A377 File Offset: 0x00068577
		static TimeAssignmentDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TimeAssignmentDefOf));
		}

		// Token: 0x04006931 RID: 26929
		public static TimeAssignmentDef Anything;

		// Token: 0x04006932 RID: 26930
		public static TimeAssignmentDef Work;

		// Token: 0x04006933 RID: 26931
		public static TimeAssignmentDef Joy;

		// Token: 0x04006934 RID: 26932
		public static TimeAssignmentDef Sleep;

		// Token: 0x04006935 RID: 26933
		[MayRequireRoyalty]
		public static TimeAssignmentDef Meditate;
	}
}
