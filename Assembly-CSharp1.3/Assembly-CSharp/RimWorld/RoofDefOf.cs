using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141E RID: 5150
	[DefOf]
	public static class RoofDefOf
	{
		// Token: 0x06007D11 RID: 32017 RVA: 0x002C473D File Offset: 0x002C293D
		static RoofDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoofDefOf));
		}

		// Token: 0x0400497E RID: 18814
		public static RoofDef RoofConstructed;

		// Token: 0x0400497F RID: 18815
		public static RoofDef RoofRockThick;

		// Token: 0x04004980 RID: 18816
		public static RoofDef RoofRockThin;
	}
}
