using System;

namespace RimWorld
{
	// Token: 0x02001C56 RID: 7254
	[DefOf]
	public static class MainButtonDefOf
	{
		// Token: 0x06009F59 RID: 40793 RVA: 0x0006A256 File Offset: 0x00068456
		static MainButtonDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MainButtonDefOf));
		}

		// Token: 0x040068B7 RID: 26807
		public static MainButtonDef Inspect;

		// Token: 0x040068B8 RID: 26808
		public static MainButtonDef Architect;

		// Token: 0x040068B9 RID: 26809
		public static MainButtonDef Research;

		// Token: 0x040068BA RID: 26810
		public static MainButtonDef Menu;

		// Token: 0x040068BB RID: 26811
		public static MainButtonDef World;

		// Token: 0x040068BC RID: 26812
		public static MainButtonDef Quests;

		// Token: 0x040068BD RID: 26813
		public static MainButtonDef Factions;
	}
}
