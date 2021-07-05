using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C58 RID: 7256
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x06009F5B RID: 40795 RVA: 0x0006A278 File Offset: 0x00068478
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x040068C0 RID: 26816
		public static RecipeDef RemoveBodyPart;

		// Token: 0x040068C1 RID: 26817
		public static RecipeDef CookMealSimple;

		// Token: 0x040068C2 RID: 26818
		public static RecipeDef InstallPegLeg;
	}
}
