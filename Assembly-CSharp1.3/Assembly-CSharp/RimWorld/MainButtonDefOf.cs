using System;

namespace RimWorld
{
	// Token: 0x02001416 RID: 5142
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x06007D09 RID: 32009 RVA: 0x002C46B5 File Offset: 0x002C28B5
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x0400492A RID: 18730
		public static MainButtonDef Inspect;

		// Token: 0x0400492B RID: 18731
		public static MainButtonDef Architect;

		// Token: 0x0400492C RID: 18732
		public static MainButtonDef Research;

		// Token: 0x0400492D RID: 18733
		public static MainButtonDef Menu;

		// Token: 0x0400492E RID: 18734
		public static MainButtonDef World;

		// Token: 0x0400492F RID: 18735
		public static MainButtonDef Quests;

		// Token: 0x04004930 RID: 18736
		public static MainButtonDef Factions;

		// Token: 0x04004931 RID: 18737
		[MayRequireIdeology]
		public static MainButtonDef Ideos;
	}
}
