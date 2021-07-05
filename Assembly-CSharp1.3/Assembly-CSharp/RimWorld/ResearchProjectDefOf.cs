using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001441 RID: 5185
	[DefOf]
	public static class ResearchProjectDefOf
	{
		// Token: 0x06007D34 RID: 32052 RVA: 0x002C4990 File Offset: 0x002C2B90
		static ResearchProjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ResearchProjectDefOf));
		}

		// Token: 0x04004CA0 RID: 19616
		public static ResearchProjectDef CarpetMaking;

		// Token: 0x04004CA1 RID: 19617
		public static ResearchProjectDef MicroelectronicsBasics;
	}
}
