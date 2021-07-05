using System;

namespace RimWorld
{
	// Token: 0x02001440 RID: 5184
	[DefOf]
	public static class SitePartDefOf
	{
		// Token: 0x06007D33 RID: 32051 RVA: 0x002C497F File Offset: 0x002C2B7F
		static SitePartDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SitePartDefOf));
		}

		// Token: 0x04004C93 RID: 19603
		public static SitePartDef Outpost;

		// Token: 0x04004C94 RID: 19604
		public static SitePartDef Turrets;

		// Token: 0x04004C95 RID: 19605
		public static SitePartDef Manhunters;

		// Token: 0x04004C96 RID: 19606
		public static SitePartDef SleepingMechanoids;

		// Token: 0x04004C97 RID: 19607
		public static SitePartDef AmbushHidden;

		// Token: 0x04004C98 RID: 19608
		public static SitePartDef AmbushEdge;

		// Token: 0x04004C99 RID: 19609
		public static SitePartDef PreciousLump;

		// Token: 0x04004C9A RID: 19610
		public static SitePartDef PossibleUnknownThreatMarker;

		// Token: 0x04004C9B RID: 19611
		public static SitePartDef BanditCamp;

		// Token: 0x04004C9C RID: 19612
		[MayRequireIdeology]
		public static SitePartDef WorshippedTerminal;

		// Token: 0x04004C9D RID: 19613
		[MayRequireIdeology]
		public static SitePartDef AncientComplex;

		// Token: 0x04004C9E RID: 19614
		[MayRequireIdeology]
		public static SitePartDef AncientAltar;

		// Token: 0x04004C9F RID: 19615
		[MayRequireIdeology]
		public static SitePartDef Archonexus;
	}
}
