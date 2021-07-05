using System;

namespace RimWorld
{
	// Token: 0x02001479 RID: 5241
	[DefOf]
	public static class GauranlenTreeModeDefOf
	{
		// Token: 0x06007D6B RID: 32107 RVA: 0x002C4D37 File Offset: 0x002C2F37
		static GauranlenTreeModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GauranlenTreeModeDefOf));
		}

		// Token: 0x04004E32 RID: 20018
		[MayRequireIdeology]
		public static GauranlenTreeModeDef Gaumaker;
	}
}
