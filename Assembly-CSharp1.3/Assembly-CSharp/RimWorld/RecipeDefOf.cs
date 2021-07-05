using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001418 RID: 5144
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x06007D0B RID: 32011 RVA: 0x002C46D7 File Offset: 0x002C28D7
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x04004934 RID: 18740
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04004935 RID: 18741
		public static RecipeDef CookMealSimple;

		// Token: 0x04004936 RID: 18742
		public static RecipeDef InstallPegLeg;
	}
}
