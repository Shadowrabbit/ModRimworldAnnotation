using System;

namespace RimWorld
{
	// Token: 0x0200146E RID: 5230
	[DefOf]
	public static class IssueDefOf
	{
		// Token: 0x06007D60 RID: 32096 RVA: 0x002C4C7C File Offset: 0x002C2E7C
		static IssueDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IssueDefOf));
		}

		// Token: 0x04004E0B RID: 19979
		[MayRequireIdeology]
		public static IssueDef Charity;

		// Token: 0x04004E0C RID: 19980
		[MayRequireIdeology]
		public static IssueDef Ranching;

		// Token: 0x04004E0D RID: 19981
		[MayRequireIdeology]
		public static IssueDef Blindness;
	}
}
