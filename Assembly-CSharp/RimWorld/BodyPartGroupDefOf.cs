using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5F RID: 7263
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x06009F62 RID: 40802 RVA: 0x0006A2EF File Offset: 0x000684EF
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x04006905 RID: 26885
		public static BodyPartGroupDef Torso;

		// Token: 0x04006906 RID: 26886
		public static BodyPartGroupDef Legs;

		// Token: 0x04006907 RID: 26887
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04006908 RID: 26888
		public static BodyPartGroupDef RightHand;

		// Token: 0x04006909 RID: 26889
		public static BodyPartGroupDef FullHead;

		// Token: 0x0400690A RID: 26890
		public static BodyPartGroupDef UpperHead;

		// Token: 0x0400690B RID: 26891
		public static BodyPartGroupDef Eyes;
	}
}
