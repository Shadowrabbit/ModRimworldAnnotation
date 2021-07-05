using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141F RID: 5151
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x06007D12 RID: 32018 RVA: 0x002C474E File Offset: 0x002C294E
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x04004981 RID: 18817
		public static BodyPartGroupDef Torso;

		// Token: 0x04004982 RID: 18818
		public static BodyPartGroupDef Legs;

		// Token: 0x04004983 RID: 18819
		public static BodyPartGroupDef LeftHand;

		// Token: 0x04004984 RID: 18820
		public static BodyPartGroupDef RightHand;

		// Token: 0x04004985 RID: 18821
		public static BodyPartGroupDef FullHead;

		// Token: 0x04004986 RID: 18822
		public static BodyPartGroupDef UpperHead;

		// Token: 0x04004987 RID: 18823
		public static BodyPartGroupDef Eyes;
	}
}
